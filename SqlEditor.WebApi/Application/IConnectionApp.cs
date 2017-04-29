using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Application
{
    public interface IConnectionApp
    {
         bool TryToConnect(DatabaseConnection conn);
    }
}