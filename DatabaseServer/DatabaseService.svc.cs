
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
namespace DatabaseServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    /*public class Service1 : IDatabaseService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }*/
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DatabaseService : IDatabaseService
    {
        private Database _database;

        public DatabaseService()
        {
            _database = new Database("123"); 
        }

        
        public List<Schema> GetSchemas()
        {
            try
            {
                return _database.GetSchemas();
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error retrieving schemas: {ex.Message}");
            }
        }

        
        public Database AddSchema(Database db, Schema schema)
        {
            try
            {
                db.AddSchema(schema);
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error adding schema: {ex.Message}");
            }
            return db;
        }


        public string AddSchema2(string db2, Schema schema)
        {
            try
            {
                Database db = Convert(db2);
                db.AddSchema(schema);
                return JsonSerializer.Serialize(db);
            }
            catch (Exception ex)
            {
                throw new FaultException($"Error adding schema: {ex.Message}");
            }
            //return db;
            //return JsonSerializer.Serialize(db);
        }
        
        public List<Row> GetRows(string tableName)
        {
            var table = _database.GetTable(tableName);
            if (table == null)
            {
                throw new Exception($"Table '{tableName}' not found.");
            }

            return table.Rows; 
        }

        
        public Database AddRow(Database db, string tableName, Row row)
        {
            var table = db.GetTable(tableName);
            if (table == null)
            {
                throw new Exception($"Table '{tableName}' not found.");
            }

            
            //table.Rows.Add(row);
            table.AddRow(row);
            return db;
        }
        public string AddRow2(string db2, string tableName, Row row)
        {
            Database db = JsonSerializer.Deserialize<Database>(db2);
            //
            var table = db.GetTable(tableName);
            if (table == null)
            {
                throw new Exception($"Table '{tableName}' not found.");
            }
            
            //table.Rows.Add(row);
            table.AddRow(row);
            string jsonContent = JsonSerializer.Serialize(db);
            return jsonContent;
        }

        public void UpdateRow(Database db, string tableName, int rowId, Row updatedRow)
        {
            var table = db.GetTable(tableName);
            if (table == null)
            {
                throw new Exception($"Table '{tableName}' not found.");
            }
            table.EditRow(rowId, updatedRow);
            /*var existingRow = table.Rows.FirstOrDefault(r => r.I == rowId); 
            if (existingRow == null)
            {
                throw new Exception($"Row with ID '{rowId}' not found in table '{tableName}'.");
            }

            
            existingRow.Values = updatedRow.Values;*/
        }

       
        public Database DeleteRow(Database db, string tableName, int rowId)
        {
            var table = db.GetTable(tableName);
            if (table == null)
            {
                throw new Exception($"Table '{tableName}' not found.");
            }
            table.DeleteRow(rowId);
            return db;
            /*var rowToDelete = table.Rows.FirstOrDefault(r => r.Id == rowId); 
            if (rowToDelete == null)
            {
                throw new Exception($"Row with ID '{rowId}' not found in table '{tableName}'.");
            }

           
            table.Rows.Remove(rowToDelete);*/
        }
        public string DeleteRow2(string db2, string tableName, int rowId)
        {
            Database db = Convert(db2);
            var table = db.GetTable(tableName);
            if (table == null)
            {
                throw new Exception($"Table '{tableName}' not found.");
            }
            table.DeleteRow(rowId);
            //return db;
            return JsonSerializer.Serialize(db);
            /*var rowToDelete = table.Rows.FirstOrDefault(r => r.Id == rowId); 
            if (rowToDelete == null)
            {
                throw new Exception($"Row with ID '{rowId}' not found in table '{tableName}'.");
            }

           
            table.Rows.Remove(rowToDelete);*/
        }

        // Валідація значення на основі типу
        public bool ValidateValue(object value, string fieldType)
        {
           
            switch (fieldType.ToLower())
            {
                case "int":
                    return value is int;
                case "real":
                    return value is float || value is double;
                case "char":
                    return value is string str && str.Length == 1;
                case "string":
                    return value is string;
                case "timeint":
                    
                    return ValidateTimeInt(value);
               
                default:
                    throw new Exception($"Unknown field type: {fieldType}");
            }
        }

        private bool ValidateTimeInt(object value)
        {
           
            if (value is string timeString)
            {
                return TimeSpan.TryParse(timeString, out TimeSpan _);
            }
            else if (value is TimeSpan timeSpan)
            {
                return true; 
            }
            return false;
        }


        public Database CreateDatabase(string databaseName)
        {
            /*if (DatabaseExists(databaseName))
            {
                throw new InvalidOperationException("Database with this name already exists.");
            }*/

            Database newDatabase = new Database(databaseName);
            newDatabase.SaveToDisk(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseName + ".db"));
            return newDatabase;
            //_database=newDatabase;
        }

        public bool DatabaseExists(string databaseName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseName + ".db");
            return File.Exists(filePath);
        }

        public Database LoadDatabase(string filePath)
        {
            Database loadedDatabase = new Database("Loaded Database");
            loadedDatabase.LoadFromDisk(filePath);
            //_database= loadedDatabase;
            //string jsonString = loadedDatabase.ToString();

            return loadedDatabase;
        }
        public string LoadDatabase2(string filePath)
        {
            Database loadedDatabase = new Database("Loaded Database");
            loadedDatabase.LoadFromDisk(filePath);
            //_database= loadedDatabase;
            //string jsonString = loadedDatabase.ToString();
            string jsonContent = JsonSerializer.Serialize(loadedDatabase);
            //return new Database { JsonContent = jsonContent };
            //return jsonString;
            return jsonContent;
        }

        public Table GetTableDifference(Database db, string tableName1, string tableName2)
        {
            
            Table table1 = db.GetTable(tableName1);
            Table table2 = db.GetTable(tableName2);

            if (table1 == null || table2 == null)
            {
                throw new FaultException($"One or both tables '{tableName1}' and '{tableName2}' do not exist.");
            }

            
            Table differenceTable = table1.Difference(table2);

            
            differenceTable.Name = $"Difference_{tableName1}_{tableName2}";

            return differenceTable;
        }
        public string GetTableDifference2(string db2, string tableName1, string tableName2)
        {
            Database db = Convert(db2);
           
            Table table1 = db.GetTable(tableName1);
            Table table2 = db.GetTable(tableName2);

            if (table1 == null || table2 == null)
            {
                throw new FaultException($"One or both tables '{tableName1}' and '{tableName2}' do not exist.");
            }

           
            Table differenceTable = table1.Difference(table2);

            
            differenceTable.Name = $"Difference_{tableName1}_{tableName2}";

            //return differenceTable;
            return JsonSerializer.Serialize(differenceTable);
        }

        public Database AddTable(Database db, string tableName, Table table)
        {
           
            db.AddTable(table);
            return db;
        }
        public string AddTable2(string db2, string tableName, string table1)
        {
            Database db = Convert(db2);
            Table table = JsonSerializer.Deserialize<Table>(table1);
           
            db.AddTable(table);
            //return db;
            return JsonSerializer.Serialize(db);
        }

        public Database DeleteTable(Database db, string tableName)
        {
                db.DeleteTable(tableName); 
                return db;
        }

        public string DeleteTable2(string db2, string tableName)
        {
            Database db = Convert(db2);
            db.DeleteTable(tableName); 
            //return db;
            return JsonSerializer.Serialize(db);
        }
        public void SaveDatabase(Database db, string fileName)
        {
          
            db.SaveToDisk(fileName); 
        }
        public void SaveDatabase2(string db2, string fileName)
        {
            Database db = Convert(db2);
            db.SaveToDisk(fileName);
        }
        public Database CreateTable(Database db, string name, Schema schema)
        {
            db.CreateTable(name, schema);
            return db;
        }

        public string CreateTable2(string db2, string name, Schema schema)
        {
            Database db = Convert(db2);
            db.CreateTable(name, schema);
            //return db;
            return JsonSerializer.Serialize(db);

        }
        public Table GetTable(Database db, string name)
        {
            return db.GetTable(name);
        }
        public Database Convert(string db)
        {
            return JsonSerializer.Deserialize<Database>(db);
        }
        public string DConvert(Database db)
        {
            return JsonSerializer.Serialize(db);
        }
        public string GetTable2(string db, string name)
        {
            Database db2 = JsonSerializer.Deserialize<Database>(db);
            string jsonContent = JsonSerializer.Serialize(db2.GetTable(name));
            //return db2.GetTable(name);
            return jsonContent;
        }
        public Schema GetSchemaByName(Database db,string schemaName)
        {

            foreach (var schema in db.Schemas)
            {
                if (schema.Name == schemaName)
                {
                    return schema;
                }
            }
            return null;
        }
        public string GetSchemaByName2(string db2, string schemaName)
        {
            Database db = Convert(db2);
            foreach (var schema in db.Schemas)
            {
                if (schema!=null && schema.Name == schemaName)
                {
                    //return schema;
                    return JsonSerializer.Serialize(schema);
                }
            }
            return null;
        }
        public int ToInt(Value value)
        {
            return value.ToInt();
        }
        public int ToInt2(string value)
        {
            Value v = JsonSerializer.Deserialize<Value>(value);
            return v.ToInt();
        }

        public int CountV(List<Value> values)
        {
            return values.Count;
        }
        public int CountV2(string values)
        {
            List<Value> v = JsonSerializer.Deserialize<List<Value>>(values);
            return v.Count;
        }
    }
}
