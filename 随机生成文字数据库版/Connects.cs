using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 随机生成文字数据库版
{
    public class Connects
    {

        //测试数据库是否正常连接
        public bool ConnectTest(string path)
        {
            SQLiteConnection conn = new SQLiteConnection(path);
            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        //连接数据库并查询数据
        public DataTable GetDataTable(string strSQL, string path)
        {
            DataTable dt = null;
            SQLiteConnection conn = new SQLiteConnection(path);
            SQLiteCommand cmd = new SQLiteCommand(strSQL, conn);
            try
            {
                SQLiteDataAdapter dao = new SQLiteDataAdapter(cmd);
                dt = new DataTable();
                dao.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SetDataTable(string strSQL,string path)
        {
            SQLiteConnection conn = new SQLiteConnection(path);
            SQLiteCommand cmd = new SQLiteCommand(strSQL, conn);
            try
            {
                conn.Open();
            return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

    }
}
