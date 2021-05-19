using System;
using MySql.Data.MySqlClient;

namespace PaymentGateway.IntegrationTests
{
    
    public abstract class MySqlTestBase : IDisposable
    {
        private readonly string _connectionString;
        protected readonly string _databaseName;

        public MySqlTestBase(string connectionString, string databaseName)
        {
            _databaseName = databaseName;
            _connectionString = connectionString;
            
            SetUpDatabase();
        }
        
        protected abstract string DatabaseSeed { get; }

        private void SetUpDatabase()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            
            var testSetup = $@"
                CREATE DATABASE IF NOT EXISTS `{_databaseName}`;
                USE `{_databaseName}`;
                {DatabaseSeed};";

            var command = new MySqlCommand(testSetup, connection);
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var cleanDB = $"DROP DATABASE `{_databaseName}`;";
            
            var command = new MySqlCommand(cleanDB, connection);
            command.ExecuteNonQuery();
        }
    }
}