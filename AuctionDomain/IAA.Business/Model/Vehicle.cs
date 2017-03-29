using IAA.Business.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Business.Model
{
	public class Vehicle
	{

		public Vehicle(string make, int year, string vin)
		{
			Make = make;
			Year = year;
			VIN = vin;
		}

		public string VIN { get; private set; }
		public string Make { get; private set; }
		public int Year { get; private set; }
	}
}
