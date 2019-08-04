using CaderModel;
using CaterBll;
using System;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FM : Form
    {
        public FM()
        {
            InitializeComponent();
        }
        //实现窗体的单例
        private static FM _form;
        public static FM Cradte()
        {
            if (_form==null)
            {
                _form = new FM();
            }
            return _form;
        }
        //创建业务逻辑层对象
        ManagerInfoBll miBll = new ManagerInfoBll();
        private void FormManagerInfo_Load(object sender, EventArgs e)
        {
            //加载列表
            LoadList();
        }
        private void LoadList()
        {


            //禁用列表的自动生成(取消列表的自动生成列功能)
            dgvList.AutoGenerateColumns = false;
            //调用方法获取数据，绑定到列表的数据源上
            dgvList.DataSource = miBll.GetList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        { //接收用户输入
            ManagerInfo mi = new ManagerInfo()
            {
                MName = textName.Text,
                MPwd = textPwd.Text,
                MType = rb1.Checked ? 1 : 0, //经理值为1，店员值为0
            };
            if (textid.Text.Equals("添加时无编号"))
            {

                #region 添加

                //调用bll的Add方法
                if (miBll.Add(mi))
                {
                    //如果添加成功，则重新加载数据
                    LoadList();

                }
                else
                {
                    MessageBox.Show("添加失败，请检查后在添加");

                }
                #endregion
            }
            else
            {
                #region 修改
                mi.MId = int.Parse(textid.Text);
                if (miBll.Edit(mi))
                {
                    LoadList();
                }
                #endregion
            }

            //清除文本框中的值
            textName.Clear();
            textPwd.Clear();
            rb2.AutoCheck = true;
            btnAdd.Text = "添加";
            textid.Text = "添加时无编号";
        }

        //CellFormatting 事件
        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //对类型列进行格式化处理
            if (e.ColumnIndex == 2)
            {
                //把1转换成经理  0转换成店员
                //根据类型判断内容
                e.Value = Convert.ToInt32(e.Value) == 1 ? "经理" : "店员";
            }
        }



        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //根据当前点击的单元格，找到行与列，进行赋值
            //根据索引找到行
            DataGridViewRow row = dgvList.Rows[e.RowIndex];
            //根据ID找到对应列
            textid.Text = row.Cells[0].Value.ToString();
            textName.Text = row.Cells[1].Value.ToString();
            if (row.Cells[2].Value.ToString().Equals("1"))
            {
                rb1.Checked = true; //值为1，则经理选中
            }
            else
            {
                rb2.Checked = true;//如果值为0，则店员选中
            }

            //指定密码的值
            textPwd.Text = "这是原来的密码吗";
            btnAdd.Text = "修改";
        }


        /// <summary>
        /// 取消按钮键，恢复最初状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btncancel_Click(object sender, EventArgs e)
        {
            textid.Clear();
            textName.Clear();
            textPwd.Clear();
            rb2.Checked = true;
            btnAdd.Text = "添加";
        }

        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btndelete_Click(object sender, EventArgs e)
        {
            //获取选中行
            var rows = dgvList.SelectedRows;
            if (rows.Count > 0)
            {
                //删除前的确认提示
                DialogResult resu = MessageBox.Show("确认要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if(resu==DialogResult.Cancel)
                {
                    //用户取消删除
                    return;
                }

                //获取选中的行
                int id = int.Parse(rows[0].Cells[0].Value.ToString());

                //调用删除的操作
                if(miBll.Remover(id))
                {
                    LoadList();
                    MessageBox.Show("删除成功");
                }

            }
            else
            {
                MessageBox.Show("请选择要删除的行");
            }
        }

        private void 删除此行数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //获取选中行
            var rows = dgvList.SelectedRows;
            if (rows.Count > 0)
            {
                //删除前的确认提示
                DialogResult resu = MessageBox.Show("确认要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (resu == DialogResult.Cancel)
                {
                    //用户取消删除
                    return;
                }

                //获取选中的行
                int id = int.Parse(rows[0].Cells[0].Value.ToString());
                //调用删除的操作
                if (miBll.Remover(id))
                {
                    LoadList();
                    MessageBox.Show("删除成功");
                }

            }
            else
            {
                MessageBox.Show("请选择要删除的行");
            }
        }


        private void dgvList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //右键全部选中
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                dgvList.CurrentRow.Selected = false;
                dgvList.Rows[e.RowIndex].Selected = true;
            }
        }

        private void FM_FormClosing(object sender, FormClosingEventArgs e)
        {
            //与单利保持一直，出现这种代码的原因是，Form的Close()会释放当前窗体对象
            _form = null;   //添加了 FormClosing 事件，让关闭 窗体在打开时不会出错！
        }
    }
}
