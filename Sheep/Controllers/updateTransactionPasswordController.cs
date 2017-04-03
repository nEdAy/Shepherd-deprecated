using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Models;
using MvcApplication1.Utility;
using Sheep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Sheep.Controllers
{
    public class transactionPasswordController : baseApiController
    {
        //数据访问层
        _UserBLL bll = new _UserBLL();
        //
        // GET: /api/v1/transactionPassword
        [ApiAuthorizationFilter]
        public IHttpActionResult GetTransactionPassword(string v1)
        {
            try
            {
                string objectId = HttpContext.Current.Request.Headers["objectId"];
                //查询交易密码
                List<Wheres> whs = new List<Wheres>() { new Wheres("objectId", "=", objectId) };
                var dir = bll.QuerySingleByWheres(whs);
                string transaction_password = dir.transaction_password;
                if (transaction_password.Equals(objectId.Md5()))
                {
                    return ok("0"); 
                }
                else
                    return ok("1"); 
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        [ApiAuthorizationFilter]
        public IHttpActionResult PutTransactionPassword(string v1,String oldPassword, String newPassword)
        {
            try
            {
                string objectId = HttpContext.Current.Request.Headers["objectId"];
                if (isNUll(oldPassword, newPassword))
                {
                    return invildRequest("参数不能为空");
                }

                List<Wheres> whs = new List<Wheres>() { new Wheres("objectId", "=", objectId) };
                var dir = bll.QuerySingleByWheres(whs);
                string transaction_password = dir.transaction_password;


                if (!transaction_password.Equals((transaction_password + objectId).Md5()))
                {
                    return notFound("旧密码错误");
                }

                DateTime dt = DateTime.Now;
                bool result = bll.UpdateById(objectId, new Dictionary<string, object> { { "transaction_password", (newPassword + objectId).Md5() }, { "updatedAt", dt } });
                if (result)
                {
                    create(new { updateAt = dt.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                return notFound("失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }       
           
        }
       
    }
}
