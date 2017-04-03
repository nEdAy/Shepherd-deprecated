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
    public class MessagesController : baseApiController
    {
        MessageBLL bll = new MessageBLL();
        public IHttpActionResult GetMessages(string v1, int limit = 10, int page = 0, string where = null, string include = null, string order = null)
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
        public IHttpActionResult PostMessage(string v1, [FromBody]Message model)
        {
            try
            {
                //{"_User":{"objectId":"556fb922-de5c-4086-a489-9f09799a5456"},"isReaded":false,"msg":"还看呢","toldId":"556fb922-de5c-4086-a489-9f09799a5456"}
                //表单验证
                if (isNUll(model._User.objectId, model.msg, model.toldId))
                {
                    return invildRequest("参数不能为空");
                }
                //主键
                Guid guid = Guid.NewGuid();
                model.objectId = guid.ToString();
                //时间
                model.createdAt = DateTime.Now;
                model.updatedAt = DateTime.Now;

                bool result = bll.Insert(model);
                if (result)
                {
                    //return ok(model.updatedAt.ToString("yyyy-mm-dd HH:mm:ss"));

                    DateTime start = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    DateTime now = DateTime.Now;
                    TimeSpan toNow = now.Subtract(start);
                    string timeStamp = toNow.Ticks.ToString();
                    timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);

                    string type = "push/single_account";
                    string message_type = "1";
                    var body = new
                    {
                        content = model.msg,
                        title = "消息",
                        vibrate = 1
                    };
                    //string objectId = HttpContext.Current.Request.Headers["objectId"];
                    //string objectId = "844b0bf2-d4a6-418b-a327-cb7d4f72a0fa";
                    string url = "access_id=2100180778&account=" + model.toldId + "&message=" + JsonHelper.Serialize(body) + "&message_type=" + message_type + "&timestamp=" + timeStamp;
                    string encode = string.Format("POSTopenapi.xg.qq.com/v2/{0}{1}0537318f3b4384e6c74e8396ef9122a4", type, url.Replace("&", ""));
                    string ss = url + "&sign=" + encode.Md5();
                    MvcApplication1.Utility.HttpClient p = new MvcApplication1.Utility.HttpClient("http://openapi.xg.qq.com/v2");
                    return create(p.Post(ss, type));


                }
                return notFound("注册失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
    }
}
