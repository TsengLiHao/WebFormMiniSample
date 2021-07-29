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
    public class AccountingManager
    {
        public static DataTable GetAccountingList(string userID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbcommand =
                $@"SELECT
                       ID,
                       Caption,
                       Amount,
                       ActType,
                       CreateDate
                   FROM Accounting
                   WHERE userID = @userID
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@userID", userID));
            //comm.Parameters.AddWithValue("@userID", userID);
            try
            {
                return DBHelper.ReadDataTable(connStr, dbcommand, list);
            }
            catch (Exception ex)
            {
                Logger.Writelog(ex);
                return null;
            }
        }

        public static void CreateAccounting(string userID, string caption, int amount, int actType, string body)
        {
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("Amount must between 0 and 1,000,000.");
            if (actType < 0 || actType > 1)
                throw new ArgumentException("ActType must be 0 or 1.");

            string connStr = DBHelper.GetConnectionString();
            string dbcommand =
                $@" INSERT INTO [dbo].[Accounting]
                   (
                       UserID
                       ,Caption
                       ,Amount
                       ,ActType
                       ,CreateDate
                       ,Body
                   )
                   VALUES
                   (
                       @userID
                       ,@caption
                       ,@amount
                       ,@actType
                       ,@createDate
                       ,@body
                   )
                ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbcommand, conn))
                {
                    comm.Parameters.AddWithValue("@userID", userID);
                    comm.Parameters.AddWithValue("@caption", caption);
                    comm.Parameters.AddWithValue("@amount", amount);
                    comm.Parameters.AddWithValue("@actType", actType);
                    comm.Parameters.AddWithValue("@createDate", DateTime.Now);
                    comm.Parameters.AddWithValue("@body", body);

                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Logger.Writelog(ex);
                    }
                }
            }
        }

        public static bool UpdateAccounting(int ID, string userID, string caption, int amount, int actType, string body)
        {
            if (amount < 0 || amount > 1000000)
                throw new ArgumentException("Amount must between 0 and 1,000,000.");
            if (actType < 0 || actType > 1)
                throw new ArgumentException("ActType must be 0 or 1.");

            string connStr = DBHelper.GetConnectionString();
            string dbcommand =
                $@" UPDATE [Accounting]
                    SET
                       UserID         =  @userID
                       ,Caption       =  @caption
                       ,Amount        =  @amount
                       ,ActType       =  @actType
                       ,CreateDate    =  @createDate
                       ,Body          =  @body
                    WHERE
                        ID=@id
                ";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbcommand, conn))
                {
                    comm.Parameters.AddWithValue("@userID", userID);
                    comm.Parameters.AddWithValue("@caption", caption);
                    comm.Parameters.AddWithValue("@amount", amount);
                    comm.Parameters.AddWithValue("@actType", actType);
                    comm.Parameters.AddWithValue("@createDate", DateTime.Now);
                    comm.Parameters.AddWithValue("@body", body);
                    comm.Parameters.AddWithValue("@id", ID);

                    try
                    {
                        conn.Open();
                        int effectRows = comm.ExecuteNonQuery();

                        if (effectRows == 1)
                            return true;
                        else
                            return false;
                    }
                    catch (Exception ex)
                    {
                        Logger.Writelog(ex);
                        return false;
                    }
                }
            }
        }

        public static DataRow GetAccounting(int id,string userID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbcommand =
                $@"SELECT
                       ID,
                       Caption,
                       Amount,
                       ActType,
                       CreateDate,
                       Body
                   FROM Accounting
                   WHERE ID = @id AND UserID = @userID
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@id", id));
            list.Add(new SqlParameter("@userID", userID));
            try
            {
                return DBHelper.ReadDataRow(connStr, dbcommand, list);
            }
            catch (Exception ex)
            {
                Logger.Writelog(ex);
                return null;
            }
        }

        public static void DeleteAccounting(int ID)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbcommand =
                $@"DELETE[Accounting]
                   WHERE ID = @id";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand comm = new SqlCommand(dbcommand, conn))
                {
                    comm.Parameters.AddWithValue("@id", ID);

                    try
                    {
                        conn.Open();
                        var reader = comm.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                    }
                    catch (Exception ex)
                    {
                        Logger.Writelog(ex);
                    }
                }
            }
        }
    }
}
