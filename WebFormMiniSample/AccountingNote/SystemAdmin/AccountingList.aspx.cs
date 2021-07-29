using AccountingNote_DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SystemAdmin
{
    public partial class AccountingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //確認登入
            if(this.Session["UserLoginInfo"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            string account = this.Session["UserLoginInfo"] as string;//取得帳號
            var dr = UserInfoManager.GetUserInfoByAccount(account);//取得使用者資料

            if(dr == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            var dt = AccountingManager.GetAccountingList(dr["ID"].ToString());//取得ID

            if(dt.Rows.Count > 0)
            {
                this.gvAccountingList.DataSource = dt;
                this.gvAccountingList.DataBind();//資料繫結
            }
            else//0筆資料的情況
            {
                this.gvAccountingList.Visible = false;
                this.plcNoData.Visible = true;
            }

           
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("/SystemAdmin/AccountingDetail.aspx");
        }

        protected void gvAccountingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if(row.RowType == DataControlRowType.DataRow)
            {
                //Literal lt1 = row.FindControl("ltActType") as Literal;
                //lt1.Text = "OK";
                Label lbl1 = row.FindControl("lblActType") as Label;

                var dr = row.DataItem as DataRowView;
                int actType = dr.Row.Field<int>("ActType");

                if (actType == 0)
                {
                    //lt1.Text = "支出";
                    lbl1.Text = "支出";
                }
                else
                {
                    //lt1.Text = "收入";
                    lbl1.Text = "收入";
                }

                if(dr.Row.Field<int>("Amount") > 15000)
                {
                    lbl1.ForeColor = Color.Green;
                }
            }
        }
    }
}