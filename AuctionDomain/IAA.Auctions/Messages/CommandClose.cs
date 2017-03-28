using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Messages
{
	public class CommandClose: Command
	{
		public string AuctionId { get; set; }
	}
}
