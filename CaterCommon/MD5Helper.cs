using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CaterCommon
{



    public partial class MD5Helper
    {
        public static string EncryptString(string str)
        {
            //utf8，x2  创建MD5加密
            //创建对象的方法，1 构造方法，2 静态方法（工厂设计模式）
            MD5 md5 = MD5.Create();
            //将字符串str 转换成字节数组
            byte[] byte01d = Encoding.UTF8.GetBytes(str);
            //调用加密方法
            byte[] byteNew = md5.ComputeHash(byte01d);
            //将加密结果进行转换成字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                //将字符转换成16进制表示的字符串
                sb.Append(b.ToString("x2"));
            }
            //返回加密的字符串
            return sb.ToString();
        }
    }
}
