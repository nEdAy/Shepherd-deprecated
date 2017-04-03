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
    public partial class CommentDAO
    {
       
        #region 向数据库中添加一条记录 +bool reply(Comment model)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool reply(Comment model, bool isReply)
        {
            const string sql = @"INSERT INTO [dbo].[Comment] (objectId,parentId,contents,replyCount,_UserId,updatedAt,createdAt) VALUES (@objectId,@parentId,@contents,@replyCount,@_UserId,@updatedAt,@createdAt)";
            SqlParameter[] pams = { new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@parentId", model.parentId.ToDBValue()), new SqlParameter("@contents", model.contents.ToDBValue()), new SqlParameter("@replyCount", model.replyCount.ToDBValue()), new SqlParameter("@_UserId", model._User.objectId.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()) };

            if (isReply)
            {
                const string sql1 = @"UPDATE [dbo].[Comment] SET commentCount=commentCount+1 WHERE [objectId] = @objectId";
                SqlParameter[] pams1 = { new SqlParameter("@objectId", model.parentId.ToDBValue()) };

                return SqlHelper.ExecuteNonQuerysTransaction(sql, pams, sql1, pams1) > 0;
            }
            else {
                const string sql1 = @"UPDATE [dbo].[item] SET commentCount=commentCount+1 WHERE [objectId] = @objectId";
                SqlParameter[] pams1 = { new SqlParameter("@objectId", model.parentId.ToDBValue()) };

                return SqlHelper.ExecuteNonQuerysTransaction(sql, pams, sql1, pams1) > 0;
            }

           
        }
        #endregion
    }
}
