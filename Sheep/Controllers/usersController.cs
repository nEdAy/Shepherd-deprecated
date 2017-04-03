using Model;
using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Models;
using MvcApplication1.Utility;
using Newtonsoft.Json.Linq;
using Sheep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Sheep.Controllers
{
    public class usersController : baseApiController
    {

        _UserBLL bll = new _UserBLL();
        wechatBLL wechat_bll = new wechatBLL();
        authDataBLL auth_bll = new authDataBLL();
        InviteHistoryBLL invite_bll = new InviteHistoryBLL();
        
        public IHttpActionResult GetIsExit(string v1,bool isNew=false, string where = null)
        {
            try
            {
                if (isNew)
                {
                    List<Wheres> list = new List<Wheres>();
                    //条件
                    if (string.IsNullOrEmpty(where))
                    {
                        return notFound("请求失败");
                    }
                    list = JsonHelper.Deserialize<List<Wheres>>(where);
                    _User user = new _User();
                    if (where.Contains("openId"))
                    {
                        var m = wechat_bll.QuerySingleByWheres(list);
                        if (m != null)
                        {
                            List<Wheres> wheres = new List<Wheres>() { new Wheres("wechatId", "=", m.objectId) };
                            var n = auth_bll.QuerySingleByWheres(wheres);
                            if (n != null)
                            {

                                List<Wheres> whes = new List<Wheres>() { new Wheres("authDataId", "=", n.objectId) };
                                user = bll.QuerySingleByWheres(whes);
                            }
                        }
                    }
                    else
                    {
                        user = bll.QuerySingleByWheres(list);
                    }
                    if (user != null)
                    {
                        if (user.authData == null)
                        {
                            return ok(new { objectId = user.objectId, credit = user.credit });
                        }
                        else
                        {
                            return ok(new { objectId = user.objectId, openId = user.authData.wechat.openId, inopenId = user.authData.wechat.inopenId, credit = user.credit,username=user.username, sign_in = user.sign_in});
                        }
                    }
                    else
                    {
                        return notFound("用户不存在");
                    }
                }
                else
                {
                    List<Wheres> list = new List<Wheres>();
                    //条件
                    if (!string.IsNullOrEmpty(where))
                    {
                        list = JsonHelper.Deserialize<List<Wheres>>(where);
                    }
                    int count = bll.QueryCount(list);
                    return ok(new { count = count });
                }
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }             
        }

        //public async Task<HttpResponseMessage> Get()
        //{
        //    return await Task<HttpResponseMessage>.Factory.StartNew(() =>
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, "aa");
        //    });
        ////}
        //public IHttpActionResult GetUsers(string v1, string include = "")
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(include))
        //        {
        //            IEnumerable<_User> list = bll.QueryList(0);
        //            //return result<IEnumerable<_User>>(list);
        //            return ok(list);
        //        }
        //        else
        //        {
        //            //非空时，解析所有列
        //            Dictionary<string, string[]> columns = new Dictionary<string, string[]>();
        //            string includeInit = include.Substring(0, include.Count() - 1);
        //            string[] cols = includeInit.Split(new string[] { "]," }, StringSplitOptions.None);
        //            foreach (var col in cols)
        //            {
        //                string[] cols1 = col.Split('[');
        //                columns.Add(cols1[0], cols1[1].Split('|'));
        //            }

        //            IEnumerable<Dictionary<string, object>> list = bll.QueryListX(0, 10, columns);
        //            //return result<IEnumerable<Dictionary<string, object>>>(list);
        //            return ok(list);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return execept(e.Message);
        //    }
        //}
        // GET api/values/5  获取指定id信息
        public IHttpActionResult GetUser(string v1, string objectId, string include = "")
        {
            try
            {

            
                if (string.IsNullOrEmpty(objectId))
                {
                    return invildRequest("用户ID不能为空");
                }
               
               
                if (string.IsNullOrEmpty(include))
                {
                    _User model = bll.QuerySingleById(objectId);
                    return ok(model);
                }
                else
                {
                    Dictionary<string, string[]> columns = new Dictionary<string, string[]>();
                    string includeInit = include.Substring(0, include.Count() - 1);

                    string[] cols = includeInit.Split(new string[] { "]," }, StringSplitOptions.None);
                    foreach (var col in cols)
                    {
                        string[] cols1 = col.Split('[');
                        columns.Add(cols1[0], cols1[1].Split(','));
                    }
                    Dictionary<string, object> model = bll.QuerySingleByIdX(objectId, columns);
                    //IEnumerable<Dictionary<string, object>> model = bll.QueryListX(0, 1, columns, new List<Wheres> { new Wheres("objectId", "=", objectId) });
                    
                    //if (model == null||model.Count()<1) {
                    //    return notFound("查询失败");
                    //}
                    //return ok(model.First());
                    if (model == null)
                    {
                        return notFound("查询失败");
                    }
                    return ok(model);
                }
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }       
            
        }
        public IHttpActionResult GetAuthorization(string v1,string username, string password)
        {
            try
            {
                //表单验证
                if (isNUll(username, password))
                {
                    return invildRequest("参数不能为空");
                }

                List<Wheres> whs = new List<Wheres>() { new Wheres("username", "=", username) };
                var dir = bll.QuerySingleByWheres(whs);
                if (dir!=null)
                {
                    string obj = (string)(dir.objectId);
                    string pas = (string)(dir.password);
                    //string li = "raw:" + password + "  sql:" + pas + "  jiami:" + (password + obj).Md5();
                    if ((password + obj).Md5().Equals(pas))
                    {
                        string sessionToken = Guid.NewGuid().ToString();
                        bll.UpdateById(obj, new Dictionary<string, object> { { "sessionToken", sessionToken } });

                        _User model = bll.QuerySingleById(obj);
                        return ok(model); 
                    }
                    else
                    {
                        return notFound("密码错误");
                    }
                }
                else
                {
                    return notFound("用户不存在");
                }

            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        // PUT api/values/5  修改指定列
        [ApiAuthorizationFilter]
        public IHttpActionResult PutUser(string v1,[FromBody] Dictionary<string, object> column)
        {
            try
            {
                string objectId = HttpContext.Current.Request.Headers["objectId"];
                //更新时间
                DateTime dt = DateTime.Now;
                if (column!=null)
                {
                    column.Add("updatedAt", dt);
                }
                else {
                    return invildRequest("修改参数不能为空");
                }

               
                bool result = bll.UpdateById(objectId, column);              
                if (result)
                {
                    return create(new { updateAt = dt.ToString("yyyy-MM-dd HH:mm:ss") });
                }

                return notFound("修改失败");
               
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }
        public IHttpActionResult PutPassword(string v1,string username, string password, string code)
        {
            try
            {
                if (isNUll(username, password, code))
                {
                    return invildRequest("参数不能为空");
                }
                //objectId
                var dir = bll.QuerySingleByWheres(new List<Wheres> { new Wheres("username", "=", username) });
                if (dir == null)
                {
                    return notFound("用户不存在");
                }
                string objectId =dir.objectId;
                //验证手机验证码
                MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
                string postUri = "sms/verify?appkey=1ad08332b2ac0&phone=" + username + "&zone=86&code=" + code;

                //string userJson = @"{""appkey"":""1ad08332b2ac0"",""phone"":" + username + @",""zone"":""86"",""code"":" + code + "}";
                //请求验证
                string postResponse = client.Get(postUri);
                if (!string.IsNullOrEmpty(postResponse))
                {
                    JObject jo = JsonHelper.DeserializeObject(postResponse);
                    string status = jo["status"].ToString();
                    if (!status.Equals("200"))
                    {
                        return notFound("验证码错误" + postResponse);
                    }
                }
                else {
                    return notFound("验证码请求验证失败");
                }
                //更新时间
                DateTime dt = DateTime.Now;
               
                
                //更新
                bool result = bll.UpdateByWheres(new Wheres("username","=",username), new Dictionary<string, object> { { "password", (password + objectId).Md5() }, { "updatedAt", dt } });

                if (result)
                {
                    return ok(new { updateAt = dt.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                return notFound("修改失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }
        [ApiAuthorizationFilter]
        public IHttpActionResult PutPassword(string v1,String oldPassword, String newPassword)
        {
            try
            {
                string objectId = HttpContext.Current.Request.Headers["objectId"];
                //查询密码
                List<Wheres> whs = new List<Wheres>() { new Wheres("objectId", "=", objectId) };
                var dir = bll.QuerySingleByWheres(whs);
                string password = dir.password.ToString();
                if (!password.Equals((oldPassword + objectId).Md5()))
                {
                    return notFound("旧密码错误");
                }
                DateTime dt = DateTime.Now;
                bool result = bll.UpdateById(objectId, new Dictionary<string, object> { { "password", (newPassword + objectId).Md5() }, { "updatedAt", dt } });
                if (result)
                {
                    return create(new { updateAt = dt.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    return notFound("失败");
                }
            }
            catch (Exception e)
            {
                return execept(e.Message); 
            }

        }

        /// <summary>
        /// 微信端注册与绑定接口
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="model"></param>       
        /// <param name="code"></param>
        /// <param name="wechat"></param>
        /// <returns></returns>
        public IHttpActionResult Post(string v1, [FromBody]_User model,string code,bool wechat)
        {
            try
            {
                //表单验证
                if (isNUll(model.username, model.password, code))
                {
                    return invildRequest("参数不能为空");
                }
                //注册流程
                wechat chat = new wechat();
                //主键
                Guid guid = Guid.NewGuid();
                //判断是否有openId
                if (model.authData == null || model.authData.wechat == null || isNUll(model.authData.wechat.openId))
                {
                    return invildRequest("参数有误");
                }

                //微信端短信验证
                MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
                string postUri = "sms/checkcode?appkey=1077112ae0d07&phone=" + model.username + "&zone=86&code=" + code;

                //string userJson = @"{""appkey"":""1ad08332b2ac0"",""phone"":" + model.username + @",""zone"":""86"",""code"":" + code + "}";
                //请求验证
                string postResponse = client.Get(postUri);
                if (!string.IsNullOrEmpty(postResponse))
                {
                    JObject jo = JsonHelper.DeserializeObject(postResponse);
                    string status = jo["status"].ToString();
                    if (!status.Equals("200"))
                    {
                        return notFound("验证码错误");
                    }
                }
                else
                {
                    return notFound("验证码请求验证失败");
                }

                //判断微信号是否绑定过
                if (wechat_bll.QueryExitByOpenId(model.authData.wechat.openId))
                {
                    return notFound("此微信号已经绑定过了哦！");
                }
                //注册与绑定逻辑
                model.authData.objectId = guid.ToString();
                model.authData.wechat.objectId = guid.ToString();

                //判断用户是否存在
                if (bll.QueryExitByUsername(model.username))
                {
                    //用户已存在
                    //微信绑定操作
                    //更新openId和inopenId                                           
                    //邀请码选填
                    if (isNUll(model.authData.wechat.inopenId))
                    {
                        //邀请码为空，绑定
                        if (bll.UpdateInsert1(model))
                        {
                            return ok(new { msg = "绑定成功" });
                        }
                        return notFound("绑定失败");
                    }
                    //邀请码不为空                        
                    if (!bll.QueryExitByUsername(model.authData.wechat.inopenId))
                    {
                        //inopenId无效
                        return notFound("您的邀请用户手机号无效！");
                    }

                    //判断用户是否在APP端被邀请过
                    if (invite_bll.QueryExitByUsername(model.username))
                    {
                        //用户在APP端被邀请过，邀请用户不再获得积分，只进行微信信息绑定，双方均不得积分。
                        if (bll.UpdateInsert1(model))
                        {
                            return ok(new { msg = "绑定成功" });
                        }
                        return notFound("绑定失败");
                    }

                    //绑定只给邀请人积分，不给被邀请人积分                                        
                    //邀请者记录
                    //条件
                    List<Wheres> whs = new List<Wheres>() { new Wheres("username", "=", model.authData.wechat.inopenId) };
                    _User user = bll.QuerySingleByWheres(whs);
                    CreditsHistory history = new CreditsHistory();
                    history.objectId = guid.ToString();
                    history.createdAt = DateTime.Now;
                    history.updatedAt = DateTime.Now;
                    history.change = 30;
                    history.credit = user.credit + 30;
                    history.userId = user.objectId;
                    //微信邀请好友
                    history.type = 3;
                    if (bll.UpdateInsert(model, history))
                    {
                        return ok(new { msg = "绑定成功" });
                    }
                    else
                    {
                        return notFound("绑定失败");
                    }

                }
                //用户不存在
                //微信注册操作                    

                DateTime dt = DateTime.Now;
                model.objectId = guid.ToString();
                //密码加盐保存
                model.password = (model.password + model.objectId).Md5();
                //初始化数据
                model.nickname = "口袋爆料人";
                model.credit = 40;
                model.overage = 0;
                model.sign_in = true;
                model.shake_times = 3;
                model.createdAt = dt;
                model.updatedAt = dt;
                string initPassword = "123456";
                model.transaction_password = (initPassword.Md5() + model.objectId).Md5();

                CreditsHistory history2 = new CreditsHistory();
                history2.objectId = guid.ToString();
                history2.createdAt = dt;
                history2.updatedAt = dt;
                history2.change = 40;
                history2.credit = 40;
                history2.type = 4;//注册得积分
                bool result = false;
                if (isNUll(model.authData.wechat.inopenId))
                {
                    //没有邀请人
                    result = bll.Insert(model, history2);
                }
                else
                {
                    //有邀请人
                    //条件
                    List<Wheres> whs = new List<Wheres>() { new Wheres("username", "=", model.authData.wechat.inopenId) };
                    _User user = bll.QuerySingleByWheres(whs);
                    CreditsHistory history1 = new CreditsHistory();
                    Guid guid1 = Guid.NewGuid();
                    history1.objectId = guid1.ToString();
                    history1.createdAt = dt;
                    history1.updatedAt = dt;
                    history1.type = 3;//邀请得积分
                    history1.change = 30;
                    history1.credit = user.credit + 30;
                    history1.userId = user.objectId;
                    result = bll.Insert(model, history2, history1);
                }
                if (result)
                {
                    return ok(new { msg = "注册成功" });
                }
                return notFound("注册失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }     
        }

        /// <summary>
        /// 手机端注册邀请接口
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <param name="inviteCode">邀请人手机号码</param>
        /// <returns></returns>
        public IHttpActionResult Post(string v1, [FromBody]_User model, string code, string inviteCode = "")
        {
            bool isInvited = false;
            try
            {
                //表单验证
                if (isNUll(model.username, model.password, code))
                {
                    return invildRequest("参数不能为空");
                }

                //判断是否有邀请码
                if (!string.IsNullOrEmpty(inviteCode))
                {
                    //邀请人手机号码是否存在                    
                    if (!bll.QueryExitByUsername(inviteCode))
                    {
                        return notFound("邀请人手机号码不存在哦！");
                    }
                    isInvited = true;
                }

                //手机端短信验证
                string postUri = "sms/verify?appkey=1ad08332b2ac0&phone=" + model.username + "&zone=86&code=" + code;
                //短信验证
                MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
                //请求验证
                string postResponse = client.Get(postUri);
                if (!string.IsNullOrEmpty(postResponse))
                {
                    JObject jo = JsonHelper.DeserializeObject(postResponse);
                    string status = jo["status"].ToString();
                    if (!status.Equals("200"))
                    {
                        return notFound("验证码错误" + postResponse);
                    }
                }
                else
                {
                    return notFound("验证码请求验证失败");
                }

                //查询用户是否已经存在
                if (bll.QueryExitByUsername(model.username))
                {
                    return notFound("用户名已存在");
                }
                bool result = false;
                //主键
                Guid guid = Guid.NewGuid();
                DateTime dt = DateTime.Now;
                model.objectId = guid.ToString();
                //密码加盐保存
                model.password = (model.password + model.objectId).Md5();
                //初始化数据
                model.nickname = "口袋爆料人";
                model.credit = 40;
                model.overage = 0;
                model.sign_in = true;
                model.shake_times = 3;
                model.createdAt = dt;
                model.updatedAt = dt;
                string initPassword = "123456";
                model.transaction_password = (initPassword.Md5() + model.objectId).Md5();
                //注册积分记录
                CreditsHistory history1 = new CreditsHistory();
                history1.objectId = guid.ToString();
                history1.createdAt = dt;
                history1.updatedAt = dt;
                history1.change = 40;
                history1.credit = 40;
                history1.type = 4;//注册得积分
                result = bll.Insert1(model, history1);
                if (isInvited)
                {
                    List<Wheres> whs = new List<Wheres>() { new Wheres("username", "=", inviteCode) };
                    _User user = bll.QuerySingleByWheres(whs);
                    //邀请积分记录
                    CreditsHistory history2 = new CreditsHistory();
                    Guid guid1 = Guid.NewGuid();
                    history2.objectId = guid1.ToString();
                    history2.createdAt = dt;
                    history2.updatedAt = dt;
                    history2.type = 3;//邀请得积分
                    history2.change = 30;
                    history2.credit = user.credit + 30;
                    history2.userId = user.objectId;
                    result = bll.Insert1(model, history1, history2, inviteCode);
                }
                if (result)
                {
                    return ok(new { msg = "注册成功" });
                }
                return notFound("注册失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }


        // POST api/values  添加用户
        /// <summary>
        /// 手机端注册邀请接口
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="model"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        //public IHttpActionResult Post(string v1, [FromBody]_User model, string code)
        //{
        //    是否是手机端注册
        //    bool isPhone = false;
        //    try
        //    {
        //        表单验证
        //        if (isNUll(model.username, model.password, code))
        //        {
        //            return invildRequest("参数不能为空");
        //        }



        //        判断是否有openId
        //        if (model.authData == null || model.authData.wechat == null || isNUll(model.authData.wechat.openId))
        //        {
        //            无openId,手机端注册操作
        //            手机端短信验证


        //            string postUri = "sms/verify?appkey=1ad08332b2ac0&phone=" + model.username + "&zone=86&code=" + code;

        //            string userJson = @"{""appkey"":""1ad08332b2ac0"",""phone"":" + model.username + @",""zone"":""86"",""code"":" + code + "}";
        //            短信验证
        //            MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
        //            请求验证
        //            string postResponse = client.Get(postUri);
        //            if (!string.IsNullOrEmpty(postResponse))
        //            {
        //                JObject jo = JsonHelper.DeserializeObject(postResponse);
        //                string status = jo["status"].ToString();
        //                if (!status.Equals("200"))
        //                {
        //                    return notFound("验证码错误" + postResponse);
        //                }
        //            }
        //            else
        //            {
        //                return notFound("验证码请求验证失败");
        //            }

        //            条件
        //            List<Wheres> list = new List<Wheres>();
        //            Wheres wh = new Wheres();
        //            wh.setField("username", "=", model.username, "");
        //            list.Add(wh);
        //            查询用户是否已经存在
        //            int num = bll.QueryCount(list);
        //            if (num > 0)
        //            {
        //                return notFound("用户名已存在");
        //            }
        //            isPhone = true;
        //        }
        //        else
        //        {
        //            有openId,微信端
        //            查询openId是否已经存在
        //            注册操作
        //            用户名已存在，则只更新记录

        //            微信端短信验证
        //            MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
        //            string postUri = "sms/checkcode?appkey=1077112ae0d07&phone=" + model.username + "&zone=86&code=" + code;

        //            string userJson = @"{""appkey"":""1ad08332b2ac0"",""phone"":" + model.username + @",""zone"":""86"",""code"":" + code + "}";
        //            请求验证
        //            string postResponse = client.Get(postUri);
        //            if (!string.IsNullOrEmpty(postResponse))
        //            {
        //                JObject jo = JsonHelper.DeserializeObject(postResponse);
        //                string status = jo["status"].ToString();
        //                if (!status.Equals("200"))
        //                {
        //                    return notFound("验证码错误");
        //                }
        //            }
        //            else
        //            {
        //                return notFound("验证码请求验证失败");
        //            }


        //            if (bll.QueryExitByUsername(model.username) && wechat_bll.QueryExitByOpenId(model.authData.wechat.openId))
        //            {
        //                用户openId和username都已存在
        //                return notFound("用户已绑定过！");
        //            }
        //            else if (bll.QueryExitByUsername(model.username) && !wechat_bll.QueryExitByOpenId(model.authData.wechat.openId))
        //            {
        //                用户存在，openId不存在。微信绑定操作
        //                更新openId和inopenId

        //                model.authData.objectId = guid.ToString();
        //                model.authData.wechat.objectId = guid.ToString();

        //                邀请码选填
        //                if (isNUll(model.authData.wechat.inopenId))
        //                {
        //                    邀请码为空
        //                    if (bll.UpdateInsert1(model))
        //                    {
        //                        return ok(new { msg = "绑定成功" });
        //                    }
        //                    return notFound("绑定失败");
        //                }
        //                邀请码不为空
        //                if (!bll.QueryExitByUsername(model.authData.wechat.inopenId))
        //                {
        //                    inopenId无效
        //                    return notFound("您的邀请用户手机号无效！");
        //                }
        //                查询绑定记录表是否有注册用户和邀请人的组合。

        //                绑定只给邀请人积分，不给被邀请人积分
        //                邀请者记录
        //                条件
        //                List<Wheres> whs = new List<Wheres>() { new Wheres("username", "=", model.authData.wechat.inopenId) };
        //                _User user1 = bll.QuerySingleByWheres(whs);
        //                CreditsHistory history = new CreditsHistory();
        //                history.objectId = guid.ToString();
        //                history.createdAt = DateTime.Now;
        //                history.updatedAt = DateTime.Now;
        //                history.credit = user1.credit + 40;
        //                history.userId = user1.objectId;
        //                微信邀请好友
        //                history.type = 3;
        //                if (bll.UpdateInsert(model, history, 40))
        //                {
        //                    return ok(new { msg = "绑定成功" });
        //                }
        //                else
        //                {
        //                    return notFound("绑定失败");
        //                }
        //            }
        //            else if (!bll.QueryExitByUsername(model.username) && !wechat_bll.QueryExitByOpenId(model.authData.wechat.openId))
        //            {
        //                用户不存在，openId不存在,注册操作
        //                if (!isNUll(model.authData.wechat.inopenId))
        //                {
        //                    if (!bll.QueryExitByUsername(model.authData.wechat.inopenId))
        //                    {
        //                        inopenId无效
        //                        return notFound("您的邀请用户手机号无效！");
        //                    }
        //                }
        //                else
        //                {
        //                    model.authData.wechat.inopenId = "";
        //                }
        //                model.authData.objectId = guid.ToString();
        //                model.authData.wechat.objectId = guid.ToString();

        //            }
        //            else
        //            {
        //                return notFound("请检查数据是否正确");
        //            }
        //        }


        //        DateTime dt = DateTime.Now;
        //        model.objectId = guid.ToString();
        //        密码加盐保存
        //        model.password = (model.password + model.objectId).Md5();
        //        初始化数据
        //        model.nickname = "口袋爆料人";
        //        model.credit = 40;
        //        model.overage = 0;
        //        model.sign_in = true;
        //        model.shake_times = 3;
        //        model.createdAt = dt;
        //        model.updatedAt = dt;
        //        string initPassword = "123456";
        //        model.transaction_password = (initPassword.Md5() + model.objectId).Md5();


        //        CreditsHistory history2 = new CreditsHistory();
        //        history2.objectId = guid.ToString();
        //        history2.createdAt = dt;
        //        history2.updatedAt = dt;
        //        history2.change = 40;
        //        history2.credit = 40;
        //        history2.type = 4;//注册得积分

        //        bool result = false;

        //        if (isPhone)
        //        {
        //            result = bll.Insert1(model, history2);
        //        }
        //        else
        //        {
        //            if (isNUll(model.authData.wechat.inopenId))
        //            {
        //                没有邀请人
        //                result = bll.Insert(model, history2);
        //            }
        //            else
        //            {
        //                有邀请人
        //                条件
        //                List<Wheres> whs = new List<Wheres>() { new Wheres("username", "=", model.authData.wechat.inopenId) };
        //                _User user = bll.QuerySingleByWheres(whs);
        //                CreditsHistory history1 = new CreditsHistory();
        //                Guid guid1 = Guid.NewGuid();
        //                history1.objectId = guid1.ToString();
        //                history1.createdAt = dt;
        //                history1.updatedAt = dt;
        //                history1.type = 3;//邀请得积分
        //                history1.change = 40;
        //                history1.credit = user.credit + 40;
        //                history1.userId = user.objectId;
        //                result = bll.Insert(model, history2, history1);
        //            }

        //        }

        //        if (result)
        //        {
        //            return ok(new { msg = "注册成功" });
        //        }
        //        return notFound("注册失败");
        //    }
        //    catch (Exception e)
        //    {
        //        return execept(e.Message);
        //    }

        //}


        public IHttpActionResult PostUserCredits(string v1, string objectId, int credits,int type)
        {
            try
            {
                if (isNUll(objectId) || credits == 0 || type == 0)
                {
                    return notFound("数据无效");
                }
                //条件
                List<Wheres> whs = new List<Wheres>() { new Wheres("objectId", "=", objectId) };               
                var m = bll.QuerySingleById(objectId);
                if (m == null)
                {
                    return notFound("数据无效");
                }
                int credit = m.credit + credits;
                CreditsHistory history = new CreditsHistory();
                Guid guid = Guid.NewGuid();
                history.objectId = guid.ToString();
                history.createdAt = DateTime.Now;
                history.updatedAt = DateTime.Now;
                history.type = type;
                history.change = credits;
                history.credit = credit;
                if (bll.UpdateCreditByObjectId(objectId, history))
                {
                    //返回当前积分
                    return ok(new { credit = credit });
                }
                else
                {
                    return notFound("修改失败");
                }                
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }

        ///// <summary>
        ///// 根据openId修改积分
        ///// </summary>
        ///// <param name="v1"></param>
        ///// <param name="openId"></param>
        ///// <param name="credits"></param>
        ///// <returns></returns>
        //public IHttpActionResult PostUserCreditsByOpenId(string v1, string openId, int credits)
        //{
        //    try
        //    {
        //        if (isNUll(openId))
        //        {
        //            return notFound("数据无效");
        //        }
                           
        //        bool success = bll.UpdateCreditByOpenId(openId,credits);
        //        if (success)
        //        {
        //            return ok("success");
        //        }
        //        else
        //        {
        //            return notFound("fail");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return execept(e.Message);
        //    }           
        //}

    }
}
