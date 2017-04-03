using MvcApplication1.BLL;
using MvcApplication1.Model;
using Sheep.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class signInController : baseApiController
    {
       
        _UserBLL userbll = new _UserBLL();
        CreditsHistoryBLL historyBLL = new CreditsHistoryBLL();
        SignInBLL signBll = new SignInBLL();

        public IHttpActionResult Get(string v1, string objectId, int type)
        {
            try
            {

                if (string.IsNullOrEmpty(objectId))
                {
                    return invildRequest("用户ID不能为空");
                }

                _User user = userbll.QuerySingleById(objectId);
                if (user == null)
                {
                    return notFound("用户不纯在");
                }
                if (!user.sign_in)
                {
                    return notFound("已签到");
                }



                CreditsHistory history = new CreditsHistory();
                if (type == 0)
                {
                    history.type = 0;
                    history.change = 2;
                }
                else if (type == 1)
                {
                    Random ran = new Random();
                    history.change = ran.Next(0, 6);
                    history.type = 1;
                }
                else
                {
                    Random ran = new Random();
                    history.change = ran.Next(-2, 9);
                    history.type = 2;
                }
                Guid guid = Guid.NewGuid();
                history.objectId = guid.ToString();

                history.updatedAt = DateTime.Now;
                history.createdAt = DateTime.Now;
                history.userId = objectId;
                history.credit = user.credit + history.change;

                if (historyBLL.SignIn(history, objectId))
                {
                    return ok(history);
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

        //public IHttpActionResult GetSignIn(string v1, string objectId)
        //{
        //    _User user = userbll.QuerySingleById(objectId);
        //    if(user == null)
        //    {
        //        return error("用户不存在");
        //    }
        //    else
        //    {
        //        SignIn signIn = signBll.QuerySingleById(objectId);
                
        //        if(signIn == null)
        //        {
        //            SignIn new_signIn = new SignIn()
        //            {
        //                objectId = user.objectId,
        //                sustain = 0,
        //                total = 0,
        //                updateAt = DateTime.MinValue.AddYears(1970)
        //            };

        //            signBll.Insert(new_signIn);

        //            return ok(new_signIn);
        //        }
        //        else
        //        {
        //            int dayDuration = GetDayDuration(signIn.updateAt);
        //            if (dayDuration > 1)
        //            {
        //                signBll.UpdateById(signIn.objectId, new Dictionary<string, Object>() { { "sustain", 0 } });
        //                return ok(signBll.QuerySingleById(signIn.objectId));
        //            }
        //            else
        //            {
        //                return ok(signIn);
        //            }
                    
                    
        //        }

        //    }

        //}

        //public IHttpActionResult PutSignIn(string v1, string objectId)
        //{
        //    SignIn signIn = signBll.QuerySingleById(objectId);
        //    if(signIn == null)
        //    {
        //        return error("用户不存在");
        //    }
        //    else
        //    {
        //        int dayDuration = GetDayDuration(signIn.updateAt);
        //        if (dayDuration == 0)
        //        {
        //            return forbidden("今日已签到");
        //        }
        //        else
        //        {
        //            int sustain = signIn.sustain;
        //            int change = 0;

        //            if (dayDuration == 1)
        //            {
        //                if (sustain == 7)
        //                {
        //                    sustain = 1;
        //                    change = 3;
        //                }
        //                else if (sustain <= 2)
        //                {
        //                    sustain += 1;
        //                    change = 3;
        //                }
        //                else if (sustain == 3)
        //                {
        //                    sustain += 1;
        //                    change = 5;
        //                }
        //                else if (sustain == 4 || sustain == 5)
        //                {
        //                    sustain += 1;
        //                    change = 6;
        //                }
        //                else if (sustain == 6)
        //                {
        //                    sustain += 1;
        //                    change = 8;
        //                }

        //            }
        //            else
        //            {
        //                sustain = 1;
        //                change = 3;
        //            }

        //            signBll.UpdateById(signIn.objectId, new Dictionary<string, Object>() {
        //                { "sustain", sustain},
        //                { "total", signIn.total + 1 },
        //                { "updateAt", DateTime.Now }
        //                });
        //            changeCredit(signIn.objectId, change);

        //            return SignInOk(signBll.QuerySingleById(signIn.objectId), change);

        //        }
                
        //    }
        //}
        

        private int GetDayDuration(DateTime updateTime)
        {
            TimeSpan updateTimeSpan = new TimeSpan(updateTime.Date.Ticks);
            TimeSpan nowSpan = new TimeSpan(DateTime.Now.Date.Ticks);
            TimeSpan duration = updateTimeSpan.Subtract(nowSpan).Duration();
            int dayDuration = duration.Days;
            return dayDuration;
        }

        private CommenResult SignInOk(SignIn signIn, int change)
        {
            _User user = userbll.QuerySingleById(signIn.objectId);
            return ok(new {
                userId = signIn.updateAt.ToShortDateString(),//为向下兼容，暂时返回时间字符串
                type = signIn,//为向下兼容，暂时返回连续签到天数
                change = change,//积分变化值
                credit = user.credit//积分变化后的值
            });
        }

        private void changeCredit(string objectId, int addCredit)
        {
            _User user = userbll.QuerySingleById(objectId);
            userbll.UpdateById(objectId, new Dictionary<string, Object>() {
                        { "credit", user.credit + addCredit},
                        });
            CreditsHistoryBLL chBll = new CreditsHistoryBLL();
            chBll.Insert(new CreditsHistory()
            {
                objectId = Guid.NewGuid().ToString(),
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                userId = user.objectId,
                type = 0,
                change = addCredit,
                credit = user.credit + addCredit
            });
            
        }
    }
}
