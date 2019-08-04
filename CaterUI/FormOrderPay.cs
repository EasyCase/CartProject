using CaderModel;
using CaterBll;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormOrderPay : Form
    {
        public FormOrderPay()
        {
            InitializeComponent();
        }
        private OrderInfoBll oiBll = new OrderInfoBll();
        private int orderId;

        public event Action Refresh;
        private void FormOrderPay_Load(object sender, EventArgs e)
        {
            //获取传递过来的订单编号
            orderId = Convert.ToInt32(this.Tag);

            //运行起来禁用的
            gbMember.Enabled = false;
            GetMoneyByorderId();


        }

        private void GetMoneyByorderId()
        {
            lblPayMoney.Text = lblPayMoneyDiscount.Text = oiBll.GetTotaiMoneByOrderId(orderId).ToString();
        }

        private void cbkMember_CheckedChanged(object sender, EventArgs e)
        {

            //是会员 ，显示会员信息框
            gbMember.Enabled = cbkMember.Checked;

        }
        private void LoadMember()
        {
            //根据会员编号和会员电话进行查询
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (txtId.Text != "")
            {
                dic.Add("mid", txtId.Text); //会员编号
            }
            if (txtPhone.Text != "")
            {
                dic.Add("mPhone", txtPhone.Text); //手机号
            }

            MemberInfoBll miBll = new MemberInfoBll();
            var list = miBll.GetList(dic);
            if (list.Count > 0)
            {
                //根据信息查到到会员
                MemberInfo mi = list[0];
                lblMoney.Text = mi.MMoney.ToString();
                lblTypeTitle.Text = mi.MTypeTitle;
                lblDiscount.Text = mi.MDiscount.ToString();

                //计算折扣价
                lblPayMoneyDiscount.Text =
                    (Convert.ToDecimal(lblPayMoney.Text) * Convert.ToDecimal(lblDiscount.Text)).ToString();
            }
            else
            {
                MessageBox.Show("会员信息有误", "提示", MessageBoxButtons.OKCancel);
            }

        }
        private void txtId_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            LoadMember();
        }

        private void btnOrderPay_Click(object sender, EventArgs e)
        {
            //1 根据是否使用余额决定扣扣方式
            //2 将订单状态为 IsPage=1
            //3 将餐桌状态IsFree=1
            string s = lblPayMoneyDiscount.Text;
            try
            {
                if (oiBll.Pay(cbkMoney.Checked, int.Parse(txtId.Text), Convert.ToDecimal(lblPayMoneyDiscount.Text), orderId,
                       Convert.ToDecimal(lblDiscount.Text)))
                {
                    MessageBox.Show("结账成功,您的消费总金额是:" + s + "元");
                    Refresh();

                    this.Close();

                }
                else
                {
                    MessageBox.Show("结账失败，请稍后重试", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);


                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
