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
    public partial class DishInfoDal
    {
        public List<DishInfo> GetList(Dictionary<string, string> dic)
        {
            string sql = "select di.*,dti.dtitle as dTypeTitle from dishinfo as di inner join dishtypeinfo as dti on di.DTypeId = dti.did where di.Disdelete = 0 and dti.Disdelete = 0";
            //拼接筛选条件
            if (dic.Count > 0)
            {
                //  sql += " adn 属于 like '%值%";
                foreach (var pair in dic)
                {
                    sql += " and di." + pair.Key + " like '%" + pair.Value + "%'";
                }
            }

            DataTable dt = SqliteHelper.GetDataTable(sql);
            List<DishInfo> list = new List<DishInfo>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DishInfo()
                {
                    DId = Convert.ToInt32(row["did"]),
                    DTitle = row["dtitle"].ToString(),
                    DTypeTitle = row["dtypeTitle"].ToString(),
                    DChar = row["dchar"].ToString(),
                    DPrice = Convert.ToDecimal(row["dprice"])
                });
            }

            return list;
        }

        public int Insert(DishInfo di)
        {
            string sql = "insert into dishinfo(dtitle,dtypeid,dprice,dchar,dIsDelete) values(@title,@tid,@price,@dchar,0)";
            SQLiteParameter[] p =
            {
                new SQLiteParameter("@title",di.DTitle),
                new SQLiteParameter("@tid",di.DTypeId),
                new SQLiteParameter("@price",di.DPrice),
                new SQLiteParameter("@dchar",di.DChar)
            };
            return SqliteHelper.ExecuteNonQuery(sql, p);
        }

        public int Update(DishInfo mi)
        {
            string sql = "update dishinfo set dtitle=@title,dtypeid=@tid,dprice=@price,dchar=@dchar where did=@id";

            SQLiteParameter[] ps =
            {
                new SQLiteParameter("@title",mi.DTitle),
                new SQLiteParameter("@tid",mi.DTypeId),
                new SQLiteParameter("@price",mi.DPrice),
                new SQLiteParameter("@dchar",mi.DChar),
                new SQLiteParameter("@id",mi.DId)
              
            };
            return SqliteHelper.ExecuteNonQuery(sql, ps);
        }
        public int Delete(int id)
        {
            string sql = "update dishinfo set DisDelete=1 where did=@id";
            SQLiteParameter p = new SQLiteParameter("id", id);
            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
    }
}
