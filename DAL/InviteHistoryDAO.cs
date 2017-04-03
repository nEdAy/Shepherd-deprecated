using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.DAO
{
    public class InviteHistoryDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">注册用户</param>
        /// <param name="inUsername">邀请用户</param>
        /// <returns></returns>
        public bool Insert(String username,String inUsername)
        {
            const string sql = @"INSERT INTO InviteHistory(username,inUsername,createdAt) values (@username,@inUsername,@createdAt)";
            SqlParameter[] parms = { new SqlParameter("@username", username), new SqlParameter("@inUsername", inUsername), new SqlParameter("createdAt", DateTime.Now.ToString()) };
            int res = SqlHelper.ExecuteNonQuery(sql, parms);
            return res > 0;
        }
        
        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="wheres">查询条件</param>
        /// <returns>条数</returns>
        public int QueryCount(object wheres)
        {
            List<SqlParameter> list = null;
            string str = wheres.parseWheres(out list);
            str = str == "" ? str : "where " + str;
            string sql = "SELECT COUNT(1) from InviteHistory " + str; var res = SqlHelper.ExecuteScalar(sql, list.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
    }
}
