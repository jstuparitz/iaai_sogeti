using System;
using Akka.Actor;
using IAA.Auctions;
using System.Xml.Linq;

namespace IAA.Auctions
{
	public class MessageBus
	{

		ActorSystem fabric;
		IActorRef actorManager;

		public void Open()
		{
			fabric = ActorSystem.Create("IAA");
			actorManager = fabric.ActorOf(Props.Create(() => new Actors.Manager()), Tools.Names.AuctionManagerId);
		}

		public void Put(XElement msg)
		{
			actorManager.Tell(new Messages.MessageToServer
			{
				RawMessage = msg
			});
		}

		public void Close()
		{
			fabric.Terminate();
		}
	}
}
