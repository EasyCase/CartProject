using CaterBll;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        OrderInfoBll oiBll = new OrderInfoBll();
        private void menuQuit_Click(object sender, EventArgs e)
        {
            //当退出时所有窗体都马上关闭
            Application.Exit();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //当关闭窗体时所有窗体都马上关闭
            Application.Exit();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //判断登陆进来用户的级别，以确定是否显示会员menuManager图标的显示
            int type = Convert.ToInt32(this.Tag);
            if (type == 1)
            {
                //经理登陆 1 经理登陆显示menuManager图标

            }
            else
            {
                //店员登陆 0
                menuManagerInfo.Visible = false; //隐藏menuManager图标
            }
            //加载所有的厅包信息
            LoadHallInfo();
        }

        private void menuManagerInfo_Click(object sender, EventArgs e)
        {
            //调用 FM窗体
            FM FormManagerInfo = FM.Cradte(); //使用了单利模式，防止窗口无限制点出来
            FormManagerInfo.Show();
            FormManagerInfo.Focus(); //将当前窗体获得焦点
        }

        private void menuMemberInfo_Click(object sender, EventArgs e)
        {
            FormMemberInfo frm = new FormMemberInfo();
            frm.Show();
        }

        private void menuDisHinfo_Click(object sender, EventArgs e)
        {
            FormDishInfo frm = new FormDishInfo();
            frm.Show();
        }

        private void menuTableInfo_Click(object sender, EventArgs e)
        {
            FormTableInfo frm = new FormTableInfo();
            frm.Refresh += LoadHallInfo;  //事件委托
            frm.Show();
        }

        TableInfoBll tiBll = new TableInfoBll();
        private void LoadHallInfo()
        {
            //2.1 获取所有的厅包对象
            HallInfoBll hiBll = new HallInfoBll();
            var list = hiBll.GetList();
            //2.2 遍历集合，向标签页中添加信息
            tcHllinfo.TabPages.Clear(); //事件委托 清空
            foreach (var hi in list)
            {
                //根据厅包对象创建标签页对象
                TabPage tp = new TabPage(hi.HTitle);
                //获取当前厅包对象的所有餐桌
                Dictionary<string, string> dic = new Dictionary<string, string>();

                dic.Add("thallid", hi.HId.ToString());
                TableInfoBll tiBll = new TableInfoBll();
                var listTableInfo = tiBll.GetList(dic);
                //3.1 动态创建列表添加到标签页面上 ，桌面图片
                ListView lvTableInfo = new ListView();
                //添加双击事件，完成开单功能
                lvTableInfo.DoubleClick += LvTableInfo_DoubleClick;
                //3.2 让列表使用图片
                lvTableInfo.LargeImageList = imageList1;
                //4.1 填满
                lvTableInfo.Dock = DockStyle.Fill;
                tp.Controls.Add(lvTableInfo);
                //4.2 向列表中添加餐桌信息
                foreach (var ti in listTableInfo)
                {
                    var lvi = new ListViewItem(ti.TTitle, ti.TIsFree ? 0 : 1);

                    //后续操作需要用到餐桌编号，所以在这里存以下
                    lvi.Tag = ti.TId;

                    lvTableInfo.Items.Add(lvi);
                }

                //将标签页加入标签容器
                tcHllinfo.TabPages.Add(tp);
            }

        }

        private void LvTableInfo_DoubleClick(object sender, EventArgs e)
        {
            //获取被点的餐桌项
            var lv1 = sender as ListView;
            var lvi = lv1.SelectedItems[0];
            // 获取餐桌编号
            int tableId = Convert.ToInt32(lv1.Tag);
            if (lvi.ImageIndex == 0)
            {
                //当前餐桌为空闲则开单
               

                //1 开单，向orderinfo表插入数据
                //2 修改餐桌状态为占用
               
                int orderId = oiBll.KaiDan(tableId);
                lvi.Tag = orderId;
                //3 更新项的图标为占用
                lv1.SelectedItems[0].ImageIndex = 1;
            }
            else
            {
                //当前餐桌已经占用。则进行点菜操作
                lvi.Tag = oiBll.GetOrderIdByTableId(tableId);

            }

            //4 打开点菜窗体
            FormOredrDish formOredrDish = new FormOredrDish();
            formOredrDish.Tag = lvi.Tag;
            formOredrDish.Show();

        }

        private void MenuOrder_Click(object sender, EventArgs e)
        {
            //先找到选中的标签页，在找到listVliew 在找到选中的项
            //项中存储了餐桌编号，由餐桌编号查到订单编号
            var listView = tcHllinfo.SelectedTab.Controls[0] as ListView;
            var lvTable = listView.SelectedItems[0];
            if (lvTable.ImageIndex == 0)
            {
                MessageBox.Show("餐桌还未使用，无法结账","提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Stop);
                return;
            }
            int tableId = Convert.ToInt32(listView.Tag);
            int orderId = oiBll.GetOrderIdByTableId(tableId);

            //打开付款窗体
            FormOrderPay frmOP = new CaterUI.FormOrderPay();
            frmOP.Tag = orderId;
            frmOP.Refresh += LoadHallInfo;
            frmOP.Show();
        }
    }
}
