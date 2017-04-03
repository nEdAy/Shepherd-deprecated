using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TbkHelper;

namespace Sheep.Controllers
{
    public class FavoriteController : baseApiController
    {
        FavoriteBLL fbll = new FavoriteBLL();
        Helper topclient = new Helper();
        /// <summary>
        /// 返回favorite商品库列表
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="page">页数</param>
        /// <param name="size">一页数的大小</param>
        /// <param name="count">是否返回记录总数</param>
        /// <param name="wheres">条件</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public IHttpActionResult GetFavoritesList(string v1, int page = 0, int limit = 20, int count = 0, string where = null, string orderField = null, bool isDesc = true)
        {
            List<Wheres> list = new List<Wheres>(); 
            //条件
            if (!string.IsNullOrEmpty(where))
            {
                list = JsonHelper.Deserialize<List<Wheres>>(where);
            }

            List<Favorite> favorites = fbll.QueryList(page + 1, limit, list, orderField, isDesc).ToList();

            if (count == 1)
            {
                if (limit == 0)
                    return ok(new { total_num = fbll.QueryCount(list), result = new List<Favorite>() });
                else
                {
                    
                    return favorites == null ? 
                        error(new { result = "获取失败，请重试" }) : 
                        ok(new { total_num = fbll.QueryCount(list), result = favorites });
                }
            }
            else
            {
                return favorites == null ? error(new { result = "获取失败，请重试" }) : ok(new { result = favorites });
            }

        }

        /// <summary>
        /// 刷新产品库列表
        /// </summary>
        /// <param name="v1">版本</param>
        /// <returns></returns>
        public IHttpActionResult PostRefresh(string v1)
        {
            bool flag = true;
            
            IEnumerable<Favorite> allOnline = topclient.getFavoriteList();
            foreach (Favorite item in allOnline)//线上的 
            {
                bool status = true;
                //本地不存在，添加
                if (fbll.QuerySingleById(item.favorites_id) == null)
                {
                    status = fbll.Insert(item);
                    if (!status)
                    {
                        flag = false;
                        break;
                    }
                }
                //本地存在，更新
                else
                {
                    Dictionary<string, object> columns = new Dictionary<string, object>();
                    columns.Add("type", item.type);
                    columns.Add("favorites_title", item.favorites_title);
                    status = fbll.UpdateById(item.favorites_id, columns);
                    if (!status)
                    {
                        flag = false;
                        break;
                    }
                }

                

            }
            //删除本地多余数据
            List<Favorite> allLocal = fbll.QueryList(1, 200).ToList();
            List<Favorite> allOnlineList = allOnline.ToList();
            foreach (Favorite localItem in allLocal)
            {

                bool status = true;
                if (allOnlineList.Where<Favorite>(e => e.favorites_id == localItem.favorites_id).ToList().Count == 0)
                {
                    status = fbll.Delete(localItem.favorites_id);
                    if (!status)
                    {
                        flag = false;
                        break;
                    }
                }
            }
                return flag ? ok(new { result = "刷新成功" }) : error(new { result = "刷新失败或不完全刷新,请重试" });
        }

        /// <summary>
        /// 更新特定产品库信息
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="favorite_id">产品库id</param>
        /// <param name="favorites_info">产品库描述</param>
        /// <param name="image">产品库图片</param>
        /// <returns></returns>
        public IHttpActionResult PutUpdate(string v1, long favorites_id, string favorites_info, string image)
        {
            Dictionary<string, object> columns = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(favorites_info))
                columns.Add("favorites_info", favorites_info);
            if(!string.IsNullOrEmpty(image))
                columns.Add("image", image);
            bool stauts = fbll.UpdateById(favorites_id, columns);
            return stauts ? ok(new { result = "更新成功" }) : error(new { result = "更新失败" });
        }
    }
}