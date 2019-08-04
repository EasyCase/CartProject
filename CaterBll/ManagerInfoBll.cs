using CaderModel;
using CaterCommon;
using CaterDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaterBll
{
    public partial class ManagerInfoBll
    {
        //创建数据层对象
        ManagerInfodal miDal = new ManagerInfodal();
        public List<ManagerInfo> GetList()
        {
            //调用查询的方法
            return miDal.GetList();
        }
        public bool Add(ManagerInfo mi)
        {
            //调用dal层的insert方法，完成插入操作
            return miDal.Insert(mi) > 0;
        }
        public bool Edit(ManagerInfo mi)
        {
            return miDal.Update(mi) > 0;
        }

        public bool Remover(int id)
        {
            return miDal.Delete(id) > 0;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public LoginState Login(string name, string pwd,out int type)
        {
            //设置type默认值，如果为此值时，不会使用
            type = -1;
          
            //根据用户名进行对象的查询
            ManagerInfo mi = miDal.GetByName(name);
            if (mi == null)
            {
                //用户名错误
                type = 0;
                return LoginState.NameError;
            }
            else
            {
                //用户名正确
                if (mi.MPwd.Equals(MD5Helper.EncryptString(pwd))) //MD5加密调用
                {
                    //密码正确
                    //登陆成功
                    type = mi.MType; //out type
                    return LoginState.OK;
                }
                else
                {
                    //密码错误
                   
                    return LoginState.PwdErrod;
                }
            }
        }

    }
}
