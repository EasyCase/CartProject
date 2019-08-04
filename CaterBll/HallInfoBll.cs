using CaderModel;
using CaterDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaterBll
{
    public class HallInfoBll
    {
        private HallinfoDal hlDal;

        public HallInfoBll()
        {
            hlDal = new HallinfoDal();
        }
        public List<HallInfo> GetList()
        {
            return hlDal.GetList();
        }
        public bool Add(HallInfo hi)
        {
            return hlDal.Insert(hi) > 0;
        }
        public bool Edit(HallInfo hi)
        {
            return hlDal.Update(hi) > 0;
        }
        public bool Remove(int id)
        {
           return hlDal.Delete(id) > 0;
        }
    }
}
