using CaderModel;
using Cater.Model;
using CaterBll;
using CaterCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormDishInfo : Form
    {
        public FormDishInfo()
        {
            InitializeComponent();
        }
        private DishInfoBll diBll = new DishInfoBll();
        private void FormDishInfo_Load(object sender, EventArgs e)
        {
            LoadTypeList();
            LoadList();


        }

        private void LoadList()
        {
            //拼接条件
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (txtTitleSearch.Text != "")
            {
                dic.Add("dtitle", txtTitleSearch.Text);
            }
            if (ddlTypeSearch.SelectedValue.ToString() != "0")
            {
                dic.Add("DTypeId", ddlTypeSearch.SelectedValue.ToString());
            }

            dgvList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = diBll.GetList(dic);
            //设置某行 修改后不跳转到第一行去
            if (dgvSelectedIndex > -1)
            {
                dgvList.Rows[dgvSelectedIndex].Selected = true;
            }
        }

        private void LoadTypeList()
        {
            DishTypeInfoBll dtiBll = new DishTypeInfoBll();
            List<DishTypeInfo> list = dtiBll.GetList();
            #region 绑定查询的下拉列表
            //向list中插入数据
            list.Insert(0, new DishTypeInfo()
            {
                DId = 0,
                DTitle = "全部"

            });

            ddlTypeSearch.DataSource = list;
            ddlTypeSearch.ValueMember = "did";
            ddlTypeSearch.DisplayMember = "dtitle";
            #endregion

            #region 绑定添加的下拉列表
            ddlTypeAdd.DataSource = dtiBll.GetList();
            ddlTypeAdd.DisplayMember = "dtitle";
            ddlTypeAdd.ValueMember = "did";
            #endregion
        }
        private void txtTitleSearch_Leave(object sender, EventArgs e)
        {
            LoadList();
        }

        private void ddlTypeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            txtTitleSearch.Text = "";
            ddlTypeSearch.SelectedIndex = 0; //全部
            LoadList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //收集用户输入的信息
            DishInfo di = new DishInfo()
            {
                DTitle = txtTitleSave.Text,
                DChar = txtChar.Text,
                DPrice = Convert.ToDecimal(txtPrice.Text),
                DTypeId = Convert.ToInt32(ddlTypeAdd.SelectedValue)
            };
            if (txtId.Text == "添加时无编号")
            {
                #region 添加
                if (diBll.Add(di))
                {
                    LoadList();
                    MessageBox.Show("添加成功");
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
                #endregion
            }
            else
            {
                #region 修改
                di.DId = int.Parse(txtId.Text);
                if (diBll.Edit(di))
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

            #region 恢复控件
            txtId.Text = "添加时无编号";
            txtPrice.Clear();
            txtChar.Clear();
            txtTitleSave.Clear();
            ddlTypeAdd.SelectedIndex = 0;
            #endregion
        }

        private void txtTitleSave_Leave(object sender, EventArgs e)
        {
            txtChar.Text = PinyinHelper.GetPinyin(txtTitleSave.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtId.Text = "添加时无编号";
            txtPrice.Clear();
            txtChar.Clear();
            txtTitleSave.Clear();
            ddlTypeAdd.SelectedIndex = 0;
        }

        private void btnAddType_Click(object sender, EventArgs e)
        {
            FormDishTypeInfo formDti = new FormDishTypeInfo();


            //以模态窗口打开分类管理
            DialogResult result = formDti.ShowDialog();
            //根据返回的值，判断是否要更新下拉列表
            if (result == DialogResult.OK)
            {
                LoadTypeList();
                LoadList();
            }
        }
        //修改后保持在原来位置上不跳转到第一行
        private int dgvSelectedIndex = -1;
        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //设置某行 修改后不跳转到第一行去
            dgvSelectedIndex = e.RowIndex;
            //获取点击的行
            var row = dgvList.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtTitleSave.Text = row.Cells[1].Value.ToString();
            ddlTypeAdd.Text = row.Cells[2].Value.ToString();
            txtPrice.Text = row.Cells[3].Value.ToString();
            txtChar.Text = row.Cells[4].Value.ToString();
            btnSave.Text = "修改";
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


            var row = Convert.ToInt32(dgvList.SelectedRows[0].Cells[0].Value);
        // 也可以用这   int id = Convert.ToInt32(row.Cells[0].Value);
            if (diBll.Remove(row))
            {
                LoadList();
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show("删除失败，请稍后重试");
            }
        }
    }
}
