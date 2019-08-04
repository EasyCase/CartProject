using CaderModel;
using CaterBll;
using System;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormHallInfo : Form
    {
        public FormHallInfo()
        {
            InitializeComponent();
            hiBll = new HallInfoBll();
            
        }
        private HallInfoBll hiBll;
        public event Action MyUpadateForm; //事件中的方法都会被执行
        private void FormHallInfo_Load(object sender, EventArgs e)
        {
            LoadList();
            //填充旁边空处
            dgvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        
        private void LoadList()
        {
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = hiBll.GetList();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            HallInfo hi = new HallInfo()
            {
                HTitle = txtTitle.Text
            };

            if (txtId.Text == "添加时无编号")
            {
                #region 添加
                if (hiBll.Add(hi))
                {
                    LoadList();
                    MessageBox.Show("添加成功");
                }
                else
                {
                    MessageBox.Show("添加失败，请稍后重试");
                }
                #endregion
            }
            else
            {
                #region 修改
                hi.HId = int.Parse(txtId.Text);
                if (hiBll.Edit(hi))
                {
                    LoadList();
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show("修改失败，请稍后重试");
                }
                #endregion
            }
            //恢复默认值
            txtId.Text = "添加时无编号";
            txtTitle.Clear();
            btnSave.Text = "添加";

            MyUpadateForm(); //调用事件，事件中的方法都会被执行
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //恢复默认值
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
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(dgvList.SelectedRows[0].Cells[0].Value);
            DialogResult resu = MessageBox.Show("确认要删除吗！", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if(resu==DialogResult.Cancel)
            {
                return;
            }

            if(hiBll.Remove(id))
            {
                LoadList();
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show("删除失败，请稍后重试");
            }
            MyUpadateForm(); //调用事件，事件中的方法都会被执行
        }
    }
}
