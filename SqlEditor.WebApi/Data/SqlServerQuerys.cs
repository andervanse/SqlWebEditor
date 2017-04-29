namespace SqlEditor.WebApi.Data
{
	public static class SqlServerQuerys
    {

		public static string QueryTableColumnsAndTableReferences()
		{
			string sql =
               "SELECT " +
			   "    FK.NAME 'FK_NAME', " +           //  0
			   "    TP.NAME 'PARENT_TABLE', " +      //  1
			   "    TP.OBJECT_ID 'PARENT_ID'," +     //  2
			   "    CP.NAME, " +                     //  3
			   "    CP.COLUMN_ID, " +                //  4
			   "    CP.IS_NULLABLE, " +              //  5
			   "    TR.NAME 'REFRENCED_TABLE', " +   //  6
			   "    TR.OBJECT_ID 'REFERENCED_ID'," + //  7
			   "    CR.NAME, " +                     //  8
			   "    CR.COLUMN_ID, " +                //  9
			   "    CR.IS_NULLABLE " +               // 10   
			   " FROM SYS.FOREIGN_KEYS FK " +
			   " INNER JOIN SYS.TABLES TP ON FK.PARENT_OBJECT_ID = TP.OBJECT_ID " +
			   " INNER JOIN SYS.TABLES TR ON FK.REFERENCED_OBJECT_ID = TR.OBJECT_ID " +
			   " INNER JOIN SYS.FOREIGN_KEY_COLUMNS FKC ON FKC.CONSTRAINT_OBJECT_ID = FK.OBJECT_ID " +
			   " INNER JOIN SYS.COLUMNS CP ON FKC.PARENT_COLUMN_ID = CP.COLUMN_ID AND FKC.PARENT_OBJECT_ID = CP.OBJECT_ID " +
			   " INNER JOIN SYS.COLUMNS CR ON FKC.REFERENCED_COLUMN_ID = CR.COLUMN_ID AND FKC.REFERENCED_OBJECT_ID = CR.OBJECT_ID " +
			   " WHERE TP.NAME = @TABLE_NAME " +
			   " UNION " +
			   " SELECT " +
			   "   NULL 'FK_NAME', " +
			   "   TP.NAME 'PARENT_TABLE', " +
			   "   TP.OBJECT_ID 'PARENT_ID', " +
			   "   CP.NAME 'PARENT_COLUMN_NAME', " +
			   "   CP.COLUMN_ID 'PARENT_COLUMN_ID', " +
			   "   CP.IS_NULLABLE 'PARENT_COLUMN_NULLABLE', " +
			   "   NULL 'REFRENCED_TABLE', " +
			   "   NULL 'REFERENCED_ID', " +
			   "   NULL 'REFERENCED_COLUMN_NAME', " +
			   "   NULL 'REFERENCED_COLUMN_ID', " +
			   "   NULL 'REFERENCED_COLUMN_NULLABLE'" +
			   " FROM SYS.TABLES TP " +
			   " INNER JOIN SYS.COLUMNS CP ON TP.OBJECT_ID = CP.OBJECT_ID" +
			   " WHERE TP.NAME = @TABLE_NAME ";

			return sql;
		}
    }
}
