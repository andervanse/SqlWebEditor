using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using SqlEditor.WebApi.Data;
using SqlEditor.WebApi.Models;
using Xunit;

namespace SqlEditor.WebApi.Test
{
	public class DatabaseRepositoryTest
    {
		private readonly ILogger<DatabaseRepository> _log;

		public DatabaseRepositoryTest()
		{
			ILoggerFactory factory = new LoggerFactory();
			factory.AddDebug();
			_log = factory.CreateLogger<DatabaseRepository>();
		}

        [Fact(DisplayName = "DatabaseRepository_GetAll_Should_Return_All_Tables")]
        public void DatabaseRepository_GetAll_Should_Return_All_Tables()
        {			
			DatabaseRepository databaseRepo = new DatabaseRepository(_log);
			ConnectionJsonRepository connRepo = new ConnectionJsonRepository();

			var conn = connRepo.FindByName("TESTE_01");
			var list = databaseRepo.GetAll(conn);

			foreach (var table in list)
			{
				Console.WriteLine($"Table Id.: {table.Id} Name.: {table.Name}");
			}

			Assert.True(list.Count() > 0);
        }

		[Fact(DisplayName = "DatabaseRepository_FindByName_Should_Return_Table_With_Relashionships")]
		public void DatabaseRepository_FindByName_Should_Return_Table_With_Relashionships()
		{
			DatabaseRepository databaseRepo = new DatabaseRepository(_log);
			ConnectionJsonRepository connRepo = new ConnectionJsonRepository();

			var conn = connRepo.FindByName("TESTE_01");
			TableSchema table = databaseRepo.FindByName(conn, "NOTA_FISCAL_ITEM");
			Console.WriteLine($"Table Id.: {table?.Id} Name.: {table?.Name}");

			foreach (var tableRef in table?.ForeignKeys)
			{
				_log.LogDebug($"Fk Name.: {tableRef.Name}");
			}

			Assert.True(table != null);
		}


		[Fact(DisplayName = "DatabaseRepository_FindByName_Should_Return_Null_When_TableName_Is_Invalid")]
		public void DatabaseRepository_Should_Return_Null_When_TableName_Is_Invalid()
		{
			//Arrange
			DatabaseRepository databaseRepo = new DatabaseRepository(_log);
			ConnectionJsonRepository connRepo = new ConnectionJsonRepository();

			//Act
			var conn = connRepo.FindByName("TESTE_01");
			TableSchema table = databaseRepo.FindByName(conn, "XXXXX");

			//Assert
			Assert.True(table == null);
		}

		[Fact(DisplayName = "DatabaseRepository_FindByName_Should_Return_Null_When_DatabaseConnection_Is_Invalid")]
		public void DatabaseRepository_FindByName_Should_Return_Null_When_DatabaseConnection_Is_Invalid()
		{
			//Arrange
			DatabaseRepository databaseRepo = new DatabaseRepository(_log);
			ConnectionJsonRepository connRepo = new ConnectionJsonRepository();

			//Act
			var conn = connRepo.FindByName("XXXXX");
			TableSchema table = databaseRepo.FindByName(conn, "TESTE_01");

			//Assert
			Assert.True(table == null);
		}
	}
}
