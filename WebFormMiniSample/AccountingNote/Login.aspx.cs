using AccountingNote_DBSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(this.Session["UserLoginInfo"] != null)//登入成功 Session可以看做登入資料的紀錄
            {
                this.plcLogin.Visible = false;//跳頁
                Response.Redirect("/SystemAdmin/UserInfo.aspx");
            }
            else
                this.plcLogin.Visible = true;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //string db_Account = "ABC";
            //string db_Password = "12345";

            string inp_Account = this.txtAccount.Text;
            string inp_PWD = this.txtPWD.Text;

            if(string.IsNullOrWhiteSpace(inp_Account)||string.IsNullOrWhiteSpace(inp_PWD))
            {
                this.ltMsg.Text = "Account / Pwd is required.";
                return;
            }

            var dr = UserInfoManager.GetUserInfoByAccount(inp_Account);//根據使用者輸入的資料查看資料庫是否有與其符合的資料

            if(dr == null)
            {
                this.ltMsg.Text = "Account does not exists.";
                return;
            }

            if(string.Compare(dr["Account"].ToString(),inp_Account,true) == 0 && 
               string.Compare(dr["PWD"].ToString(), inp_PWD,false) == 0)// false為區分大小寫
            {
                this.Session["UserLoginInfo"] = dr["Account"].ToString();
                Response.Redirect("/SystemAdmin/UserInfo.aspx");
            }
            else
            {
                this.ltMsg.Text = "Login fail.";
                return;
            }
        }
    }
}