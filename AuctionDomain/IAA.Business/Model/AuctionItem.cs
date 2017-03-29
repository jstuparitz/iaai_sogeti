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

		public AuctionItem(Vehicle vehicle)
		{
			Status = AuctionStatus.NotStarted;
			Item = vehicle ?? throw new ArgumentNullException();
		}

		public string Id
		{
			get { return String.Format("A{0}", Item.VIN); }
		}

		public AuctionStatus Status { get; private set; }
		public Money StartingPrice { get; private set; }
		public Money MaxBid { get; private set; }
		public Vehicle Item { get; private set; }

		public UserIdentity LastBidder { get; private set; }


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

		public bool PlaceBid(UserIdentity user, Money amount)
		{
			if (!AcceptsBids()) throw new ApplicationException("Cannot accept bids");
			if (amount.IsGreaterThan(MaxBid))
			{
				MaxBid = amount;
				LastBidder = user;
				return true;
			}

			return false;
		}



	}
}
