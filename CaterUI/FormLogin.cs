using CaderModel;
using CaterBll;
using System;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        //初始化BLL
        ManagerInfoBll miBll = new ManagerInfoBll();
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();  //退出
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //获取用户输入的信息
            string name = textName.Text;
            string pwd = textPwd.Text;
            //调用代码
            int type;
            ManagerInfoBll miBll = new ManagerInfoBll();
            LoginState loginState = miBll.Login(name, pwd, out type);
            switch (loginState)
            {
                case LoginState.OK:
                    FormMain main = new FormMain();
                    main.Tag = type;//将员工级别传递过去
                    main.Show();
                    //将登录窗体隐藏
                    this.Hide();
                    break;
                case LoginState.NameError:
                    MessageBox.Show("用户名错误");
                    break;
                case LoginState.PwdErrod:
                    MessageBox.Show("密码错误");
                    break;

            }
        }
    }
}
