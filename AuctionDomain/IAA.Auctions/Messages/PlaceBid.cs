using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Messages
{
	public class PlaceBid
	{

		public string UserId { get; set; }
		public int Amount { get; set; }
	}
}
