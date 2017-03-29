using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Actors
{


	public class Auction : ReceiveActor
	{

		private Business.Model.AuctionItem auctionItem;


		public Auction()
		{
			WaitForInput();
		}


		public void WaitForInput()
		{
			Receive<Messages.PlaceBid>(bid =>
			{
				ProcessBid(bid);
			});

			Receive<Messages.CommandStartAuction>(sa =>
			{
				InitAuction(sa);
			});

			Receive<Messages.CloseAuction>(ca =>
			{
				CloseAuction(ca);
			});

		}
		public void InitAuction(Messages.CommandStartAuction auction) {
			//Console.WriteLine("Auction {0}: Open with {1}", auction.AuctionId, auction.Price);
			auctionItem = CreateAuctionDomainObject(auction);
			auctionItem.OpenAuction(new Business.Model.Money(auction.StartAmount));
		}

		private Business.Model.AuctionItem CreateAuctionDomainObject(Messages.CommandStartAuction cmd)
		{
			var vehicle = new Business.Model.Vehicle()
			{
				Make = cmd.VehicleMake,
				VIN = cmd.VIN,
				Year = cmd.VehicleYear
			};
			var res = new Business.Model.AuctionItem()
			{
				Item = vehicle
			};
			return res;
		}

		public void ProcessBid(Messages.PlaceBid bid)
		{

			if (!auctionItem.AcceptsBids())
			{
				var selection = Context.ActorSelection(Tools.Names.AuctionManagerPath());
				selection.Tell(new Messages.Error()
				{
					UserId = bid.UserId,
					Location = "Auction.ProcessBid",
					Reason = "Auction is not active",
					Data = auctionItem.Id
				});
				return;
			}

			bool wasAccepted = false;
			wasAccepted = auctionItem.PlaceBid(new Business.Model.UserIdentity(bid.UserId), new Business.Model.Money(bid.Amount));

			if (wasAccepted)
			{
				Console.WriteLine("Auction {0}: Accepting bid {1} from {2}", auctionItem.Id, bid.Amount, bid.UserId);
			}

			Context.Parent.Tell(new Messages.BidResult()
			{
				Accepted = wasAccepted,
				CurrentPrice = auctionItem.MaxBid.GetValue(),
				LastBidder = auctionItem.LastBidder.Name
			});
		}


		public void CloseAuction(Messages.CloseAuction close)
		{

			auctionItem.Close();

			//Console.WriteLine("Auction {0}: Close");
			var selection = Context.ActorSelection(Tools.Names.AuctionManagerPath());
			selection.Tell(new Messages.AuctionResult()
			{
				FinalPrice = auctionItem.MaxBid.GetValue(),
				Winner = auctionItem.LastBidder.Name
			}
			, Self);

		}

	}
}
