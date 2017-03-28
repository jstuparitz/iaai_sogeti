using IAA.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Business.Model
{
	public enum AuctionStatus { NotStarted, Active, Closed }

	public class AuctionItem
	{

		public AuctionItem()
		{
			Status = AuctionStatus.NotStarted;
		}

		public string Id
		{
			get { return String.Format("A{0}", Item.VIN); }
		}

		public AuctionStatus Status { get; set; }
		public Money StartingPrice { get; set; }
		public Money MaxBid { get; set; }
		public Vehicle Item { get; set; }

		public UserIdentity LastBidder { get; set; }


		public void OpenAuction(Money startPrice)
		{
			StartingPrice = startPrice;
			MaxBid = StartingPrice;
			LastBidder = new UserIdentity("None");
			Status = AuctionStatus.Active;
		}

		public void Close()
		{
			Status = AuctionStatus.Closed;
		}

		public bool AcceptsBids()
		{
			return Status == AuctionStatus.Active;
		}

		public bool PlaceBid(UserIdentity user, Money amt)
		{
			if (!AcceptsBids()) throw new ApplicationException("Cannot accept bids");
			if (amt.IsGreaterThan(MaxBid))
			{
				MaxBid = amt;
				LastBidder = user;
				return true;
			}

			return false;
		}



	}
}
