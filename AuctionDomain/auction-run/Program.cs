using IAA.Auctions;
using System;
using System.Threading;
using System.Xml.Linq;

namespace simulate.auction
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("IAA Auction Now!");
			var status = new ServerStatus();
			status = ReadSettings(status);
			status = LoadDataFile(status);
			status = RunAuctionSimulation(status);
			ReportStatus(status);
			Console.ReadKey();
		}

		private static ServerStatus ReadSettings(ServerStatus status)
		{
			if (status.HasErrors) return status;
			status.FileName = "auction-play.xml";
			return status;
		}


		private static ServerStatus LoadDataFile(ServerStatus status)
		{
			if (status.HasErrors) return status;
			try
			{
				status.doc = XDocument.Load(status.FileName);
				return status;
			}
			catch (Exception ex)
			{
				return status.SetError("Load data", ex);
			}
		}

		private static ServerStatus RunAuctionSimulation(ServerStatus status)
		{
			if (status.HasErrors) return status;
			try
			{
				MessageBus queue = new MessageBus();
				queue.Open();
				if (status.doc == null) return status.SetError("Simulation", "No data");
				foreach (XElement xe in status.doc.Root.Elements())
				{
					queue.Put(xe);
					Thread.Sleep(500);
				}
				queue.Close();
				return status;
			}
			catch (Exception ex)
			{
				return status.SetError("Simulation", ex);
			}
		}

		private static void ReportStatus(ServerStatus status)
		{
			if (status.HasErrors)
			{
				Console.WriteLine(status.ErrorMessage);
			}
			else
			{
				Console.WriteLine("SUCCESS!");
			}
			Console.WriteLine("Done.");

		}

	}
}