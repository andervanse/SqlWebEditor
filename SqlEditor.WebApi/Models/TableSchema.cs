
using System.Collections.Generic;

namespace SqlEditor.WebApi.Models
{
    public class TableSchema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public IEnumerable<ColumnSchema> Columns { get; set; }
        public IEnumerable<ForeignKeySchema> ForeignKeys { get; set; }
    }

	public class ForeignKeySchema
	{
		public string Name { get; set; }
		public ColumnSchema Column { get; set; }
		public TableSchema ReferencedTable { get; set; }
	}
    
    public class ColumnSchema
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public bool IsNullable { get; set; }
    }
}