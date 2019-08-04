using CaderModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace CaterDal
{
    public class HallinfoDal
    {
        //显示
        public List<HallInfo> GetList()
        {
            string sql = "select * from hallInfo where hIsDelete=0";
            DataTable dt = SqliteHelper.GetDataTable(sql);

            List<HallInfo> list = new List<HallInfo>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new HallInfo()
                {
                    HId = Convert.ToInt32(row["hid"]),
                    HTitle = row["htitle"].ToString()
                });
            }

            return list;
        }
        //添加
        public int Insert(HallInfo hi)
        {
            string sql = "insert into HallInfo(htitle,hisDelete) values(@title,0)";
            SQLiteParameter p = new SQLiteParameter("@title", hi.HTitle);

            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
        //修改
        public int Update(HallInfo hi)
        {
            string sql = "update HallInfo set htitle=@title where hid=@id";
            SQLiteParameter[] p =
            {
                 new SQLiteParameter("@id", hi.HId),
                 new SQLiteParameter("title",hi.HTitle)
             };
            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
        //删除
        public int Delete(int id)
        {
            string sql = "update HallInfo set HisDelete=1 where hid=@id";
            SQLiteParameter p = new SQLiteParameter("@id", id);
            return SqliteHelper.ExecuteNonQuery(sql, p);
        }
    }
}
