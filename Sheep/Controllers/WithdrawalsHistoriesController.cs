using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using Sheep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class WithdrawalsHistoriesController : baseApiController
    {
        WithdrawalsDetailsBLL bll = new WithdrawalsDetailsBLL();
        public IHttpActionResult GetWithdrawalsDetails(string v1, int limit = 10, int page = 0, string where = null, string include = null, string order = null)
        {
            try
            {
                List<Wheres> list = new List<Wheres>();
                Boolean isDesc = true;
                string orderField = "updatedAt";
                //条件
                if (!string.IsNullOrEmpty(where))
                {
                    list = JsonHelper.Deserialize<List<Wheres>>(where);
                }
                //判断页码
                if (limit == 0)
                {
                    int num = bll.QueryCount(list);
                    return ok(new { results = new string[] { }, count = num });
                }
                //排序
                if (!string.IsNullOrEmpty(order))
                {
                    string descStr = order.Substring(0, 1);
                    if (descStr.Equals("-"))
                    {
                        isDesc = true;
                    }
                    else
                    {
                        isDesc = false;
                    }

                    orderField = order.Substring(1);
                }
                //列集合
                if (string.IsNullOrEmpty(include))
                {
                    //为空时，查询所有列
                    var models = bll.QueryList(page, limit, list, orderField, isDesc);
                    return ok(new { results = models });
                }
                else
                {
                    //非空时，解析所有列
                    Dictionary<string, string[]> columns = new Dictionary<string, string[]>();
                    string includeInit = include.Substring(0, include.Count() - 1);
                    string[] cols = includeInit.Split(new string[] { "]," }, StringSplitOptions.None);
                    foreach (var col in cols)
                    {
                        string[] cols1 = col.Split('[');
                        columns.Add(cols1[0], cols1[1].Split('|'));
                    }
                    var models = bll.QueryListX(page, limit, columns, list, orderField, isDesc);
                    return ok(new { results = models });
                }
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        [ApiAuthorizationFilter]
        public IHttpActionResult PostWithdrawalsDetail(string v1, string password, [FromBody]WithdrawalsDetails withdrawalsHistory)
        {
            try {
                if (withdrawalsHistory.change <= 0)
                {
                    return notFound("必须为非空整数");
                }
                string objectId = HttpContext.Current.Request.Headers["objectId"];
                _UserBLL userModel = new _UserBLL();
                var usermodel = userModel.QuerySingleById(objectId);
                if (!usermodel.transaction_password.Equals((password + objectId).Md5()))
                {
                    return notFound("密码错误");
                }
                if (usermodel.overage < withdrawalsHistory.change)
                {
                    return notFound("余额不足");
                }
                withdrawalsHistory.createdAt = DateTime.Now;
                withdrawalsHistory.updatedAt = DateTime.Now;
                withdrawalsHistory.before = usermodel.overage;
                withdrawalsHistory.after = usermodel.overage - withdrawalsHistory.change;
                withdrawalsHistory.objectId = Guid.NewGuid().ToString();
                withdrawalsHistory.state = "未完成";
                if (bll.saveDetail(withdrawalsHistory, objectId))
                {
                    return ok(withdrawalsHistory.objectId);
                }
                return notFound("失败");
            }
            catch(Exception e){
                return execept(e.Message);
            }
           
        }
    }
}
