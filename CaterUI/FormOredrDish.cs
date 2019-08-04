﻿using Cater.Model;
using CaterBll;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CaterUI
{
    public partial class FormOredrDish : Form
    {
        public FormOredrDish()
        {
            InitializeComponent();
        }

        private void FormOredrDish_Load(object sender, EventArgs e)
        {
            LoadDetailList();
            LoadDishType();
            LoadDishInfo();


        }
        OrderInfoBll oiBll = new OrderInfoBll();
        private void LoadDishInfo()
        {
            //拼接查询条件 
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (txtTitle.Text != "")
            {
                dic.Add("dchar", txtTitle.Text);
            }
            if (ddlType.SelectedValue.ToString() != "0")
            {
                dic.Add("dtypeId", ddlType.SelectedValue.ToString());
            }

            //查询菜品，显示到表格中
            DishInfoBll diBll = new DishInfoBll();
            dgvAllDish.AutoGenerateColumns = false;
            dgvAllDish.DataSource = diBll.GetList(dic);

            //填充空白处
            dgvAllDish.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrderDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadDishType()
        {
            DishTypeInfoBll dtiBll = new DishTypeInfoBll();
            var list = dtiBll.GetList();

            list.Insert(0, new DishTypeInfo()
            {
                DId = 0,
                DTitle = "全部"
            });

            ddlType.ValueMember = "did";
            ddlType.DisplayMember = "dtitle";
            ddlType.DataSource = list;
        }

        private void LoadDetailList()
        {
            int orderId = Convert.ToInt32(this.Tag);
            dgvOrderDetail.AutoGenerateColumns = false;
            dgvOrderDetail.DataSource = oiBll.GetDetaList(orderId);
            GetTotaiMoneyByOrderId();
        }

        //计算金额方法
        private void GetTotaiMoneyByOrderId()
        {
            int orderId = Convert.ToInt32(this.Tag);
            lblMoney.Text = oiBll.GetTotaiMoneByOrderId(orderId).ToString();
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }

        private void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDishInfo();
        }

        private void dgvAllDish_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //点菜操作
            //订单编号
            int orderId = Convert.ToInt32(this.Tag);
            //菜单编号
            int dishId = Convert.ToInt32(dgvAllDish.Rows[e.RowIndex].Cells[0].Value);

            //执行点菜操作

            if (oiBll.DianCai(orderId, dishId))
            {
                //点菜成功
                LoadDetailList();
            }
        }
        //CellEndEdit事件
        private void dgvOrderDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //先判断 从数量列2执行
            if (e.ColumnIndex == 2)
            {
                //先拿到修改的行
                var row = dgvOrderDetail.Rows[e.RowIndex];
                //获取Oid
                int oid = Convert.ToInt32(row.Cells[0].Value);
                //获取数量，  从0到2
                int count = Convert.ToInt32(row.Cells[2].Value);
                //执行更新操作
                oiBll.UpdateCountByOid(oid, count);

                //从新计算总价格
                GetTotaiMoneyByOrderId();
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            //获取订单编号
            int orderId = Convert.ToInt32(this.Tag);
            //获取总金额
            decimal money = Convert.ToDecimal(lblMoney.Text);
            //更新订单
            if (oiBll.SetOrderMoney(orderId, money))
            {
                MessageBox.Show("下单成功");
            }

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                return;
            }
            //获取编号
            try
            {
                int oid = Convert.ToInt32(dgvOrderDetail.SelectedRows[0].Cells[0].Value);
                //执行删除
                if (oiBll.DeleteDetailByid(oid))
                {
                    LoadDetailList();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("请选中整行,单击最左边的", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
          
        }

 
    }
}
