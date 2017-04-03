using MvcApplication1.BLL;
using MvcApplication1.Utility;
using Sheep.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MvcApplication1.Model;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace Sheep.Controllers
{
    public class HandlersController : baseApiController
    {
        private static string loginUrl = "http://www.duiba.com.cn/autoLogin/autologin";
        private static string APP_KEY = "4NKx99JCWcbJUpayXFwoz9WzDFTG";
        private static string APP_SECRET = "2JWLbAs2aR8r4LifEMCR1eEkQJCa";
        private _UserBLL userBll = new _UserBLL();
        private CreditsHistoryBLL chBll = new CreditsHistoryBLL();
        /// <summary>
        /// 生成邀请连接的二维码
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("api/duiba/creatQRCode")]
        public HttpResponseMessage creatQRCode(string v1, string id)
        {
            string url = "http://www.2d-code.cn/2dcode/api.php?key=c_4eaaXfMq1nsHRfxQXZ3/5/j2ZbgsdT7J05haWRfQwHc&url=neday.cn&text=" + id;
            return new HttpResponseMessage { Content = new StringContent(url, Encoding.GetEncoding("UTF-8"), "text/plain") };
        }

        //public string Timestamp(string v1)
        //{
        //    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //    DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
        //    TimeSpan toNow = dtNow.Subtract(dtStart);
        //    string timeStamp = toNow.Ticks.ToString();
        //    timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
        //    string url = "{\"timestamp\":\"" + timeStamp + ",\"datetime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
        //    //HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(url, System.Text.Encoding.GetEncoding("UTF-8"), "application/json") };
        //    return JsonHelper.Serialize(url);

        //}

        [HttpGet, Route("api/duiba/autoLogin")]
        public IHttpActionResult autoLogin(string uid, long credits)
        {
            _User user = userBll.QuerySingleById(uid);
            if (user == null)
                return creditError("no such user", 0);
            if (user.credit != (int)credits)
                return creditError("user credits not correct", 0);
            Hashtable hshTable = new Hashtable();
            hshTable.Add("uid", uid);
            hshTable.Add("credits", credits);
            string url = duiba.BuildUrlWithSign(loginUrl, hshTable, APP_KEY, APP_SECRET);
            return ok(url);
        }

        [HttpGet, Route("api/duiba/parseCreditConsume")]
        public IHttpActionResult parseCreditConsume(
            string uid,
            long credits,
            string appKey,
            string timestamp,
            string orderNum,
            string type,
            bool waitAudit,
            string sign,
            string description = null,
            int facePrice = 0,
            int actualPrice = 0,
            string ip = null,
            string @params = null
            )
        {
            _User user = userBll.QuerySingleById(uid);
            if (user == null)
                return creditError("no such user", 0);
            long userCredit = user.credit;
            if (!appKey.Equals(APP_KEY))
                return creditError("appKey not match", userCredit);
            
            if (timestamp == null)
                return creditError("timestamp can't be null", userCredit);
                
            Hashtable hshTable = duiba.GetUrlParams(HttpUtility.UrlDecode(Request.RequestUri.AbsoluteUri));

            string newSign;
            bool verify = duiba.SignVerify(APP_SECRET , hshTable, out newSign);

            if (!verify)
                return creditError("sign verify fail", userCredit);
            else
            {
                if (userCredit < credits)
                {
                    return creditError("credits not enough!", userCredit);
                }
                userBll.UpdateById(uid, new Dictionary<string, object> { { "credit", userCredit - credits } });

                CreditsHistory ch = new CreditsHistory();
                ch.objectId = "test"+Guid.NewGuid().ToString();
                ch.orderNum = orderNum;
                ch.createdAt = DateTime.Now;
                ch.updatedAt = DateTime.Now;
                ch.userId = uid;
                ch.type = getCreditType(type);
                ch.change = (int)-credits;
                ch.credit = userBll.QuerySingleById(uid).credit;
                ch.orderNum = orderNum;
                ch.bizId = duiba.GetMd5(orderNum);

                bool flag = chBll.Insert(ch);

                return flag ? creditOK(ch.bizId, ch.credit) : creditError("Unexpected Error, data roll back", userCredit); ;
            }

        }
        [HttpGet, Route("api/duiba/parseCreditNotify")]
        public HttpResponseMessage parseCreditNotify(
            string appKey,
            string timestamp,
            bool success,
            string errormessage,
            string orderNum,
            string bizId,
            string sign
            )
        {
            Hashtable hshTable = duiba.GetUrlParams(HttpUtility.UrlDecode(Request.RequestUri.AbsoluteUri));
            string newSign;
            if (appKey.Equals(APP_KEY) && timestamp != null && duiba.SignVerify(APP_SECRET, hshTable,out newSign))
            {
                //return creditError("appKey not match", userCredit);
                //return creditError("timestamp can't be null", userCredit);
                //return creditError("sign verify fail", userCredit);

                //CreditsHistory ch = chBll.QuerySingleByWheres(
                //    new List<Wheres> {
                //        new Wheres("objectId", "=", bizId),
                //        new Wheres("orderNum", "=", orderNum)
                //    });
                //if (ch == null)
                //{

                //}

                if (!success)
                {
                    CreditsHistory ch = chBll.QuerySingleByWheres(
                    new List<Wheres> {
                        //new Wheres("bizId", "=", bizId)
                        new Wheres("orderNum", "=", orderNum)
                    });

                    int change = ch.change;
                    int userCredit = ch.credit;
                    string userId = ch.userId;
                    userBll.UpdateById(userId, new Dictionary<string, object> { { "credit", userCredit - change } });

                    CreditsHistory ch2 = new CreditsHistory();
                    ch2.objectId = "test" + Guid.NewGuid().ToString();
                    ch2.orderNum = orderNum;
                    ch2.createdAt = DateTime.Now;
                    ch2.updatedAt = DateTime.Now;
                    ch2.userId = userId;
                    ch2.type = -2;
                    ch2.change = (int)-change;
                    ch2.credit = userBll.QuerySingleById(userId).credit;
                    ch2.orderNum = orderNum;
                    ch2.bizId = bizId;
                    chBll.Insert(ch2);
                }
            }

            return new HttpResponseMessage { Content = new StringContent("ok", Encoding.GetEncoding("UTF-8"), "text/plain") };
        }

        private CommenResult creditOK(string bizId, long credits)
        {
            return new CommenResult(
                new {
                    status = "ok",
                    errorMessage = "",
                    bizId = bizId,
                    credits = credits
                },
                Request, HttpStatusCode.OK);
        }

        private CommenResult creditError(string errorMessage, long credits)
        {
            return new CommenResult(
                new {
                    status = "fail",
                    errorMessage = errorMessage,
                    credits = credits
                }, Request, HttpStatusCode.OK);
        }


        private int getCreditType(string type)
        {
            switch(type.ToLower())
            {
                case "alipay":
                    return 7;
                case "coupon":
                    return 8;
                case "object":
                    return 9;
                case "phonebill":
                    return 10;
                case "phoneflow":
                    return 11;
                case "virtual":
                    return 12;
                case "turntable":
                    return 13;
                case "singlelottery":
                    return 14;
                case "hdtoollottery":
                    return 15;
                case "manuallottery":
                    return 16;
                case "gamelottery":
                    return 17;
                default:
                    return -2;
            }
        }
    }

}
