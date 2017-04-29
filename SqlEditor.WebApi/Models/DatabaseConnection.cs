using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SqlEditor.WebApi.Models
{
    public class DatabaseConnection : IEquatable<DatabaseConnection>, IValidatableObject
    {
        public Guid Id { get; set; }
        public string Database { get; set; }
        public string Name { get; set; }
        public string Server { get; set; }
        public string ConnectionString
        {
            get
            {
                if (this.Server.ToUpper() == "SQLEXPRESS")
                {
                   return $"Data Source=.\\{this.Server};Database={this.Database};" +
                          "Integrated Security=True;MultipleActiveResultSets=True";
                }
                else
                {
                   return $"Data Source={this.Server};Database={this.Database};" +
                          "User Id={this.DatabaseUser.UserName};Password={this.DatabaseUser.Password};" +
                          "Integrated Security=False;MultipleActiveResultSets=True";
                }         

            }
        }
        
        public DatabaseUser DatabaseUser { get; set; }

        public bool Equals(DatabaseConnection other)
        {
            return this.Id == other.Id;
        }

        public bool IsValid()
        {
            var validationList = this.Validate(new ValidationContext(this));

            foreach(var validation in validationList)
            {
                Console.WriteLine(validation.ErrorMessage);
            }  

            return validationList.Count() == 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(this.Name))
                yield return new ValidationResult("Name cannot be null or empty", new[] { "Name" });

            if (String.IsNullOrEmpty(this.Database))
                yield return new ValidationResult("Database cannot be null or empty", new[] { "Database" });

            if (String.IsNullOrEmpty(this.Server))
                yield return new ValidationResult("Server cannot be null or empty", new[] { "Server" });

            if (this.DatabaseUser == null && this.Server?.ToUpper() != "SQLEXPRESS")
                yield return new ValidationResult("DatabaseUser cannot be null or empty", new[] { "DatabaseUser" });
        }
    }

    public class DatabaseUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}