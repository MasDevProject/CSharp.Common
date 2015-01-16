using System;
using MasDev.Common.Data;
using MasDev.Common.Modeling;
using System.Threading.Tasks;
using System.Collections.Generic;
using NHibernate;

namespace MasDev.Common.Data.NHibernate
{
	internal class NHibernateEntitySet<T> : IEntitySet<T> where T : IModel
	{
	}

}

