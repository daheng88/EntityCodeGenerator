using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Data.SqlClient;

namespace EntityCodeGenerator
{
    /// <summary>
    /// 用于操作数据库的辅助类
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 尝试连接数据库
        /// </summary>
        /// <param name="host">数据库IP地址</param>
        /// <returns>是否连接成功</returns>
        public static bool TryConnect(string host,int port=1433)
        {
            if (TestConnection(host, port, 500))
            {
                return TryConnectDatabase();
            }
            return false;
        }

        /// <summary>
        /// 通过Socket去尝试连接远程机器,确认是否可以连接
        /// </summary>
        /// <param name="host">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="millisecondsTimeout">超时时间</param>
        /// <returns>是否连接成功</returns>
        private static bool TestConnection(string host, int port, int millisecondsTimeout)
        {
            var client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(host, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch
            {
                return false;
            }
            finally
            {
                client.Close();
            }
        }

        /// <summary>
        /// 尝试去连接数据库，确认是否连接成功
        /// </summary>
        /// <returns>是否连接成功</returns>
        private static bool TryConnectDatabase()
        {
            bool result = false;
            try
            {
                if (string.IsNullOrEmpty(ConnectionString.Trim()))
                    throw new Exception("数据库连接字符串不能为空.");
                
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    result = true;
                    conn.Close();
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 通过sql语句查询数据
        /// </summary>
        /// <param name="sqlText">要执行的SQL语句</param>
        /// <returns>结果集</returns>
        public static DataTable QueryData(string sqlText)
        {
            string connectionString = @ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlText,connection))
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
    }


}
