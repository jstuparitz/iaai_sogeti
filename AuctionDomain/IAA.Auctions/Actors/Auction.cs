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

		#region Actor routing methods
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

			Receive<Messages.CommandCloseAuction>(ca =>
			{
				CloseAuction(ca);
			});

		}
		#endregion

		#region Start Auction
		public void InitAuction(Messages.CommandStartAuction auction) {
			auctionItem = CreateAuctionDomainObject(auction);
			auctionItem.OpenAuction(new Business.Model.Money(auction.StartAmount));
		}

		private Business.Model.AuctionItem CreateAuctionDomainObject(Messages.CommandStartAuction cmd)
		{
			var vehicle = new Business.Model.Vehicle(cmd.VehicleMake, cmd.VehicleYear, cmd.VIN);
			var res = new Business.Model.AuctionItem(vehicle);
			return res;
		}
		#endregion

		#region Place Bid
		public void ProcessBid(Messages.PlaceBid bid)
		{

			if (!auctionItem.AcceptsBids())
			{
				var managerActor = Context.ActorSelection(Tools.Names.AuctionManagerPath());
				managerActor.Tell(new Messages.Error()
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
		#endregion

		#region Close Auction
		public void CloseAuction(Messages.CommandCloseAuction close)
		{

			auctionItem.Close();

			var managerActor = Context.ActorSelection(Tools.Names.AuctionManagerPath());
			managerActor.Tell(new Messages.AuctionResult()
			{
				FinalPrice = auctionItem.MaxBid.GetValue(),
				Winner = auctionItem.LastBidder.Name
			}
			, Self);

		}
		#endregion

	}
}
