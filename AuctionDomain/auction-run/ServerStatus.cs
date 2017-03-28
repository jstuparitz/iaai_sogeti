using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace simulate.auction
{
	public class ServerStatus
	{

		public string ErrorMessage = "";
		public string FileName;
		public XDocument doc = null;

		public bool HasErrors
		{
			get { return !String.IsNullOrEmpty(ErrorMessage); }
		}

		public ServerStatus SetError(string loc, Exception ex)
		{
			ErrorMessage = String.Format("Exception in {0}: {1}", loc, ex.Message);
			return this;
		}

		public ServerStatus SetError(string loc, string msg)
		{
			ErrorMessage = String.Format("Error in {0}: {1}", loc, msg);
			return this;
		}
	}
}
