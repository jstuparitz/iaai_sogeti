using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAA.Business.Model
{
	public class UserIdentity
	{

		public UserIdentity(string name)
		{
			Name = name;
		}

		public string Name { get; private set;  }


	}
}
