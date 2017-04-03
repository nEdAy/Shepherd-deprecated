using MvcApplication1.BLL;
using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class shakeController : baseApiController
    {
        _UserBLL bll = new _UserBLL();
        CreditsHistoryBLL historyBLL = new CreditsHistoryBLL();
        public IHttpActionResult Get(string v1, string objectId)
        {
            try
            {
                if (string.IsNullOrEmpty(objectId))
                {
                    return invildRequest("用户ID不能为空");
                }


                _User user = bll.QuerySingleById(objectId);
                if (user.shake_times < 1) {
                    return ok(-1);
                }

                Random ran = new Random();
                int number=ran.Next(-3, 6);
                if (number < 1)
                {
                    return ok(number);
                }
                CreditsHistory history = new CreditsHistory();
                history.change = number;
                history.type = -1;
                Guid guid = Guid.NewGuid();
                history.objectId = guid.ToString();
                history.updatedAt = DateTime.Now;
                history.createdAt = DateTime.Now;
                history.userId = objectId;
                history.credit = user.credit + history.change;

                if (historyBLL.shake(history, objectId))
                {
                    return ok(number);
                }
                else
                {
                    return notFound("发生错误");
                }

            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
    }
}
