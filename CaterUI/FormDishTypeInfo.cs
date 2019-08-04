using Cater.Model;
using CaterBll;
using System;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormDishTypeInfo : Form
    {
        public FormDishTypeInfo()
        {
            InitializeComponent();
        }
        private DialogResult result = DialogResult.Cancel;
        private static FormDishTypeInfo _form;
        public static FormDishTypeInfo Cradte()
        {
            if (_form == null)
            {
                _form = new FormDishTypeInfo();
            }
            return _form;
        }
        private int rowIndex = -1;
        private void FormDishTypeInfo_Load(object sender, EventArgs e)
        {
            LoadList();
        }
        DishTypeInfoBll dtibll = new DishTypeInfoBll();
        private void LoadList()
        {
            //填充旁边灰色地带
            dgvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = dtibll.GetList();
            //设置某行选择
            if (rowIndex >= 0)
            {
                dgvList.Rows[rowIndex].Selected = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //根据用户输入构造对象
            DishTypeInfo dti = new DishTypeInfo()
            {
                DTitle = txtTitle.Text
            };
            //添加的逻辑，如果 txtID 等于 添加时无编号 就执行以下添加
            if (txtId.Text == "添加时无编号")
            {
                if (dtibll.Add(dti))
                {
                    LoadList();
                    MessageBox.Show("添加成功");
                }
                else
                {
                    MessageBox.Show("添加失败，请稍后重试");

                }
            }
            else

            {
                //修改
                dti.DId = int.Parse(txtId.Text);
                if (dtibll.Edit(dti))
                {
                    LoadList();
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show("修改失败");
                }
            }

            //清除控件值
            txtId.Text = "添加时无编号";
            txtTitle.Clear();
            btnSave.Text = "添加";
            this.result = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //清除控件值
            txtId.Text = "添加时无编号";
            txtTitle.Clear();
            btnSave.Text = "添加";
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            btnSave.Text = "修改";
            //记录被点击行的索引,用于刷新后再次被选择
            rowIndex = e.RowIndex;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //删除前的确认提示
            DialogResult resu = MessageBox.Show("确认要删除吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (resu == DialogResult.Cancel)
            {
                //用户取消删除
                return;
            }

            var row = dgvList.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells[0].Value);
            if(dtibll.Delete(id))
            {
                LoadList();
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show("删除失败，请稍后重试");
            }
            this.result = DialogResult.OK;
        }

        private void FormDishTypeInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = this.result;
        }
    }
}
