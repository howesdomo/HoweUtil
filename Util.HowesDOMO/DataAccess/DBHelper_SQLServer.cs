using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Util
{
    /// <summary>
    /// V 1.0.1 - 2020-09-29 15:56:50
    /// 由于已修改 DBHelper 代码为支持多种关系型数据库的方式, 故原来的 DBHelper 更名为 DBHelper_SQLServer,
    /// 方便旧项目升级使用, 新项目应该直接使用 DBHelper 的形式
    /// </summary>
    public class DBHelper_SQLServer
    {
        public static DataSet GetDataSet
        (
            string connectionString,
            string commandText,
            List<SqlParameter> paramsList,
            bool isBeginTransaction = false,
            bool isRollbackForTest = false,
            CommandType argCommandType = CommandType.StoredProcedure
        )
        {
            List<object> l = new List<object>();
            paramsList.ForEach(i => l.Add(i));

            return Util.DBHelper.GetDataSet
            (
                Util.DBHelper.GetDbProviderFactory(DbProvider.SQLServer),
                connectionString,
                commandText,
                l,
                isBeginTransaction,
                isRollbackForTest,
                argCommandType
            );
        }

        public static DataSet GetDataSet
        (
            SqlTransaction tran,
            string commandText,
            List<SqlParameter> paramsList,
            CommandType argCommandType = CommandType.StoredProcedure
        )
        {
            List<object> l = new List<object>();
            paramsList.ForEach(i => l.Add(i));

            return Util.DBHelper.GetDataSet
            (
                tran: tran,
                argCommandType: argCommandType,
                commandText: commandText,
                paramsList: l                
            );
        }

        #region [弃用] 原 DBHelper 代码

        //public static DataSet GetDataSet(
        //    string connectionString,
        //    string commandText,
        //    List<SqlParameter> paramsList,
        //    bool isRollbackForTest = false,
        //    CommandType argCommandType = CommandType.StoredProcedure)
        //{
        //    DataSet r = new DataSet();
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = null;
        //        SqlTransaction tran = null;

        //        if (isRollbackForTest == true) // 实现测试回滚
        //        {
        //            tran = conn.BeginTransaction();
        //            cmd = tran.Connection.CreateCommand();
        //            cmd.Transaction = tran;
        //        }
        //        else // 普通执行模式
        //        {
        //            cmd = conn.CreateCommand();
        //        }

        //        cmd.CommandType = argCommandType;
        //        cmd.CommandText = commandText;

        //        if (paramsList != null && paramsList.Count > 0)
        //        {
        //            cmd.Parameters.AddRange(paramsList.ToArray());
        //        }

        //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //        { 
        //            sda.Fill(r);                
        //        }

        //        // 实现测试回滚
        //        if (isRollbackForTest == true)
        //        {
        //            tran.Rollback();
        //        }
        //    }

        //    return r;
        //}

        //// 使用事务时使用
        //public static DataSet GetDataSet
        //(
        //    SqlTransaction tran,
        //    string commandText,
        //    List<SqlParameter> paramsList,
        //    CommandType argCommandType = CommandType.StoredProcedure
        //)
        //{
        //    DataSet r = new DataSet();

        //    SqlCommand cmd = tran.Connection.CreateCommand();
        //    cmd.Transaction = tran;

        //    cmd.CommandType = argCommandType;
        //    cmd.CommandText = commandText;

        //    if (paramsList != null && paramsList.Count > 0)
        //    {
        //        cmd.Parameters.AddRange(paramsList.ToArray());
        //    }

        //    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
        //    {
        //        sda.Fill(r);
        //    }

        //    return r;
        //}

        #endregion
    }
}
