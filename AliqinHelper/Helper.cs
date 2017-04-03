using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace AliqinHelper //阿里大鱼
{
    public class Helper
    {
        private DefaultTopClient topClient;
        public Helper()
        {
            topClient = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23450980", "2d4e218dc844eeafee7a055e2bb9b323", "json");
        }
        public string getMessage(string extend, string sms_free_sign_name, string rec_num, string sms_template_code)
        {
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = extend;
            req.SmsType = "normal";
            req.SmsFreeSignName = sms_free_sign_name;
            req.RecNum = rec_num;
            req.SmsTemplateCode = sms_template_code;
            AlibabaAliqinFcSmsNumSendResponse response = topClient.Execute(req);
            return response.Body;
        }
    }
}
