
using System;
using Xunit;
using SqlEditor.WebApi.Data;
using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Test
{
    public class ConnectionsJsonRepositoryTest
    {
        private DatabaseConnection _testConnection;

        public ConnectionsJsonRepositoryTest()
        {
            _testConnection = new DatabaseConnection()
            {
                Database = "Oracle111",
                Name = "Pepe_teste",
                Server = "1.1.1.1",
                DatabaseUser = new DatabaseUser()
                {
                    UserName = "admin",
                    Password = "admin"
                }
            };

        }

        [Fact(DisplayName = "JsonRepository_Should_Add_Connection")]
        public void Repository_Should_Add_Connection()
        {
            ConnectionJsonRepository repo = new ConnectionJsonRepository();
            Console.WriteLine($"Adding Database .: {_testConnection.Database}");
            var addedConn = repo.Save(_testConnection);
            Assert.Equal(_testConnection.Database, addedConn.Database);
        }

        [Fact(DisplayName = "JsonRepository_Should_Update_Connection")]
        public void Repository_Should_Update_Connection()
        {
            ConnectionJsonRepository repo = new ConnectionJsonRepository();
            var connection = _testConnection;
            Console.WriteLine($"Updating Database .: {connection?.Database}");
            Console.WriteLine($"Current Database .: {connection?.Database}, Server.: {connection?.Server}");
            connection.Database = "MySql";
            connection.Server = "Localhost";
            var addedConn = repo.Save(connection);
            var connectionSaved = repo.FindByName("Pepe_teste");
            Console.WriteLine($"Database Updated .: {connectionSaved.Database}, Server.: {connectionSaved.Server}");
            Assert.Equal(connection.Database, addedConn.Database);
        }

        [Fact(DisplayName = "JsonRepository_Should_Remove_Connection")]
        public void Repository_Should_Remove_Connection()
        {
            ConnectionJsonRepository repo = new ConnectionJsonRepository();
			_testConnection.Name = "PepeJaTireiAVela";
			repo.Save(_testConnection);
			var connection = repo.FindByName("PepeJaTireiAVela");
            Console.WriteLine($"Removing Database.: {connection.Database}");
            repo.Remove(connection.Id);
            var connectionFound = repo.FindByName("PepeJaTireiAVela");
            Assert.Equal(null, connectionFound);
        }
    }
}
