using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace CaterDal
{
    public static class SqliteHelper
    {
        //从配置文件中读取连接字符串
        private static string connStr = ConfigurationManager.ConnectionStrings["itcaseCater"].ConnectionString;

         
        //执行命令的方法：insert，update，delete
        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] ps)
        {
            //创建数据库对象
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                //创建数据库命令对象
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                //添加参数
                cmd.Parameters.AddRange(ps);
                //打开数据库
                conn.Open();
                //执行命令，并返回受影响的行数
                return cmd.ExecuteNonQuery();

            }
        }
        //获取首行首列值的方法
        public static object ExecuteScalar(string sql,params SQLiteParameter[] ps)
        {
            using(SQLiteConnection conn=new SQLiteConnection(connStr))
            {
                SQLiteCommand cmd = new SQLiteCommand(sql,conn);
                cmd.Parameters.AddRange(ps);
                conn.Open();
                //执行命令，获取查询结果中的首行首列的值，返回
                return cmd.ExecuteScalar();
            }
        } 
        //获取结果集
        public static DataTable GetDataTable(string sql,params SQLiteParameter[] ps)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                //构造适配器对象
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, conn);
                //构造数据表，用户接收查询的结果
                DataTable dt = new DataTable();
                //添加参数
               
                adapter.SelectCommand.Parameters.AddRange(ps);
                //执行结果
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
