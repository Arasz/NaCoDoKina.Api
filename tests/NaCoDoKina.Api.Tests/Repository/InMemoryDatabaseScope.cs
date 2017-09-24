using Microsoft.Data.Sqlite;
using System;

namespace NaCoDoKina.Api.Repository
{
    /// <inheritdoc/>
    /// <summary>
    /// Scope in which in memory database exist 
    /// </summary>
    public class InMemoryDatabaseScope : IDisposable
    {
        public SqliteConnection Connection { get; }

        public InMemoryDatabaseScope()
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}