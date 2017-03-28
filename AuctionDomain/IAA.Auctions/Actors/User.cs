using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Actors
{
	public class User : ReceiveActor
	{

		private string _userId;

		public User(string userId)
		{
			_userId = userId;
			WaitForInput();
		}

		public void WaitForInput()
		{
			Receive<Messages.CommandBid>(cmd =>
			{
				PlaceBid(cmd);
			});

			Receive<Messages.BidResult>(res =>
			{
				ProcessResult(res);
			});

		}

		public void PlaceBid(Messages.CommandBid bid)
		{
			//Console.WriteLine("USER {0}: Place Bid {1}", bid.Author, bid.Amount);
			var selection = Context.ActorSelection(Tools.Names.AuctionActorPath(bid.AuctionId));
			selection.Tell(new Messages.PlaceBid() { UserId = _userId, Amount = bid.Amount }, Self);
		}

		public void ProcessResult(Messages.BidResult res)
		{
			Trace.WriteLine("Price " + res.CurrentPrice.ToString());
		}



	}
}
