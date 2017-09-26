using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Util
{
    public class DBHelper
    {
        public static DataSet GetDataSet(
            string connectionString,
            string commandText,
            List<SqlParameter> paramsList,
            bool isRollbackForTest = false,
            CommandType argCommandType = CommandType.StoredProcedure)
        {
            DataSet r = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = null;
                SqlTransaction tran = null;

                if (isRollbackForTest == true) // 实现测试回滚
                {
                    tran = conn.BeginTransaction();
                    cmd = tran.Connection.CreateCommand();
                    cmd.Transaction = tran;
                }
                else // 普通执行模式
                {
                    cmd = conn.CreateCommand();
                }

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = commandText;

                if (paramsList != null && paramsList.Count > 0)
                {
                    cmd.Parameters.AddRange(paramsList.ToArray());
                }

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(r);

                // 实现测试回滚
                if (isRollbackForTest == true)
                {
                    tran.Rollback();
                }
            }

            return r;
        }

        public static DataSet GetDataSet(
            SqlTransaction tran,
            string commandText,
            List<SqlParameter> paramsList,
            CommandType argCommandType = CommandType.StoredProcedure)
        {
            DataSet r = new DataSet();

            SqlCommand cmd = tran.Connection.CreateCommand();
            cmd.Transaction = tran;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = commandText;

            if (paramsList != null && paramsList.Count > 0)
            {
                cmd.Parameters.AddRange(paramsList.ToArray());
            }

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(r);

            return r;
        }

    }
}
