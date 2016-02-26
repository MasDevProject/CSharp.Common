using System.Threading.Tasks;
using SQLite;
using System;


namespace MasDev.Common.Data.SQLite
{
	public static class SQLiteAsyncConnectionExtensions
	{
		/// <summary>
		/// This method is required when you need to run async methods inside the transaction block (in the example below it is needed to call the client.getCarsAsync() inside the transaction block)
		/// If you dont need to run async methods inside the trasaction block, you can use the library's method: asyncConnection.RunInTransactionAsync (Action<SQLiteConnection> conn))
		/// 
		/// Example of usage:
		/// 
		/// await myAsyncConnection.RunInTransactionAsync (new SQliteConnection("dbPath"), async conn => {
		/// 	var cars = await client.GetCarsAsync ();
		/// 	conn.InserAll (cars);
		/// });
		/// 
		/// 
		/// Utility method:
		/// 
		/// public static Task RunInTrasaction (Func<SQLiteConnection, Task> transaction)
		/// {
		///		return ConnectionManager.Current.Connection.RunInTransactionAsync (
		///			ConnectionManager.Current.GetNewSQLiteConnection,
		///			transaction);
		//  }
		/// 
		/// </summary>
		/// <returns>The in transaction async.</returns>
		/// <param name="connection">Connection.</param>
		/// <param name="connectionFactory">Connection factory.</param>
		/// <param name="action">Action.</param>
		public static Task RunInTransactionAsync (this SQLiteAsyncConnection connection, Func<SQLiteConnection> connectionFactory, Func<SQLiteConnection, Task> action)
		{
			return Task.Run (async () =>
				{ 
					using (var conn = connectionFactory.Invoke ()) 
					{
						try
						{
							conn.BeginTransaction ();

							await action.Invoke (conn);

							conn.Commit ();

						} 
						catch
						{
							if (conn.IsInTransaction)
								conn.Rollback ();

							throw;
						}
						finally 
						{
							conn.Close ();
						}
					}
				}
			);
		}
	}
}

