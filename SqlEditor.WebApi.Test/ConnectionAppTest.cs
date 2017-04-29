using System;
using SqlEditor.WebApi.Application;
using SqlEditor.WebApi.Models;
using Xunit;

namespace SqlEditor.WebApi.Test
{
	public class ConnectionAppTest
    {

        [Fact(DisplayName="Connection_Should_be_Ok")]
        public void Connection_Should_be_Ok()
        {
            var connection = new DatabaseConnection() 
            {                 
                Database = "TESTE_01", 
                Name = "TESTE_01", 
                Server = "SQLEXPRESS"
           };

           ConnectionApp app = new ConnectionApp();
           Console.WriteLine($"Trying connection with '{connection.Database}'...");
           var result = app.TryToConnect(connection);

           Assert.True(result);
        }

		[Fact(DisplayName = "DatabaseConnection_Should_be_Valid")]
		public void DatabaseConnection_Should_Be_Valid()
		{
			var connection = new DatabaseConnection()
			{
				Database = "TESTE_01",
				Name = "TESTE_01",
				Server = "SQLEXPRESS"
			};

			var isOK = connection.IsValid();
			Console.WriteLine($"connection '{connection.Database}' {(isOK?"is Valid": "is not Valid")}");
			Assert.True(isOK);
		}
	}
}