using MasDev.Common.Modeling;
using SQLite.Net.Attributes;
using System.Collections.Generic;

namespace MasDev.Common.Share.Tests.Models
{
	public class Person : IModel
	{
		[PrimaryKey, AutoIncrement]
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual int Age { get; set; }

		public virtual IList<Interest> Interests { get; set; }
	}
}

