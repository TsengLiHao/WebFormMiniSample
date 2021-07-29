using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote_DBSource
{
    public class UserInfoManager
    {
        public static DataRow GetUserInfoByAccount(string account)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @"SELECT [ID], [Account], [PWD], [Name], [Email]
                FROM UserInfo
                WHERE [Account]  = @account
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@account", account));

            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.Writelog(ex);
                return null;
            }

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    using (SqlCommand command = new SqlCommand(dbCommandString, connection))
            //    {

            //        command.Parameters.AddRange(list.ToArray());

            //        try
            //        {
            //            connection.Open();
            //            SqlDataReader reader = command.ExecuteReader();

            //            DataTable dt = new DataTable();
            //            dt.Load(reader);
            //            reader.Close();

            //            if (dt.Rows.Count == 0)
            //                return null;
            //            DataRow dr = dt.Rows[0];
            //            return dr;
            //        }
            //        catch (Exception ex)
            //        {
            //            Logger.Writelog(ex);
            //            return null;//改回傳值
            //        }
            //    }
            //}
        }
        
    }
}
