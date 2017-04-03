using MvcApplication1.Model;
using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.DAO
{
    public partial class WithdrawalsDetailsDAO
    {
        #region 向数据库中添加一条记录 +bool saveDetail(WithdrawalsDetails)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool saveDetail(WithdrawalsDetails model, string userId)
        {
            const string sql = @"INSERT INTO [dbo].[WithdrawalsDetails] (objectId,createdAt,updatedAt,userId,type,number,before,after,change,state) VALUES (@objectId,@createdAt,@updatedAt,@userId,@type,@number,@before,@after,@change,@state)";
            SqlParameter[] pams =new SqlParameter[]{new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@userId", model.userId.ToDBValue()), new SqlParameter("@type", model.type.ToDBValue()), new SqlParameter("@number", model.number.ToDBValue()), new SqlParameter("@before", model.before.ToDBValue()), new SqlParameter("@after", model.after.ToDBValue()), new SqlParameter("@change", model.change.ToDBValue()), new SqlParameter("@state", model.state.ToDBValue())};


            const string sql1 = @"UPDATE [dbo].[_User] SET overage=overage-@change WHERE [objectId] = @objectId";
            SqlParameter[] pams1 = { new SqlParameter("@objectId", userId), new SqlParameter("@change", model.change) };
            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, pams, sql1, pams1);
            return res > 0;
        }
        #endregion
    }
}
