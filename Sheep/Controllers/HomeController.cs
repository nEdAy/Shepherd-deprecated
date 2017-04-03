using MvcApplication1.BLL;
using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sheep.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //itemBLL bll = new itemBLL();
            //for (int i = 0; i < 21; i++) {
            //    item model = new item();
            //    //model._User.objectId, model.title, model.price, model.discount, model.type, model.mall_name
            //    model.title = "title"+i;
            //    model.price = "price"+i;
            //    model.pic_a = "aa";
            //    model.url = "werw";
            //    model.discount = "discount"+i;
            //    model.type = "事务";
            //    model.mall_name = "sdfs";
            //    _User us = new _User();
            //    us.objectId = "844b0bf2-d4a6-418b-a327-cb7d4f72a0fa";
            //    model._User = us;
            //    //主键
            //    Guid guid = Guid.NewGuid();
            //    model.objectId = guid.ToString();
            //    //时间
            //    model.createdAt = DateTime.Now;
            //    model.updatedAt = DateTime.Now;

            //   bll.Insert(model);
            //}
            

            return View();
        }
    }
}
