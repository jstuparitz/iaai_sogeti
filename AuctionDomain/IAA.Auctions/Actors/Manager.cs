using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAA.Auctions.Messages;

namespace IAA.Auctions.Actors
{
	public class Manager: ReceiveActor
	{

		public Manager()
		{
			WaitForInput();
		}

		public void WaitForInput()
		{
			Receive<Messages.MessageToServer>(msg =>
			{
				ProcessMessage(msg);
			});

			Receive<Messages.AuctionResult>(ares =>
			{
				ProcessAuctionResult(ares);
			});

			Receive<Messages.Error>(err =>
			{
				ProcessError(err);
			});
		}


		public void ProcessAuctionResult(Messages.AuctionResult ares)
		{
			Console.WriteLine("Auction result: {0} won at price {1}", ares.Winner, ares.FinalPrice);
		}


		public void ProcessMessage(Messages.MessageToServer msg)
		{
			switch (msg.Atr("cmd"))
			{
				case "open":
					OpenAuction(new CommandOpen() {
						Author = msg.Atr("user"),
						VehicleMake = msg.Atr("make"),
						VehicleYear = msg.AtrInt("year"),
						VIN = msg.Atr("VIN"),

						StartAmount = msg.AtrInt("value")
					});
					break;

				case "close":
					CloseAuction(new CommandClose() { Author = msg.Atr("user"), AuctionId = msg.Atr("auction") });
					break;

				case "bid":
					PlaceBid(new CommandBid() { Author = msg.Atr("user"), AuctionId = msg.Atr("auction"), Amount = msg.AtrInt("value") });
					break;

				default:
					// Ignore for now
					break;
			}
		}


		public void ProcessError(Messages.Error err)
		{
			Console.WriteLine("{0} received ERROR in {1}: {2} {3}", err.UserId, err.Location, err.Reason, err.Data);
			//Trace.WriteLine(String.Format("ERROR in {0}: {1} {2} ",err.Location, err.Reason, err.Data));
		}

		public void OpenAuction(Messages.CommandOpen cmd)
		{

			var dmAuction = CreateAuctionDomainObject(cmd);

			//Console.WriteLine("MANAGER: Open Auction {0}/{1}", cmd.AuctionId, cmd.StartAmount);
			string actorId = Tools.Names.AuctionActorId(dmAuction.Id);
			var newAuction =
							Context.ActorOf(Props.Create(() =>
									new Actors.Auction(dmAuction)),
									actorId);
			newAuction.Tell(new Messages.StartAuction()
			{
				Price = cmd.StartAmount
			});
			//Console.WriteLine("   new auction: {0}", newAuction.Path);
		}

		private Business.Model.AuctionItem CreateAuctionDomainObject(CommandOpen cmd)
		{
			var vehicle = new Business.Model.Vehicle() {
				Make = cmd.VehicleMake,
				VIN = cmd.VIN,
				Year = cmd.VehicleYear };
			var res = new Business.Model.AuctionItem() {
				Item = vehicle
			};
			return res;
		}

		public void CloseAuction(Messages.CommandClose cmd)
		{
			//Console.WriteLine("MANAGER: Close Auction {0}", cmd.AuctionId);
			var auction = Context.Child(Tools.Names.AuctionActorId(cmd.AuctionId));
			if (auction.Equals(ActorRefs.Nobody))
			{
				Self.Tell(new Messages.Error()
				{
					UserId = cmd.Author,
					Location = "Manager.CloseAuction",
					Reason = "Invalid auction ID",
					Data = cmd.AuctionId
				});
				return;
			}
			auction.Tell(new Messages.CloseAuction());
		}

		public void PlaceBid(Messages.CommandBid cmd)
		{
			//Console.WriteLine("MANAGER: {0} place Bid {1}", cmd.Author, cmd.Amount);

			string userActorId = Tools.Names.UserActorId(cmd.Author);
			// Find existing or create new user
			var user = Context.Child(userActorId);
			if (user.Equals(ActorRefs.Nobody))
			{
				//Console.WriteLine("    creating new user {0}", cmd.Author);
				user =
						Context.ActorOf(Props.Create(() =>
								new Actors.User(cmd.Author)),
								userActorId);
			}
			user.Tell(cmd);
		}


	}
}
