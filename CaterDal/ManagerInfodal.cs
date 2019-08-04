using CaderModel;
using CaterCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaterDal
{
    public partial class ManagerInfodal
    {

        /// <summary>
        /// 查询获取结果集
        /// </summary>
        /// <returns></returns>
        public List<ManagerInfo> GetList()
        {
            //构造要查询的sql语句
            string sql = "select * from ManagerInfo";
            //调用Heiper方法，进行查询，得到结果
            DataTable dt = SqliteHelper.GetDataTable(sql);
            //将dt中的数据转到list中
            List<ManagerInfo> list = new List<ManagerInfo>();
            foreach (DataRow item in dt.Rows)
            {
                list.Add(new ManagerInfo()
                {
                    MId = Convert.ToInt32(item["mid"]),
                    MName = item["mname"].ToString(),
                    MPwd = item["mpwd"].ToString(),
                    MType = Convert.ToInt32(item["mtype"])
                });
            }
            //将list集合返回
            return list;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="mi">ManagerInfo类型的对象</param>
        /// <returns></returns>
        public int Insert(ManagerInfo mi)
        {
            //构造Insert语句
            string sql = "insert into ManagerInfo(mname,mpwd,mtype) values(@name,@pwd,@type)";
            //构造sql语句参数
            SQLiteParameter[] ps =  //使用数组初始化器
            {
                new SQLiteParameter("@name",mi.MName),
                new SQLiteParameter("@pwd",MD5Helper.EncryptString(mi.MPwd)), //MD5加密
                new SQLiteParameter("@type",mi.MType)
            };
            //执行插入操作
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public int Update(ManagerInfo mi)
        {
            //定义参数集合，可以动态添加元素
            List<SQLiteParameter> listPS = new List<SQLiteParameter>();
            //构造update的sql语句
            string sql = "update ManagerInfo set mname=@name";
            listPS.Add(new SQLiteParameter("@name", mi.MName));
            //判断是否修改密码
            if (!mi.MPwd.Equals("这是原来的密码吗"))
            {
                sql += ",mpwd=@pwd";
                listPS.Add(new SQLiteParameter("@pwd", MD5Helper.EncryptString(mi.MPwd)));
                MessageBox.Show("密码修改成功");


            }
            sql += ",mpwd=@pwd,mtype=@type where mid=@id";
            //继续拼接语句
            listPS.Add(new SQLiteParameter("@type", mi.MType));
            listPS.Add(new SQLiteParameter("@id", mi.MId));

            //执行语句并返回结果
            return SqliteHelper.ExecuteNonQuery(sql, listPS.ToArray());
        }

        /// <summary>
        /// 根据编号删除管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            //构造删除的SQL语句
            string sql = "delete from ManagerInfo where mid=@id";
            //根据语句构造参数
            SQLiteParameter p = new SQLiteParameter("@id", id);
            //执行操作
            return SqliteHelper.ExecuteNonQuery(sql, p);

        }

        /// <summary>
        /// 根据用户名查找对象-登陆
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        public ManagerInfo GetByName(string name)
        {
            //定义一个对象
            ManagerInfo mi = null;
            //构造语句
            string sql = "select * from managerInfo where mname=@name";
            //为语句构造参数
            SQLiteParameter p = new SQLiteParameter("@name", name);
            //执行查询得到结果
            DataTable dt = SqliteHelper.GetDataTable(sql, p);
            //判断是否根据用户名查找到了对象
            if (dt.Rows.Count > 0)
            {
                //用户名是否存在
                mi = new ManagerInfo()
                {
                    MId = Convert.ToInt32(dt.Rows[0][0]),
                    MName = name,
                    MPwd = dt.Rows[0][2].ToString(),
                    MType = Convert.ToInt32(dt.Rows[0][3])

                };
            }
            else
            {
                //用户名不存在
            }
            return mi;
        }
    }
}
