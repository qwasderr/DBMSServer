using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DatabaseServer
{
    /* // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
     [ServiceContract]
     public interface IDatabaseService
     {

         [OperationContract]
         string GetData(int value);

         [OperationContract]
         CompositeType GetDataUsingDataContract(CompositeType composite);

         // TODO: Add your service operations here
     }


     // Use a data contract as illustrated in the sample below to add composite types to service operations.
     [DataContract]
     public class CompositeType
     {
         bool boolValue = true;
         string stringValue = "Hello ";

         [DataMember]
         public bool BoolValue
         {
             get { return boolValue; }
             set { boolValue = value; }
         }

         [DataMember]
         public string StringValue
         {
             get { return stringValue; }
             set { stringValue = value; }
         }
     }*/





    [ServiceContract]
    public interface IDatabaseService
    {
        
        [OperationContract]
        List<Schema> GetSchemas();

      
        [OperationContract]
        Database AddSchema(Database db, Schema schema);

        [OperationContract]
        string AddSchema2(string db, Schema schema);

        
        [OperationContract]
        List<Row> GetRows(string TableName);

       
        [OperationContract]
        Database AddRow(Database db, string TableName, Row row);

        [OperationContract]
        string AddRow2(string db2, string tableName, Row row);

        
        [OperationContract]
        void UpdateRow(Database db, string TableName, int rowId, Row updatedRow);

       
        [OperationContract]
        Database DeleteRow(Database db, string TableName, int rowId);

        [OperationContract]
        string DeleteRow2(string db2, string TableName, int rowId);
        
        [OperationContract]
        bool ValidateValue(object value, string fieldType);

        [OperationContract]
        Database CreateDatabase(string databaseName);

        
        [OperationContract]
        bool DatabaseExists(string databaseName);

       
        [OperationContract]
        Database LoadDatabase(string filePath);

        [OperationContract]
        string LoadDatabase2(string filePath);

        [OperationContract]
        Table GetTableDifference(Database db, string tableName1, string tableName2);
        [OperationContract]
        string GetTableDifference2(string db, string tableName1, string tableName2);

        [OperationContract]
        Database AddTable(Database db, string tableName, Table table);
        [OperationContract]
        string AddTable2(string db, string tableName, string table);

        [OperationContract]
        Database DeleteTable(Database db, string tableName);
        [OperationContract]
        string DeleteTable2(string db, string tableName);

        [OperationContract]
        void SaveDatabase(Database db, string fileName);

        [OperationContract]
        void SaveDatabase2(string db, string fileName);
        [OperationContract]
        Database CreateTable(Database db, string name, Schema schema);

        [OperationContract]
        string CreateTable2(string db, string name, Schema schema);
        [OperationContract]
        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "players")]
        Table GetTable(Database db, string name);
        [OperationContract]
        string GetTable2(string db, string name);

        [OperationContract]
        Schema GetSchemaByName(Database db, string name);

        [OperationContract]
        string GetSchemaByName2(string db, string name);
        [OperationContract]
        int ToInt(Value value);
        [OperationContract]
        int ToInt2(string value);
        [OperationContract]
        int CountV(List<Value> values);
        [OperationContract]
        int CountV2(string values);

    }










}
