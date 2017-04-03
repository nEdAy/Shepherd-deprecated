using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.BLL
{
    public partial class CreditsHistoryBLL
    {
        #region 向数据库中添加一条记录 +bool SignIn(CreditsHistory model,string userId)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool SignIn(CreditsHistory model, string userId)
        {

            return _dao.SignIn(model, userId);
        }
        #endregion
        #region 向数据库中添加一条记录 +bool shake(CreditsHistory model,string userId)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool shake(CreditsHistory model, string userId)
        {

            return _dao.shake(model, userId);
        }
        #endregion
        #region 向数据库中添加一条记录 +bool shakeTime(string userId)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool shakeTime(string userId)
        {
            return _dao.shakeTime(userId);
        }
        #endregion
    }
}
