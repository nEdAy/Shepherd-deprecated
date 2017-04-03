using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TbkHelper;
using MvcApplication1.BLL;
using MvcApplication1.Utility;

namespace Sheep.Controllers
{
    public class TbkItemController : baseApiController
    {
        TbkItemBLL bll = new TbkItemBLL();
        Helper topClient = new Helper();

        /// <summary>
        /// 获取淘宝客商品列表
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="page">页数</param>
        /// <param name="limit">一页数的大小</param>
        /// <param name="count">是否返回记录总数</param>
        /// <param name="where">条件</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public IHttpActionResult getTbkItem(string v1, int page = 0, int limit = 20, int count = 0, string where = null, string orderField = null, bool isDesc = true)
        {
            List<Wheres> list = new List<Wheres>();
            //条件
            if (!string.IsNullOrEmpty(where))
            {
                list = JsonHelper.Deserialize<List<Wheres>>(where);
            }

            List<TbkItem> tbkItems = bll.QueryList(page + 1, limit, list, orderField, isDesc).ToList();
            if (count == 1)
            {
                if (limit == 0)
                    return ok(new { total_num = bll.QueryCount(list), result = new List<TbkItem>() });
                else
                {
                    
                    return tbkItems == null ? error(new { result = "获取失败，请重试" }) : ok(new { total_num = bll.QueryCount(list), result = tbkItems });
                }
            }
            else
            {
                return tbkItems == null ? error(new { result = "获取失败，请重试" }) : ok(new { result = tbkItems });
            }
        }

        /// <summary>
        /// 刷新产品库内产品
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="sort">排序方案</param>
        /// <param name="count">刷新量（以1*100为单位）</param>
        /// <param name="start_tk_rate">佣金比例上限</param>
        /// <param name="end_tk_rate">佣金比例下限</param>
        /// <returns></returns>
        public IHttpActionResult postRefresh(string v1, string sort, int count = 1, long start_tk_rate = 3000, long end_tk_rate = 0)
        {
            if(bll.QueryCount(null) == 0 || bll.DeleteAll())
            {
                bool flag = true;
                int amount = 0;
                for (int i = 1; i <= count; i++)
                {
                    List<TbkItem> list = topClient.getTbkitems(sort, i, 100, start_tk_rate, end_tk_rate);
                    if (list == null)
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
    }
}