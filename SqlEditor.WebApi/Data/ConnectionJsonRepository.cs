using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SqlEditor.WebApi.Models;

namespace SqlEditor.WebApi.Data
{
    public class ConnectionJsonRepository : IRepository<DatabaseConnection>
    {
        private const string _connectionsJsonFile = "./Connections.json";
        private IList<DatabaseConnection> _list;

        public ConnectionJsonRepository()
        {
            if (!File.Exists(_connectionsJsonFile))
                File.Create(_connectionsJsonFile);

            string text = File.ReadAllText(_connectionsJsonFile);
            _list = JsonConvert.DeserializeObject<List<DatabaseConnection>>(text);
        }

        public DatabaseConnection Save(DatabaseConnection model)
        {            
            if (model != null)
            {
               if (model.Validate(new ValidationContext(model)).Count() > 0)
               {
                   throw new ArgumentException("invalid DatabaseConnection");
               }
            }
            else
            {
                throw new ArgumentNullException("DatabaseConnection cannot be null");
            }

            DatabaseConnection connFound = _list.Where(c => c.Id == model.Id).FirstOrDefault(); 

            if (connFound == null)
            {
                model.Id = Guid.NewGuid();
                _list.Add(model);
            }
            else
            {
                _list[_list.IndexOf(model)] = model;         
            }

            File.WriteAllText(_connectionsJsonFile, JsonConvert.SerializeObject(_list));

            return model;
        }

        public IEnumerable<DatabaseConnection> GetAll()
        {
            return _list;
        }

        public DatabaseConnection FindByName(string name)
        {
            return _list.Where(x => x.Name == name)
                        .FirstOrDefault();
        }

        public void Remove(Guid id)
        {
            var conn = _list.Where(c => c.Id == id).FirstOrDefault();

            if (conn != null)
            {
               _list.Remove(conn);
               File.WriteAllText(_connectionsJsonFile, JsonConvert.SerializeObject(_list));
            }
            else
               throw new ArgumentNullException("DatabaseConnection not found.");
        }

    }
}