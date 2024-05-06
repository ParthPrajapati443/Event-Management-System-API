using System.Configuration;

namespace LIBRARY
{
    public class Connection
    {
       
        private static string _CONSTREMS = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        public static string constrEMS = _CONSTREMS;

    }
}
