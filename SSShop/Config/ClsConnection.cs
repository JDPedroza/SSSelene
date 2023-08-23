using System.Data.SqlClient;

namespace SeleneShop.Config
{
    class ClsConnection
    {
        public static SqlConnection StartConnection()
        {
            SqlConnection connection = new SqlConnection("server=DESKTOP-J0H4N\\SQLEXPRESS; database=DB_Selene; Trusted_Connection=True");
            connection.Open();
            return connection;
        }
    }
}
