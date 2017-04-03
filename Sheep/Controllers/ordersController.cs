using Model;
using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class ordersController : baseApiController
    {
        RechargeHistoryBLL bll = new RechargeHistoryBLL();
        public IHttpActionResult Post(string v1, [FromBody]order myOrder)
        {
           
            try {
                //string objectId = myOrder.trade_no;
                //RechargeHistory model = bll.QuerySingleById(objectId);
                //HttpClint query = new HttpClint();

                //RechargeHistory mm = new RechargeHistory();
                //mm.createdAt = DateTime.Now;
                //mm.updatedAt = DateTime.Now;
                //mm.objectId = "11111111";
                //bll.Update(mm);

                string out_trade_no = myOrder.out_trade_no;
                string response = HttpHelper.Get(@"https://api.bmob.cn/1/pay/" + out_trade_no, new { });
                RechargeHistory model = JsonHelper.Deserialize<RechargeHistory>(response);


                RechargeHistory modelX = bll.QuerySingleById(model.body);

                model.updatedAt = DateTime.Now;
                model.createdAt = modelX.createdAt;


                model.objectId = model.body;
                model.userId = modelX.userId;
                model.trade_state = myOrder.trade_status;
                model.out_trade_no = myOrder.out_trade_no;
                bool result = bll.Update(model);
                _UserBLL userbll = new _UserBLL();
                _User userModel =userbll.QuerySingleById(model.userId);
                userbll.UpdateById(model.userId, new Dictionary<string, object> { { "overage", userModel.overage+model.total_fee*100 } });
                if (result)
                {
                    return ok("success");
                }
                return ok("failure");
            }
            catch(Exception e) {
                return ok(e.Message);
            }

        }
    }
}
