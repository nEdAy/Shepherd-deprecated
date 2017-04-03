using MvcApplication1.BLL;
using MvcApplication1.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class MobSMSController : baseApiController
    {
        public IHttpActionResult GetMassMessage(string v1,string phone)
        {
            //短信请求前，检查用户是否已经绑定
            _UserBLL bll = new _UserBLL();
            if (isNUll(phone))
            {
                return notFound("手机号不能为空");
            }       
            if (bll.QueryBandByUsername(phone))
            {
                return notFound("手机号已绑定过");
            }

            MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
            string postUri = "sms/sendmsg?appkey=1077112ae0d07&phone=" + phone + "&zone=86" ;
            string postResponse = client.Get(postUri);
            if (!string.IsNullOrEmpty(postResponse))
            {
                JObject jo = JsonHelper.DeserializeObject(postResponse);
                string status = jo["status"].ToString();
                if (status.Equals("200"))
                {
                    return ok("请求验证码成功");
                }
                else
                {
                    return notFound("请求验证码失败" + postResponse);
                }
            }
            else
            {
                return notFound("验证码请求失败");
            }
        }
    }
}
