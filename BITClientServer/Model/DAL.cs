using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace BITClientServer.Model
{
    class DAL
    {
        MySqlConnection conn;
        private void ConnectDb()
        {
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            conn = new MySqlConnection(connStr);
        }

        public int Query(string queryStr)
        {
            ConnectDb();
            int rows = 0;
            try
            {
                MySqlCommand updateCmd = new MySqlCommand(queryStr, conn);
                updateCmd.Connection.Open();
                rows = updateCmd.ExecuteNonQuery();
                updateCmd.Connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
        

        public DataTable ReadRecords(string queryStr)
        {
            ConnectDb();
            DataTable dt = new DataTable();
            try
            {
                MySqlCommand cmd = new MySqlCommand(queryStr, conn);
                MySqlDataAdapter adapt = new MySqlDataAdapter(cmd);
                adapt.Fill(dt);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        
    }
}
