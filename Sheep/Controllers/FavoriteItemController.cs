using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TbkHelper;


namespace Sheep.Controllers
{
    public class FavoriteItemController : baseApiController
    {
        FavoriteItemBLL fibll = new FavoriteItemBLL();
        Helper topclient = new Helper();

        /// <summary>
        /// 获取favorite商品列表
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="page">页数</param>
        /// <param name="limit">一页数的大小</param>
        /// <param name="count">是否返回记录总数</param>
        /// <param name="where">条件</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public IHttpActionResult GetFavoritesItemList(string v1, int page = 0, int limit = 20, int count = 0, string where = null, string orderField = null, bool isDesc = true)
        {
            List<Wheres> list = new List<Wheres>();
            //条件
            if (!string.IsNullOrEmpty(where))
            {
                list = JsonHelper.Deserialize<List<Wheres>>(where);
            }

            List<FavoriteItem> favoriteItems = fibll.QueryList(page + 1, limit, list, orderField, isDesc).ToList();

            if (count == 1)
            {
                if (limit == 0)
                    return ok(new { total_num = fibll.QueryCount(list), result = new List<FavoriteItem>() });
                else
                {
                    
                    return favoriteItems == null ? error(new { result = "获取失败，请重试" }) : ok(new { total_num = fibll.QueryCount(list), result = favoriteItems });
                }
            }
            else
            {
                return favoriteItems == null ? error(new { result = "获取失败，请重试" }) : ok(new { result = favoriteItems });
            }
        }

        /// <summary>
        /// 刷新产品库内产品
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="favorites_id">对应产品库id（默认为0，刷新全部）</param>
        /// <returns></returns>
        public IHttpActionResult PostRefresh(string v1, long favorites_id = 0)
        {
            bool flag = true;
            if (favorites_id == 0)
            {
                if(fibll.QueryCount(null) == 0 || fibll.DeleteAll())
                {
                    foreach(Favorite favorites in topclient.getFavoriteList())
                    {
                        foreach(FavoriteItem item in topclient.getFavoriteitems(favorites.favorites_id))
                        {
                            bool status = fibll.Insert(item);
                            if (!status)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    return flag ? ok(new { result = "刷新成功" }) : error(new { result = "刷新失败或不完全刷新,请重试" });

                }
                else return error("刷新失败");
            }
            else
            {
                    foreach (FavoriteItem item in topclient.getFavoriteitems(favorites_id))
                    {
                        bool status = fibll.Insert(item);
                        if (!status)
                        {
                            flag = false;
                            break;
                        }
                    }
                    return flag ? ok(new { result = "刷新成功" }) : error("刷新失败或不完全刷新,请重试");
            }
        }

    }
}