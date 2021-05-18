using System;
using MySql.Data.MySqlClient;

namespace PaymentGateway.IntegrationTests
{
    
    public abstract class MySqlTestBase : IDisposable
    {
        protected readonly string _databaseName;
        
        public MySqlTestBase(string connectionString, string databaseName)
        {
            _databaseName = databaseName;
            SetUpDatabase(connectionString);
        }
        
        protected abstract string DatabaseSeed { get; }
        
        public void Dispose()
        {
        }

        private void SetUpDatabase(string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            
            var testSetup = $@"
                CREATE DATABASE IF NOT EXISTS `{_databaseName}`;
                USE `{_databaseName}`;
                {DatabaseSeed};";

            var command = new MySqlCommand(testSetup, connection);
            command.ExecuteNonQuery();
        }
    }
}