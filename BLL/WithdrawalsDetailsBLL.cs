using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.BLL
{
    public partial class WithdrawalsDetailsBLL
    {
        #region 向数据库中添加一条记录 +bool saveDetail(WithdrawalsDetails)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool saveDetail(WithdrawalsDetails model, string userId)
        {
            return _dao.saveDetail(model, userId);
        }
        #endregion
    }
}
