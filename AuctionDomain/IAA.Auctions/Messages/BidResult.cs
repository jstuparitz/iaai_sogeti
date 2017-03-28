using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Messages
{
	public class BidResult
	{
		public int CurrentPrice { get; set; }
		public string LastBidder { get; set; }

		public bool Accepted { get; set; }
	}
}
