using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.BLL
{
    public partial class itemBLL
    {

        #region 修改热点 +bool UpdateHot(string objectId, updatedAt)
        /// <summary>
        /// 修改热点
        /// </summary>
        /// <returns>是否成功</returns>
        public bool UpdateHot(string objectId)
        {
            return _dao.UpdateHot(objectId);
        }
        #endregion
    }
}
