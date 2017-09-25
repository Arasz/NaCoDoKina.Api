using System;
using Microsoft.Data.Sqlite;

namespace NaCoDoKina.Api.Repository.Database
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