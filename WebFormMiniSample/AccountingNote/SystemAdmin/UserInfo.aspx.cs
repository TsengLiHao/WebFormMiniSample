using AccountingNote_DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SystemAdmin
{
    public partial class UserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.IsPostBack)//可能是按鈕跳回本頁,而非第一次登入
            {
                if(this.Session["UserLoginInfo"] == null)//表示尚未登入
                {
                    Response.Redirect("/Login.aspx");
                    return;
                }

                string account = this.Session["UserLoginInfo"] as string;//Session轉成字串,來看使用者存不存在
                DataRow dr = UserInfoManager.GetUserInfoByAccount(account);

                if(dr == null)//帳號被刪的情況
                {
                    this.Session["UserLoginInfo"] = null;//系統認為Session存在,會導致無限迴圈,所以清除登入資訊
                    Response.Redirect("/Login.aspx");
                    return;
                }

                this.ltAccount.Text = dr["Account"].ToString();
                this.ltName.Text = dr["Name"].ToString();
                this.ltEmail.Text = dr["Email"].ToString();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            this.Session["UserLoginInfo"] = null;
            Response.Redirect("/Login.aspx");
        }
    }
}