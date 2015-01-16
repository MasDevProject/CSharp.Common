using System;
using MasDev.Common.Modeling;
using NHibernate;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Common.Data.NHybernate
{
	public class NHibernateQueryResult<T> : IQueryResult<T> where T : IModel
	{
		protected IQueryOver<T> _queryOver1;
		protected IQueryOver<T, T> _queryOver2;

		public NHibernateQueryResult (IQueryOver<T, T> queryOver)
		{
			_queryOver2 = queryOver;
		}

		protected void ThrowIfNoQuery2 ()
		{
			if (_queryOver2 == null)
				throw new InvalidOperationException ("Should never happen");
		}


		public async Task<T> FirstAsync (System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await Task.Factory.StartNew (() =>
			{
				var items = Enumberable;
				if (items.MoveNext ())
					return items.Current;
				throw new ArgumentException ("No entries satisfy the given predicate");
			});
		}

		public async Task<T> FirstOrDefaultAsync (System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			try
			{
				return await FirstAsync (predicate);
			} catch (ArgumentException)
			{
				return default(T);
			}
		}

		public async Task<T> SingleAsync (System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await Task.Factory.StartNew (() =>
			{
				ThrowIfNoQuery2 ();
				if (_queryOver2.RowCount () != 1)
					throw new ArgumentException ("More than one entry satisfy the predicate");
				return _queryOver2.SingleOrDefault ();
			});
		}

		public async Task<T> SingleOrDefaultAsync (System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await Task.Factory.StartNew (() =>
			{
				ThrowIfNoQuery2 ();
				var count = _queryOver2.RowCount ();
				if (count > 1)
					throw new ArgumentException ("More than one entry satisfy the predicate");
				return count == 0 ? default(T) : _queryOver2.SingleOrDefault ();
			});
		}

		public async Task<bool> AnyAsync (System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await Task.Factory.StartNew (() =>
			{
				ThrowIfNoQuery2 ();
				var count = _queryOver2.Where (predicate).RowCount ();
				return count > 0;
			});
		}

		public async Task<bool> AllAsync (System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await Task.Factory.StartNew (() =>
			{
				ThrowIfNoQuery2 ();
				var originalCount = _queryOver2.RowCount ();
				var count = _queryOver2.Where (predicate).RowCount ();
				return originalCount == count;
			});
		}

		public async Task<uint> CountAsync (System.Linq.Expressions.Expression<Func<T, bool>> predicate)
		{
			return await Task.Factory.StartNew (() =>
			{
				ThrowIfNoQuery2 ();
				var count = _queryOver2.Where (predicate).RowCount ();
				return (uint)count;
			});
		}

		public async Task<IEnumerable<T>> DistinctAsync (IEqualityComparer<T> comparer)
		{
			var l = await ToListAsync ();
			return await Task.Factory.StartNew (() => l.Distinct (comparer));
		}

		public async Task<IEnumerable<T>> DistinctAsync ()
		{
			var l = await ToListAsync ();
			return await Task.Factory.StartNew (() => l.Distinct ());
		}

		public async Task<List<T>> ToListAsync ()
		{
			return await Task.Factory.StartNew (() =>
			{
				if (_queryOver2 != null)
					return (List<T>)_queryOver2.List ();
				if (_queryOver1 == null)
					throw new InvalidOperationException ("Should never happen");
				return (List<T>)_queryOver1.List ();
			});
		}

		protected IEnumerator<T> Enumberable
		{
			get { 
				if (_queryOver2 != null)
					return _queryOver2.Future ().GetEnumerator ();
				if (_queryOver1 == null)
					throw new InvalidOperationException ("Should never happen");
				return _queryOver1.Future ().GetEnumerator ();
			}
		}
	}
}

