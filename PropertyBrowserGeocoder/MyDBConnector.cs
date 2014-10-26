using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Net;
using System.IO; //For the stream objects

namespace PropertyBrowserGeocoder
{
   public class MyDBConnector
   {
       private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
       public OracleConnection Connection { get; set; }

       private string SchemaName;
       private string Password;
       private string DataSource;

       public MyDBConnector(string aSchemaName, string aPassword, string aDataSource)
       {
           this.SchemaName = aSchemaName;
           this.Password = aPassword;
           this.DataSource = aDataSource;

           string connectionString;
           connectionString = "User Id=" + this.SchemaName + ";" + "Password=" + this.Password +
               ";" + "Data Source=" + this.DataSource + ";";
           Connection = new OracleConnection(connectionString);

       }

       public bool OpenConnection()
       {
           //connection MySQl
           //var ConnectionString = "server=127.0.0.1;uid=root;pwd=dev_$$tt;database=medical";
           try
           {
               Connection.Open();
               return true;
           }
           catch (OracleException ex)
           {
               switch (ex.Number)
               {
                   case 0:
                       logger.Error("Cannot connect to server.  Contact administrator");
                       break;

                   case 1045:
                       logger.Error("Invalid username/password, please try again");
                       break;
               }
               return false;

           }
       }
       public bool CloseConnection()
       {
           try
           {
               Connection.Close();
               return true;
           }
           catch (OracleException ex)
           {
               logger.Error(ex.Message);
               return false;
           }
       }

   }
}
