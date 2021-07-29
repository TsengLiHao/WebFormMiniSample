using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote_DBSource
{
    public class Logger
    {
        public static void Writelog(Exception ex)
        {
            string msg =
                $@"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
                   {ex.ToString()}
                ";

            System.IO.File.AppendAllText("C\\Log\\Log.log", msg);

            throw ex;
        }
    }
}
