using MasDev.Common.Modeling;
using System.Collections.Generic;

namespace MasDev.Common.Share.Tests.Models
{
	public class Interest : IModel
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual IList<Person> PersonInterestedIn { get; set; }
	}
}

