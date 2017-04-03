using AliqinHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class SmsController : baseApiController
    {
        Helper topClient = new Helper();

        public IHttpActionResult PostMassMessage(string v1, string extend, string sms_free_sign_name, string rec_num, string sms_template_code)
        {
            string result = topClient.getMessage(extend, sms_free_sign_name, rec_num, sms_template_code);
            return ok(result);
        }
    }
}