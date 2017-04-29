using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Data
{
	public class DatabaseRepository : IDatabaseRepository
    {
		private readonly ILogger<DatabaseRepository> _logger;

		public DatabaseRepository(ILogger<DatabaseRepository> logger)
		{
			_logger = logger;
			_logger.LogInformation("Repository Created...");
		}

        public IEnumerable<TableSchema> GetAll(DatabaseConnection dbConnection)
        {
			if (dbConnection == null)
				return null;

			IList<TableSchema> list = new List<TableSchema>();

            if (dbConnection.IsValid())
            {
				_logger.LogInformation("dbConnection is valid...");

                using (var connection = new SqlConnection(dbConnection.ConnectionString))
                {
					using (SqlCommand command = new SqlCommand("SELECT OBJECT_ID, NAME FROM SYS.TABLES WHERE TYPE = 'U'", connection))
					{
						connection.Open();

						_logger.LogInformation("connection established...");

						using (SqlDataReader reader = command.ExecuteReader())
						{					

							while (reader.Read())
							{
								list.Add(
									new TableSchema
									{
										Id = reader.GetInt32(0),
										Name = reader.GetString(1)
									});
							}
						}
						connection.Close();
					}
                }
            }

            return list;
        }

        public TableSchema FindByName(DatabaseConnection dbConnection, string name)
        {
			if (dbConnection == null)
				return null;

			string sql = SqlServerQuerys.QueryTableColumnsAndTableReferences();

			TableSchema tableSchema = null;

            if (dbConnection.IsValid())
            {
				_logger.LogInformation("dbConnection is valid...");

				using (var connection = new SqlConnection(dbConnection.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add("TABLE_NAME", SqlDbType.VarChar);
						command.Parameters["TABLE_NAME"].Value = name;
						connection.Open();
						_logger.LogInformation("connection established...");

						using (SqlDataReader reader = command.ExecuteReader())
                        {                            
                            int loopNr = 1;
							string refTableName = "";
							string oldTableName = "";
                            IList<ColumnSchema> columns = new List<ColumnSchema>();
							IList<ForeignKeySchema>  foreignKeys = new List<ForeignKeySchema>();

							while (reader.Read())
                            {
                                if (loopNr == 1)//Get Parent table once
                                {
                                    tableSchema = new TableSchema
                                    {
                                        Id = reader.GetInt32(2),
                                        Name = reader.GetString(1)
                                    };
                                    loopNr++;

									_logger.LogInformation($"reading table '{tableSchema.Name}'");
								}

								//Adding the column that references another table
								var currentColumn = new ColumnSchema
								{
									Id = reader.GetInt32(4),
									TableId = tableSchema.Id,
									Name = reader.GetString(3),
									IsNullable = reader.GetBoolean(5)
								};

								columns.Add(currentColumn);

								if (!reader.IsDBNull(6))
								{
									//Get the name of referenced table
									refTableName = reader.GetString(6);

									if (refTableName != oldTableName)
									{
										var refTable = new TableSchema
										{
											Id = reader.GetInt32(7),
											Name = reader.GetString(6)
										};

										refTable.Columns = new List<ColumnSchema>
										{
											new ColumnSchema
											{
												Id = reader.GetInt32(9),
												TableId = refTable.Id,
												Name = reader.GetString(8),
											    IsNullable = reader.GetBoolean(10)
										    }
										};

										foreignKeys.Add(new ForeignKeySchema
										{
											Name = reader.GetString(0),
											Column = currentColumn,
											ReferencedTable = refTable
										});

										oldTableName = refTableName;
									}
								}
                            }

							if (tableSchema != null)
							{
								tableSchema.Columns = columns;
								tableSchema.ForeignKeys = foreignKeys;
							}
                        }
						connection.Close();
						_logger.LogInformation("Connection closed.");
					}
                }
            }

            return tableSchema;
        }
    }
}