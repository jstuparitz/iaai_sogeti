using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Messages
{
	public class Error
	{
		public string Location { get; set; }
		public string Reason { get; set; }
		public string Data { get; set; }
		public string UserId { get; set; }

	}
}
