using CaderModel;
using CaterDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaterBll
{
    public partial class OrderInfoBll
    {



        //开单操作
        private OrdeInfoDal oiDal = new OrdeInfoDal();

        public int KaiDan(int tableId)
        {
            return oiDal.KaiOrder(tableId);
        }

        public int GetOrderIdByTableId(int tableId)
        {
            return oiDal.GetOrderIdByTableId(tableId);
        }
        //点菜操作
        public bool DianCai(int orderId,int dishId)
        {
            return oiDal.DianCai(orderId, dishId) > 0;
        }
        //数量更改
        public bool  UpdateCountByOid(int oid,int count)
        {
            return oiDal.UpdateCountByOid(oid, count) > 0;
        }
        //查询总金额
        public decimal GetTotaiMoneByOrderId(int orderid)
        {
            return oiDal.GetTotalMoneyByOrderId(orderid);
        }
        public List<OrderDetailInfo> GetDetaList(int orderId)
        {
            return oiDal.GetDetaiList(orderId);
        }

        //设置下单按钮
        public bool SetOrderMoney(int orderid,decimal money)
        {
            return oiDal.SetOrderMomery(orderid, money) > 0;
        }
        //删除
        public bool DeleteDetailByid(int oid)
        {
            return oiDal.DeleteDetailById(oid) > 0;
        }

        //结账方法
        public bool Pay(bool isUseMoney, int memberId, decimal payMoney, int orderid, decimal discount)
        {
            return oiDal.Pay(isUseMoney, memberId, payMoney, orderid, discount) > 0;
        }
    }
}
