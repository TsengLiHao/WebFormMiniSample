using AccountingNote_DBSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AccountingNote.SystemAdmin
{
    public partial class AccountingDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //確認登入
            if (this.Session["UserLoginInfo"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            string account = this.Session["UserLoginInfo"] as string;//取得帳號
            var drUserInfo = UserInfoManager.GetUserInfoByAccount(account);//取得使用者資料

            if (drUserInfo == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                //check is createmode or editmode
                if (this.Request.QueryString["ID"] == null)
                {
                    this.btnDelete.Visible = false;
                }
                else
                {
                    this.btnDelete.Visible = true;

                    string idText = this.Request.QueryString["ID"];
                    int id;
                    if (int.TryParse(idText, out id))
                    {
                        var drAccounting = AccountingManager.GetAccounting(id,drUserInfo["ID"].ToString());

                        if (drAccounting == null)
                        {
                            this.ltMsg.Text = "Data does not exist.";
                            this.btnSave.Visible = false;
                            this.btnDelete.Visible = false;
                        }
                        else
                        {
                            //if(drAccounting["UserID"] == drUserInfo["ID"].ToString()) { }
                            this.ddlActType.SelectedValue = drAccounting["ActType"].ToString();
                            this.txtAmount.Text = drAccounting["Amount"].ToString();
                            this.txtCaption.Text = drAccounting["Caption"].ToString();
                            this.txtDesc.Text = drAccounting["Body"].ToString();
                        }
                    }
                    else
                    {
                        this.ltMsg.Text = "ID is required.";
                        this.btnSave.Visible = false;
                        this.btnDelete.Visible = false;
                    }


                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> msgList = new List<string>();
            if (!this.Checkinput(out msgList))
            {
                this.ltMsg.Text = string.Join("<br/>", msgList);
                return;
            }

            string account = this.Session["UserLoginInfo"] as string;//取得帳號
            var dr = UserInfoManager.GetUserInfoByAccount(account);//取得使用者資料

            if (dr == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            string userID = dr["ID"].ToString();
            string actTypeText = this.ddlActType.SelectedValue;
            string amountText = this.txtAmount.Text;
            string caption = this.txtCaption.Text;
            string body = this.txtDesc.Text;

            int amount = Convert.ToInt32(amountText);
            int actType = Convert.ToInt32(actTypeText);

            string idText = this.Request.QueryString["ID"];

            if (string.IsNullOrWhiteSpace(idText))
            {
                AccountingManager.CreateAccounting(userID, caption, amount, actType, body);
            }
            else
            {
                int id;
                if(int.TryParse(idText,out id))
                {
                    AccountingManager.UpdateAccounting(id, userID, caption, amount, actType, body);
                }
            }
            Response.Redirect("/SystemAdmin/AccountingList.aspx");

        }

        private bool Checkinput(out List<string> errorMsgList)//字串清單 將錯誤資料做回傳
        {
            List<string> msgList = new List<string>();

            if(this.ddlActType.SelectedValue != "0" && this.ddlActType.SelectedValue != "1")
            {
                msgList.Add("Type should be 0 or 1.");
            }

            if(string.IsNullOrWhiteSpace(this.txtAmount.Text))
            {
                msgList.Add("Amount is required.");
            }
            else
            {
                int tempint;
                if(!int.TryParse(this.txtAmount.Text,out tempint))//確認是否能轉整數
                {
                    msgList.Add("Amount should be a number.");
                }

                if(tempint < 0 || tempint > 1000000)
                {
                    msgList.Add("Amount must between 0 and 1,000,000.");
                }
            }
            //Caption,Desc非必填,所以不做驗證

            errorMsgList = msgList;//使用out,所以要在內部做初始化,不論有無錯誤訊息都宣告

            if (msgList.Count == 0)//沒有任何錯誤訊息
                return true;
            else
                return false;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idText = this.Request.QueryString["ID"];

            if (string.IsNullOrWhiteSpace(idText))
            {
                return;
            }
            else
            {
                int id;
                if (int.TryParse(idText, out id))
                {
                    AccountingManager.DeleteAccounting(id);
                }
            }
            Response.Redirect("/SystemAdmin/AccountingList.aspx");
        }
    }
}