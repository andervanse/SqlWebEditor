using System.Data.SqlClient;
using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Application
{
    public class ConnectionApp : IConnectionApp
    {
        public bool TryToConnect(DatabaseConnection conn)
        {
            bool success = false;
            using (var connection = new SqlConnection(conn.ConnectionString))
            {
                connection.Open();
                connection.Close();
                success = true;
            }
            return success;
        }

    }
}