using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Net;
using System.IO; //For the stream objects
using System.Web.Script.Serialization;


namespace PropertyBrowserGeocoder
{
    public class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static GoogleGeocodeResponse GeoCode(string inputAddress, string apiKey)
        {
            try
            {
                string sURL = "https://maps.googleapis.com/maps/api/geocode/json?address=" + inputAddress + "&key=" + apiKey;
                WebClient webClient = new WebClient();

                Stream stream = webClient.OpenRead(sURL);
                StreamReader reader = new StreamReader(stream);
                String responseJson = reader.ReadToEnd();
                
                JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                GoogleGeocodeResponse geocoderesult = jsonSerializer.Deserialize<GoogleGeocodeResponse>(responseJson);
                return geocoderesult;
            }
            catch (Exception)
            {
                logger.Info("Exception. Geocoding address:" + inputAddress);
               
                throw;
            }
        }

        public static void Main()
        {
            //--FOR EACH RECORD
            //----    make a web service call to google to get lat long
            //----    Parse the result
            //--    Update property table and set the Lat and Long fields in the databae to the result given by google
            //--END LOOP

            //--> Connect to Oracle <--
            MyDBConnector DBConnector = new MyDBConnector("allproperty", "zacchi_star", "localdb");
            string sql = "select * from allproperties where nvl(isgeocoded, 0)  != 1";
            DBConnector.OpenConnection();
            OracleCommand cmd = new OracleCommand(sql, DBConnector.Connection);
            OracleDataReader dr = cmd.ExecuteReader();

            //--- Connect to Oracle simpler way ---
            //OracleConnection con;
            //con = new OracleConnection();
            //con.ConnectionString = "User Id=allproperty;Password=zacchi;Data Source=localdb";
            //con.Open();
            //string sql = "select * from allproperties where isgeocoded != 1 or isgeocoded is null";
            //OracleCommand cmd = new OracleCommand(sql, con);
            //OracleDataReader dr = cmd.ExecuteReader();

            // Get Geocode information from Google
            var apiKey = "AIzaSyD7FUliTBfIW4_TW4IFq9ZiJP23putrTeI";
            int countKey = 0;// key change count
            
            while (dr.Read() == true)
            {
                long propertyId = (long)dr["PropertyId"];
                string dbAddress = (string)dr["Address"];
                string dbPostalCode = (string)dr["PostalCode"];
                string dbCounty = (string)dr["County"];
                string country = "Republic of Ireland";
                string dbFullAddress = dbAddress + ", " + dbPostalCode + ", " + dbCounty + ", " + country;

                GoogleGeocodeResponse g = GeoCode(dbFullAddress, apiKey);
                logger.Info(g.Status);

                if (g.Status.Equals("OVER_QUERY_LIMIT"))
                {
                    apiKey = "AIzaSyCahm8jN84LMOuAYbUEA6_Mquank8AQsR8";
                    //AIzaSyD7FUliTBfIW4_TW4IFq9ZiJP23putrTeI
                    logger.Info("API Key has been changed to:" + apiKey);
                    countKey++;
                    logger.Info("count: "+ countKey);

                    if (countKey == 2)
                    {
                        logger.Error("Geocode limit has been reached. Program will shutdown.");
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                // stop the loop after g.Status from google geocode has changed the key once.
                
                if (g.Results.Count == 0)
                {
                    Console.WriteLine("Unable to geocode property: " + dbFullAddress);
                    Console.WriteLine(g.Status);
                    //string updateSQLFailure = "update allproperties set geocodenotfound = 1 where propertyid =" + propertyId;
                    //OracleCommand updCmd1 = new OracleCommand(updateSQLFailure, con);
                    //updCmd1.ExecuteScalar();
                    continue;
                }
                string updatesql = "update allproperties set lat = " + g.Results[0].Geometry.Location.Lat + ", lng ="
                    + g.Results[0].Geometry.Location.Lng + ", isgeocoded = 1 where propertyid = " + propertyId;

                OracleCommand updCmd = new OracleCommand(updatesql, DBConnector.Connection);
                updCmd.ExecuteScalar();
            }
            DBConnector.CloseConnection();
        }

    }
    public class Math
    {
        public void main()
        {
            int resultadoDaSoma = Sum(15, 40);
        }
        public static int Sum(int num1, int num2)
        {

            return num1 + num2;
        }
    }
}