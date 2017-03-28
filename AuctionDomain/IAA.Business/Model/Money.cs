using IAA.Business.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Business.Model
{

	public class Money : ValueObject<Money>
	{
		protected int Value { get; set; }

		public Money(): this(0)
		{
		}

		public Money(int value)
		{
			CheckInputValue(value);
			Value = value;
		}

		public int GetValue()
		{
			return Value;
		}

		private void CheckInputValue(int value)
		{
			if (value < 0)
				throw new ApplicationException("Money cannot be less than zero");
		}

		public Money add(Money money)
		{
			return new Money(Value + money.Value);
		}

		public bool IsGreaterThan(Money money)
		{
			return this.Value > money.Value;
		}

		public bool IsGreaterThanOrEqualTo(Money money)
		{
			return this.Value > money.Value || this.Equals(money);
		}

		public bool IsLessThanOrEqualTo(Money money)
		{
			return this.Value < money.Value || this.Equals(money);
		}

		public override string ToString()
		{
			return string.Format("{0}", Value);
		}

		// Equality Implementation

		protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
		{
			return new List<Object>() { Value };
		}
	}
}
