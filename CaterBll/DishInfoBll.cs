using CaderModel;
using CaterDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaterBll
{

    public partial class DishInfoBll
    {
        private DishInfoDal diDal = new DishInfoDal();

        public List<DishInfo> GetList(Dictionary<string, string> dic)
        {
            return diDal.GetList(dic);
        }
        //添加
        public bool Add(DishInfo di)
        {
            return diDal.Insert(di) > 0;
        }

        //修改
        public bool Edit(DishInfo di)
        {
            return diDal.Update(di) > 0;
        }
        //删除
        public bool Remove(int id)
        {
            return diDal.Delete(id) > 0;
        }

    }
}
