using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IAA.Auctions.Messages
{
	public class MessageToServer
	{
		public XElement RawMessage { get; set; }

		public string Atr(string aname)
		{
			if (RawMessage == null) return "";
			var xa = RawMessage.Attribute(aname);
			if (xa == null) return "";
			return xa.Value;
		}

		public int AtrInt(string aname)
		{
			if (RawMessage == null) return 0;
			var xa = RawMessage.Attribute(aname);
			if (xa == null) return 0;
			return Convert.ToInt32(xa.Value);
		}
	}
}
