using MvcApplication1.Model;
using MvcApplication1.Utility;
using Sheep.Models.ItemCache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class PortItemsController : baseApiController
    {
        /// <summary>
        /// 商品API转换接口
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="type">类型（1 = 全站领券商品， 2 = 实时跑量榜， 3 = TOP100人气榜）</param>
        /// <param name="page">页数</param>
        /// <param name="count">是否返回记录总数</param>
        /// <param name="limit">一页内容数</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public IHttpActionResult GetItems(string v1, int type = 1, int page = 0, int count = 0, int limit = 20, string where = null)
        {

            
            string tableName = null;
            Cache cache = null;
            try
            {
                switch (type)
                {
                    //case 0:
                    //    new CacheManager().RefreshCache(type+2);
                    //    return ok("refresh ok");
                    //case 1:
                    //    tableName = "ALL"; break;
                    case 2:
                        tableName = "RT"; break;
                    case 3:
                        tableName = "TOP"; break;
                    case 4:
                        tableName = "POP"; break;
                    default:
                        tableName = "ALL"; break;
                }

                cache = CacheManager.GetCasheByName(tableName);
                if(cache == null)
                    cache = CacheManager.GetFromLocal(tableName);

                Cache c = CacheManager.Query(cache, where, page, limit);
                List<CacheItem> results = c.result;

                if (count == 1)
                {
                    if (limit == 0)
                        return ok(new { total_num = c.data.total_num, result = new List<CacheItem>() });
                    return ok(new { total_num = c.data.total_num, result = results });
                }
                else
                    return ok(new { result = results });
            }
            catch (NullReferenceException e)
            {
                if (cache == null)
                    return notFound("缓存数据为空或不存在，请尝试手动刷新(" + e.Message + ")");
                else
                    return notFound(e.Message);
            }
            catch (Exception e)
            {
                return error(e.Message);
            }
        }

        /// <summary>
        /// 手动刷新数据
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="type">选择刷新的数据（0为全部）</param>
        /// <returns></returns>
        public IHttpActionResult PostRefresh(String v1, int type = 0)
        {
            try
            {
                new CacheManager().RefreshCache(type);
                return ok(new { result = "refresh ok" });
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }
    }
}
