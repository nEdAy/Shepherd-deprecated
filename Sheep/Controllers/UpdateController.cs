using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApplication1.BLL;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class UpdateController : baseApiController
    {
        _UserBLL bll = new _UserBLL();

        public IHttpActionResult GetUpdateAllUser(string v1)
        {
            try
            {
                bool result = bll.UpdateByWheres(null, new Dictionary<String, object> { { "sign_in", true }, { "shake_times", 3 } });
                DateTime dt = DateTime.Now;
                if (result)
                {
                    return create(new { updateAt = dt.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    return invildRequest("失败");
                }
            }
            catch(Exception e)
            {
                return error(e.Message);
            }
        }
    }
}