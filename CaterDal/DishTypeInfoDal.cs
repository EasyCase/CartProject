using CaderModel;
using Cater.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaterDal
{
    public partial class DishTypeInfoDal
    {
        public List<DishTypeInfo> GetList()
        {
            //构造sql语句
            string sql = "select * from DishTypeInfo where DisDelete=0";
            DataTable dt = SqliteHelper.GetDataTable(sql);
            //转存集合
            List<DishTypeInfo> list = new List<DishTypeInfo>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DishTypeInfo()
                {
                    DId = Convert.ToInt32(row["did"]),
                    DTitle = row["dtitle"].ToString(),
                });
            }
            //返回
            return list;
        }

        public int Insert(DishTypeInfo dti)
        {
            string sql = "insert into dishtypeinfo(dtitle,dIsDelete) values(@title,0)";
            SQLiteParameter p = new SQLiteParameter("@title", dti.DTitle);
            //返回
            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
        public int Update(DishTypeInfo dti)
        {
            //构造sql语句
            string sql = "update dishtypeinfo set dtitle=@title where did=@id";
            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@title",dti.DTitle),
                new SQLiteParameter("@id",dti.DId),
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }

        public int Delete(int id)
        {
            string sql = "delete from dishtypeinfo where did=@id";

            SQLiteParameter ps = new SQLiteParameter("@id", id);

           return SqliteHelper.ExecuteNonQuery(sql,ps);

        }
    }
}
