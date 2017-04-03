using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.DAO
{
    public partial class itemDAO
    {
        #region 修改热点 +bool UpdateHot(string objectId)
        /// <summary>
        /// 修改热点
        /// </summary>
        /// <returns>是否成功</returns>
        public bool UpdateHot(string objectId)
        {
            const string sql = @"UPDATE [dbo].[item] SET hot=hot+1 WHERE [objectId] = @objectId";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@objectId", objectId.ToDBValue())) > 0;
        }
        #endregion
    }
}
