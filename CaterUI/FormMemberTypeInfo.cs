using CaderModel;
using CaterBll;
using System;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormMemberTypeInfo : Form
    {
        public FormMemberTypeInfo()
        {
            InitializeComponent();
        }
        //调用BLL
        MemberTypeInfoBll mtiBll = new MemberTypeInfoBll();

        private DialogResult result = DialogResult.Cancel;
        
        private void LoadList()
        {
            //禁用列表的自动生成
            dgvList.AutoGenerateColumns = false;
            //调用方法获取数据，绑定到列表的数据源上
            dgvList.DataSource = mtiBll.GetList();

        }

        private void FormMemberTypeInfo_Load_1(object sender, EventArgs e)
        {
            //加载列表
            LoadList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //接收用户输入的值，添加对象
            MemberTypeInfo mti = new MemberTypeInfo()
            {
                MTitle = txtTitle.Text,
                MDiscount = Convert.ToDecimal(txtDiscount.Text)
            };
            if (txtId.Text.Equals("添加时无编号"))
            {
                //添加操作
               
                //调用添加的方法,添加数据到表中
                if (mtiBll.Add(mti))
                {
                    LoadList();
                }
                else
                {
                    MessageBox.Show("添加失败，请稍后重试");
                }

            }
            else
            {
                //修改操作
                mti.MId = int.Parse(txtId.Text);
                //调用修改的方法
            if( mtiBll.Edit(mti))
                {
                    LoadList();
                    MessageBox.Show("修改成功");
                }
            else
                {
                    MessageBox.Show("修改失败，请稍后重试");
                }
            }

            //将控件还原
            txtId.Text = "添加时无编号";
            txtTitle.Clear();
            txtDiscount.Clear();
            btnSave.Text = "添加";

            result= DialogResult.OK;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //将控件还原
            txtId.Text = "添加时无编号";
            txtTitle.Clear();
            txtDiscount.Clear();
            btnSave.Text = "添加";

        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //获取点击的行
            var row = dgvList.Rows[e.RowIndex];
            //将行中列的值赋给文本框
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            txtDiscount.Text = row.Cells[2].Value.ToString();
            btnSave.Text = "修改";

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //获取选择行的编号
            var row = dgvList.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells[0].Value);
            //删除前的确认提示
            DialogResult resu = MessageBox.Show("确认要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (resu == DialogResult.Cancel)
            {
                //用户取消删除，就不进行删除
                return;
            }
            //进行删除
            if ( mtiBll.Remove(id))
            {
                LoadList();
                MessageBox.Show("删除成功");
            }
           else
            {
                MessageBox.Show("删除失败，请稍后重试");
            }
            result = DialogResult.OK;
        }

        private void FormMemberTypeInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = result;
        }
    }
}
