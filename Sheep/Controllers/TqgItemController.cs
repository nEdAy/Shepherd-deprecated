using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MvcApplication1.Utility;
using MvcApplication1.BLL;
using TbkHelper;
using MvcApplication1.Model;

namespace Sheep.Controllers
{
    public class TqgItemController : baseApiController
    {
        TqgItemBLL bll = new TqgItemBLL();
        Helper topClient = new Helper();

        public IHttpActionResult getTqgItem(string v1 ,int time = 0 , int type = 1, int page = 0, int limit = 20, int count = 0)
        {
            DateTime day_start = DateTime.Now.Date;
            DateTime dt;
            if (type == 0)
            {
                dt = day_start.AddHours(time);
            }
            else
            {
                switch (type)
                {
                    case 1:
                        dt = day_start;
                        break;
                    case 2:
                        dt = day_start.AddHours(10);
                        break;
                    case 3:
                        dt = day_start.AddHours(19);
                        break;
                    case 4:
                        dt = day_start.AddHours(22);
                        break;
                    default:
                        dt = day_start;
                        break;
                }
            }

            List<TqgItem> tqgItems = getTqgItembyTime(dt, page + 1, limit);

            if (count == 1)
            {
                if (limit == 0)
                    return ok(new { total_num = countTqgItem(dt), result = new List<TqgItem>() });
                else
                {
                    
                    return tqgItems == null ? error(new { result = "获取失败，请重试" }) : ok(new { total_num = countTqgItem(dt), result = tqgItems });
                }
            }
            else
            {
                return tqgItems == null ? error(new { result = "获取失败，请重试" }) : ok(new { result = tqgItems });
            }
        }

        public IHttpActionResult postRefresh(string v1, int count = 10)
        {
            DateTime day_start = DateTime.Now.Date;
            DateTime day_end = day_start.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (bll.QueryCount(null) == 0 || bll.DeleteAll())
            {
                bool flag = true;
                int i;
                List<TqgItem> list = new List<TqgItem>();
                int amount = 0;
                for (i = 1; i <= count; i++)
                {
                    list = topClient.getTqgItem(i, day_start, day_end);
                    if (list  == null)
                        break;
                    bool stauts = bll.InsertSome(list);
                    if (!stauts)
                        flag = false;
                    else
                        amount += list.Count;
                }
                return flag ? ok(new { result = "刷新成功，刷新数据量:" + amount }) : ok(new { reuslt = "刷新不完全，建议重试" });
            }
            else return error("刷新失败");

        }

        private List<TqgItem> getTqgItembyTime(DateTime start_time, int page, int limit = 20)
        {
            var result = bll.QueryList(page, limit,
                new List<Wheres>() {
                    new Wheres("start_time", "=", start_time.ToString("yyyy-MM-dd HH:mm:ss"))
                });
            return result.ToList();
        }

        private int countTqgItem(DateTime start_time)
        {
            return bll.QueryCount(
                new List<Wheres>() {
                    new Wheres("start_time", "=", start_time.ToString("yyyy-MM-dd HH:mm:ss"))
                });
        }


    }
}