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

		private Business.Model.AuctionItem theAuction;


		public Auction(Business.Model.AuctionItem anAuction)
		{
			theAuction = anAuction;
			WaitForInput();
		}


		public void WaitForInput()
		{
			Receive<Messages.PlaceBid>(bid =>
			{
				ProcessBid(bid);
			});

			Receive<Messages.StartAuction>(sa =>
			{
				InitAuction(sa);
			});

			Receive<Messages.CloseAuction>(ca =>
			{
				CloseAuction(ca);
			});

		}
		public void InitAuction(Messages.StartAuction auction) {
			//Console.WriteLine("Auction {0}: Open with {1}", auction.AuctionId, auction.Price);
			theAuction.OpenAuction(new Business.Model.Money(auction.Price));
		}

		public void ProcessBid(Messages.PlaceBid bid)
		{

			if (!theAuction.AcceptsBids())
			{
				var selection = Context.ActorSelection(Tools.Names.AuctionManagerPath());
				selection.Tell(new Messages.Error()
				{
					UserId = bid.UserId,
					Location = "Auction.ProcessBid",
					Reason = "Auction is not active",
					Data = theAuction.Id
				});
				return;
			}

			bool wasAccepted = false;
			wasAccepted = theAuction.PlaceBid(new Business.Model.UserIdentity(bid.UserId), new Business.Model.Money(bid.Amount));

			if (wasAccepted)
			{
				Console.WriteLine("Auction {0}: Accepting bid {1} from {2}", theAuction.Id, bid.Amount, bid.UserId);
			}

			Context.Parent.Tell(new Messages.BidResult()
			{
				Accepted = wasAccepted,
				CurrentPrice = theAuction.MaxBid.GetValue(),
				LastBidder = theAuction.LastBidder.Name
			});
		}


		public void CloseAuction(Messages.CloseAuction close)
		{

			theAuction.Close();

			//Console.WriteLine("Auction {0}: Close");
			var selection = Context.ActorSelection(Tools.Names.AuctionManagerPath());
			selection.Tell(new Messages.AuctionResult()
			{
				FinalPrice = theAuction.MaxBid.GetValue(),
				Winner = theAuction.LastBidder.Name
			}
			, Self);

		}

	}
}
