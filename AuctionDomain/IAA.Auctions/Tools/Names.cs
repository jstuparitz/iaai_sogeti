using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Tools
{
	public class Names
	{

		public static string AuctionServerId = "AuctionServer";
		public static string AuctionManagerId = "AuctionManager";

		public static string AuctionId(string VIN)
		{
			return String.Format("A{0}", VIN);
		}
		public static string AuctionActorId(string auctionId)
		{
			return String.Format("auction-{0}", auctionId);
		}
		public static string UserActorId(string userId)
		{
			return String.Format("user-{0}", userId);
		}
		public static string AuctionActorPath(string auctionId)
		{
			return String.Format("/user/{0}/{1}", AuctionManagerId, AuctionActorId(auctionId));
		}
		public static string UserActorPath(string userId)
		{
			return String.Format("/user/{0}/{1}", AuctionManagerId, UserActorId(userId));
		}

		public static string AuctionManagerPath()
		{
			return String.Format("/user/{0}", AuctionManagerId);
		}

	}
}
