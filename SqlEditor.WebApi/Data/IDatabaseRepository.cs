

using System.Collections.Generic;
using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Data
{
    public interface IDatabaseRepository
    {
        IEnumerable<TableSchema> GetAll(DatabaseConnection dbConnection);

        TableSchema FindByName(DatabaseConnection dbConnection, string name);
    }    

}