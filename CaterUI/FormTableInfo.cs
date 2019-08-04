using CaderModel;
using CaterBll;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormTableInfo : Form
    {
        public FormTableInfo()
        {
            InitializeComponent();


        }
        private TableInfoBll tiBll = new TableInfoBll();

        public event Action Refresh;

        private void FormTableInfo_Load(object sender, EventArgs e)
        {
            dgvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadSerList();
            LoadList();

        }


        private void LoadList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (ddlHallSearch.SelectedIndex > 0)
            {
                dic.Add("tHallId", ddlHallSearch.SelectedValue.ToString());
            }
            if (ddlFreeSearch.SelectedIndex > 0)
            {
                dic.Add("tIsFree", ddlFreeSearch.SelectedValue.ToString());
            }

            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = tiBll.GetList(dic);
        }

        private void LoadSerList()
        {
            //厅包-显示框
            HallInfoBll hiBll = new HallInfoBll();
            var list = hiBll.GetList();

            list.Insert(0, new HallInfo()
            {
                HId = 0,
                HTitle = "全部"
            });
            ddlHallSearch.DataSource = list;
            ddlHallSearch.ValueMember = "hid";
            ddlHallSearch.DisplayMember = "htitle";

            //空闲-显示框 ，在UI层建立了 DdlModel类
            ddlHallAdd.DataSource = hiBll.GetList();
            ddlHallAdd.ValueMember = "hid";
            ddlHallAdd.DisplayMember = "htitle";

            List<DdlModel> listDdl = new List<DdlModel>()
            {
                new DdlModel("-1","全部"),
                new DdlModel("1","空闲"),
                new DdlModel("0","使用中")
            };
            ddlFreeSearch.DataSource = listDdl;
            ddlFreeSearch.ValueMember = "id";
            ddlFreeSearch.DisplayMember = "title";
        }


        private void ddlHallSearch_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadList();
        }

        private void ddlFreeSearch_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadList();
        }
        private void btnSearchAll_Click_1(object sender, EventArgs e)
        {
            //显示全部 - 按钮
            ddlHallSearch.SelectedIndex = 0;
            ddlFreeSearch.SelectedIndex = 0;
            LoadList();
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            //接收用户值，构造对象
            TableInfo ti = new TableInfo()
            {
                TTitle = txtTitle.Text,
                THallId = Convert.ToInt32(ddlHallAdd.SelectedValue),
                TIsFree = rbFree.Checked
            };

            if (txtId.Text == "添加时无编号")
            {
                #region 添加

                if (tiBll.Add(ti))
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

                ti.TId = int.Parse(txtId.Text);
                if (tiBll.Edit(ti))
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

            //恢复控件值
            txtId.Text = "添加时无编号";
            txtTitle.Text = "";
            ddlHallAdd.SelectedIndex = 0;
            rbFree.Checked = true;
            btnSave.Text = "添加";

            Refresh();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            //恢复控件值
            txtId.Text = "添加时无编号";
            txtTitle.Clear();
            ddlHallSearch.SelectedIndex = 0;
            rbFree.Checked = true;
            btnSave.Text = "添加";
        }


        private void btnRemove_Click_1(object sender, EventArgs e)
        {
            var id = Convert.ToInt32(dgvList.SelectedRows[0].Cells[0].Value);
            DialogResult resu = MessageBox.Show("确认要删除吗？", "温馨提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (resu == DialogResult.OK)
            {
                if (tiBll.Remove(id))
                {
                    LoadList();
                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show("删除失败，请稍后重试");
                }
            }
            Refresh(); //调用

        }

        private void dgvList_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitle.Text = row.Cells[1].Value.ToString();
            ddlHallAdd.Text = row.Cells[2].Value.ToString();
            //空闲还是选中
            if (Convert.ToBoolean(row.Cells[3].Value))
            {
                rbFree.Checked = true; //值为1，则经理选中
            }
            else
            {
                rbUnFree.Checked = true;//如果值为0，则店员选中
            }


            btnSave.Text = "修改";
        }

        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                e.Value = Convert.ToBoolean(e.Value) ? "空闲中" : "正在使用中";
            }
        }

        private void btnAddHall_Click_1(object sender, EventArgs e)
        {
            FormHallInfo frmHi = new FormHallInfo();
            frmHi.MyUpadateForm += LoadSerList; //事件中的方法都会被执行
            frmHi.MyUpadateForm += LoadList;
            frmHi.Show();

        }
    }
}
