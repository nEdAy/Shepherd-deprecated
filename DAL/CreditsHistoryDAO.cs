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
    public partial class CreditsHistoryDAO
    {
        #region 向数据库中添加一条记录 +bool SignIn(CreditsHistory model,string userId)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool SignIn(CreditsHistory model,string userId)
        {

            const string sql = @"INSERT INTO [dbo].[CreditsHistory] (objectId,createdAt,updatedAt,userId,type,change,credit) VALUES (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@userId", model.userId.ToDBValue()), new SqlParameter("@type", model.type.ToDBValue()), new SqlParameter("@change", model.change.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()) };

            const string sql1 = @"UPDATE [dbo].[_User] SET sign_in='False',credit=credit+@number WHERE [objectId] = @objectId";
            SqlParameter[] pams1 = { new SqlParameter("@objectId", userId), new SqlParameter("@number", model.change) };
            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, pams1);
            return res > 0;
        }

        public bool InsertWithOrderInfo(CreditsHistory model)
        {
            const string sql = @"INSERT INTO [dbo].[CreditsHistory] (objectId,createdAt,updatedAt,userId,type,change,credit,orderNum) VALUES (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit,@orderNum,@bizId)";
            int res = SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@userId", model.userId.ToDBValue()), new SqlParameter("@type", model.type.ToDBValue()), new SqlParameter("@change", model.change.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()), new SqlParameter("@orderNum", model.orderNum.ToDBValue()), new SqlParameter("@bizId", model.bizId.ToDBValue()));
            return res > 0;
        }
        #endregion
        #region 向数据库中添加一条记录 +bool shake(CreditsHistory model,string userId)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool shake(CreditsHistory model, string userId)
        {

            const string sql = @"INSERT INTO [dbo].[CreditsHistory] (objectId,createdAt,updatedAt,userId,type,change,credit) VALUES (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@userId", model.userId.ToDBValue()), new SqlParameter("@type", model.type.ToDBValue()), new SqlParameter("@change", model.change.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()) };

            const string sql1 = @"UPDATE [dbo].[_User] SET shake_times=shake_times-1,credit=credit+@number WHERE [objectId] = @objectId";
            SqlParameter[] pams1 = { new SqlParameter("@objectId", userId), new SqlParameter("@number", model.change) };
            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, pams1);
            return res > 0;
        }
        #endregion
        #region 向数据库中添加一条记录 +bool shakeTime(string userId)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool shakeTime(string userId)
        {
            const string sql = @"UPDATE [dbo].[_User] SET shake_times=shake_times-1 WHERE [objectId] = @objectId";
            SqlParameter[] pams = { new SqlParameter("@objectId", userId) };
            int res = SqlHelper.ExecuteNonQuery( sql, pams);
            return res > 0;
        }
        #endregion
    }
}
