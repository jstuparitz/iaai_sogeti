using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Auctions.Messages
{
	public class CommandOpen: Command
	{
		public int VehicleYear { get; set; }
		public string VehicleMake { get; set; }
		public string VIN { get; set; }
		public int StartAmount { get; set; }
	}
}
