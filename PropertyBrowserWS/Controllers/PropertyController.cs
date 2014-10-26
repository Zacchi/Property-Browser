using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Oracle.ManagedDataAccess.Client;
using System.Net.Http;
using System.Web.Http;

namespace PropertyBrowserWS.Controllers
{
    public class PropertyController : ApiController
    {
        public List<PropertyDTO> GetAllProperties()
        {

            //--- Connect to Oracle simpler way ---
            OracleConnection con;
            con = new OracleConnection();
            con.ConnectionString = "User Id=allproperty;Password=zacchi_star;Data Source=localdb";
            con.Open();
            string sql = "select * from allproperties where isgeocoded = 1 and ROWNUM < 20";
            OracleCommand cmd = new OracleCommand(sql, con);
            OracleDataReader dr = cmd.ExecuteReader();
            
            var listOfProperties = new List<PropertyDTO>();
            
            // read from database 
            while (dr.Read() == true)
            {
                PropertyDTO aProperty = new PropertyDTO();
                aProperty.Address = Convert.ToString(dr["Address"]);
                aProperty.County = Convert.ToString(dr["County"]);
                aProperty.Lat = Convert.ToDouble(dr["Lat"]);
                aProperty.Lng = Convert.ToDouble(dr["Lng"]);
                aProperty.Price = Convert.ToInt32(dr["Price"]);

                listOfProperties.Add(aProperty);

                //Console.WriteLine(aProperty.Address + "; " + aProperty.Lat + "; " + aProperty.Lng);
            }
            return listOfProperties;
        }
    }

    public class PropertyDTO
    {
        public string Address;
        public double Lat;
        public double Lng;
        public int Price;
        public string County;
    }
}
