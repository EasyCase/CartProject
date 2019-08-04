using CaderModel;
using Cater.Model;
using CaterDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaterBll
{
    public partial class DishTypeInfoBll
    {
        DishTypeInfoDal dtidal = new DishTypeInfoDal();
        public List<DishTypeInfo> GetList()
        {
            return dtidal.GetList();
        }
        public bool Add(DishTypeInfo dti)
        {
            return dtidal.Insert(dti) > 0;
        }
        public bool Edit(DishTypeInfo dti)
        {
            return dtidal.Update(dti) > 0;
        }
        public bool Delete(int dti)
        {
           return dtidal.Delete(dti) > 0;
        }
    }
}
