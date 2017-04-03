using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Data;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.IO;
using MvcApplication1.Utility;
using MvcApplication1.Model;
using System.Collections;

namespace Sheep.Models.ItemCache
{
    public class CacheManager
    {
        public HttpClient client;
        public CacheManager()
        {
            this.client = new HttpClient("http://api.dataoke.com");
            CacheInit();
        }

        /// <summar>y
        /// 初始化缓存
        /// </summary>
        private static void CacheInit()
        {
            if (HttpRuntime.Cache["ITEM.CACHE.ALL"] == null
                || HttpRuntime.Cache["ITEM.CACHE.RT"] == null
                || HttpRuntime.Cache["ITEM.CACHE.TOP"] == null
                || HttpRuntime.Cache["ITEM.CACHE.POP"] == null)
            {
                //写入缓存
                if (HttpRuntime.Cache["ITEM.CACHE.ALL"] == null)
                {
                    HttpRuntime.Cache.Insert("ITEM.CACHE.ALL", new Cache());
                }

                if (HttpRuntime.Cache["ITEM.CACHE.RT"] == null)
                {
                    HttpRuntime.Cache.Insert("ITEM.CACHE.RT", new Cache());
                }

                if (HttpRuntime.Cache["ITEM.CACHE.TOP"] == null)
                {
                    HttpRuntime.Cache.Insert("ITEM.CACHE.TOP", new Cache());
                }

                if (HttpRuntime.Cache["ITEM.CACHE.POP"] == null)
                {
                    HttpRuntime.Cache.Insert("ITEM.CACHE.POP", new Cache());
                }
            }
        }

        /// <summary>
        /// 从接口获得商品信息
        /// </summary>
        /// <param name="url">API</param>
        /// <returns></returns>
        private Cache getCacheFromAPI(string url)
        {
            int flag = 0;
            string json = String.Empty;
            while (flag != 2)
            {
                try
                {
                    json = client.Get(url);
                    flag = 2;
                }
                catch
                {
                    flag++;
                }
            }
            if (String.IsNullOrEmpty(json))
                return null;
            else
                return JsonConvert.DeserializeObject<Cache>(json);

        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public void RemoveCache(string type)
        {
            if (type == "ALL")
                HttpRuntime.Cache.Remove("ITEM.CACHE.ALL");

            if (type == "RT")
                HttpRuntime.Cache.Remove("ITEM.CACHE.RT");

            if (type == "TOP")
                HttpRuntime.Cache.Remove("ITEM.CACHE.TOP");

            if (type == "POP")
                HttpRuntime.Cache.Remove("ITEM.CACHE.POP");

        }

        /// <summary>
        /// 写入缓存（此方法已作废）
        /// </summary>
        /// <param name="cacheName">缓存名</param>
        /// <param name="cache">缓存对象</param>
        private void WriteCache(string cacheName, Cache cache)
        {
            DataTable dt = (DataTable)HttpRuntime.Cache["ITEM.CACHE." + cacheName];
            foreach (CacheItem item in cache.result)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = item.ID;
                dr["GoodsID"] = item.GoodsID;
                dr["Pic"] = item.Pic;
                dr["Cid"] = item.Cid;
                dr["Org_Price"] = item.Org_Price;
                dr["Price"] = item.Price;
                dr["IsTmall"] = item.IsTmall;
                dr["Sales_num"] = item.Sales_num;
                dr["Introduce"] = item.Introduce;
                dr["Quan_surplus"] = item.Quan_surplus;
                dr["Quan_condition"] = item.Quan_condition;
                dr["Quan_link"] = item.Quan_link;
                dt.Rows.Add(dr);
            }
        }
         
			
        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="type"></param>
        public void RefreshCache(int type = 0)
        {
            CacheInit();

            if (type == 0 || type == 1)
            {
                try
                {
                    int i = 1;
                    Cache cache = new Cache();
                    Cache cache_all = new Cache();
                    while (i<=100)
                    {
                        cache_all = getCacheFromAPI("index.php?r=Port/index&type=total&appkey=nj3qafdd2j&v=2&page=" + i);
                        cache_all.check();
                        System.Threading.Thread.Sleep(5000);
                        i++;
                        if (cache_all.result == null || cache_all.data.total_num == 0)
                            break;
                        cache.result.AddRange(cache_all.result);
                    }
                    cache.data.total_num = cache.result.Count;
                    Serialize2Local(cache, "ALL");
                    RemoveCache("ALL");
                    HttpRuntime.Cache.Insert("ITEM.CACHE.ALL", cache);
                }
                catch {
                    RefreshCache(type);
                }
                //WriteCache("ITEM.CACHE.ALL", cache_all);
            }
            if (type == 0 || type == 2)
            {
                try
                {
                    Cache cache_rt = getCacheFromAPI("index.php?r=Port/index&type=paoliang&appkey=nj3qafdd2j&v=2");
                    cache_rt.check();
                    Serialize2Local(cache_rt, "RT");
                    RemoveCache("RT");
                    HttpRuntime.Cache.Insert("ITEM.CACHE.RT", cache_rt);
                }
                catch
                {
                    RefreshCache(type);
                }
                //WriteCache("ITEM.CACHE.RT", cache_rt);
            }
            if (type == 0 || type == 3)
            {
                try
                {
                    Cache cache_top = getCacheFromAPI("index.php?r=Port/index&type=top100&appkey=nj3qafdd2j&v=2");
                    cache_top.check();
                    Serialize2Local(cache_top, "TOP");
                    RemoveCache("TOP");
                    HttpRuntime.Cache.Insert("ITEM.CACHE.TOP", cache_top);
                }
                catch
                {
                    RefreshCache(type);
                }
                //WriteCache("ITEM.CACHE.TOP", cache_top);
            }
            if (type == 4)
            {
                try
                {
                    Cache cache_pop = getCacheFromAPI("index.php?r=goodsLink/android&type=android_quan&appkey=nj3qafdd2j&v=2 ");
                    Serialize2Local(cache_pop, "POP");
                    RemoveCache("POP");
                    HttpRuntime.Cache.Insert("ITEM.CACHE.POP", cache_pop);
                }
                catch
                {
                    RefreshCache(type);
                }
            }
        }

        /// <summary>
        /// 获得缓存
        /// </summary>
        /// <param name="name">key</param>
        /// <returns></returns>
        public static Cache GetCasheByName(string key)
        {
            Cache cache = (Cache)HttpRuntime.Cache["ITEM.CACHE." + key];
            return cache;
        }

        /// <summary>
        /// 数据表转为缓存对象集合（此方法已作废）
        /// </summary>
        /// <param name="drs"></param>
        /// <returns></returns>
        private static List<CacheItem> Table2CacheItems(List<DataRow> drs)
        {
            List<CacheItem> items = new List<CacheItem>();
            if (drs != null)
            {
                foreach (DataRow item in drs)
                {
                    CacheItem cacheItem = new CacheItem();
                    cacheItem.ID = item["ID"].ToString();
                    cacheItem.GoodsID = item["GoodsID"].ToString();
                    cacheItem.Title = item["Title"].ToString();
                    cacheItem.Pic = item["Pic"].ToString();
                    cacheItem.Cid = item["Cid"].ToString();
                    cacheItem.Org_Price = item["Org_Price"].ToString();
                    cacheItem.Price = item["Price"].ToString();
                    cacheItem.IsTmall = item["IsTmall"].ToString();
                    cacheItem.Sales_num = item["Sales_num"].ToString();
                    cacheItem.Dsr = item["Dsr"].ToString();
                    cacheItem.Introduce = item["Introduce"].ToString();
                    cacheItem.Quan_price = item["Quan_price"].ToString();
                    cacheItem.Quan_time = item["Quan_time"].ToString();
                    cacheItem.Quan_surplus = item["Quan_surplus"].ToString();
                    cacheItem.Quan_receive = item["Quan_receive"].ToString();
                    cacheItem.Quan_condition = item["Quan_condition"].ToString();
                    cacheItem.Quan_link = item["Quan_link"].ToString();
                    items.Add(cacheItem);
                }
            }
            return items;
        }

        /// <summary>
        /// 分页逻辑
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="where">查询语句（json形式）</param>
        /// <param name="page">页数</param>
        /// <param name="limit">一页的内容数</param>
        /// <returns></returns>
        public static Cache Query(Cache cache, string where, int page, int limit)
        {

            List<Wheres> list = new List<Wheres>();
            //查询语句模板
            string wherePredicate = "";
            //查询语句值
            object[] whereValues = null;

            //被模糊查询属性名
            string like = "";
            //模糊查询值
            string likeWho = "";

            //将json格式的查询字符串转化为查询语句
            if (!string.IsNullOrEmpty(where))
            {
                list = JsonHelper.Deserialize<List<Wheres>>(where);
                wherePredicate = ((object)list).parseWheresForDynamicLINQ(out whereValues, out like, out likeWho);
            }

            //解决不同json反序列化为cache后的适配问题
            if (cache.result.Count == 0)
                cache.result = cache.data.result;


            //查询语句不为空的时候
            if (!String.IsNullOrEmpty(wherePredicate))
            {
                if (!String.IsNullOrEmpty(like))
                {
                    List<CacheItem> results = cache.result.AsQueryable()
                        .Where(wherePredicate, whereValues)
                        //.Where(c => c.GetObjectPropertyValue<CacheItem>(c, like).Contains(likeWho)).ToList()
                        .Where(c => ContainsAny(c.GetObjectPropertyValue<CacheItem>(c, like), likeWho.ToCharArray())).ToList();
                    results.Sort(new ItemSort(likeWho));
                    cache = new Cache()
                    {
                        result = results
                    };
                }

                else
                    cache = new Cache()
                    {
                        result = cache.result.AsQueryable()
                        .Where(wherePredicate, whereValues).ToList()
                    };
            }
            else
                //模糊查询不为空的时候
                if (!String.IsNullOrEmpty(like))
            {
                List<CacheItem> results = cache.result.AsQueryable()
                        .Where(c => ContainsAny(c.GetObjectPropertyValue<CacheItem>(c, like), likeWho.ToCharArray())).ToList();
                        results.Sort(new ItemSort(likeWho));
                cache = new Cache()
                {
                    result = results
                };
            }

            List<CacheItem> result = null;
            //分页逻辑
            if (limit != 0)
            {
                //起始行数
                int rowBegin = page * limit;
                //结束行数
                int rowEnd = (page + 1) * limit;
                if (rowBegin >= cache.result.Count)
                {
                    return new Cache() { data = new Data() { total_num = 0 }, result = Table2CacheItems(null) };
                }
                if (rowEnd > cache.result.Count)
                {
                    rowEnd = cache.result.Count;
                }

                result = cache.result.AsEnumerable().Take(rowEnd).Skip(rowBegin).ToList();
            }
            else
                result = cache.result;
            return new Cache() { data = new Data() { total_num = cache.result.Count }, result = result.ToList() };


        }


        public void Serialize2Local(Cache cache, string name)
        {

            using (FileStream fsWrite = new FileStream("D:\\cache\\" + name, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cache));
                fsWrite.Write(buffer, 0, buffer.Length);
                fsWrite.Flush();
                fsWrite.Close();
            }
        }

        public static Cache GetFromLocal(string name)
        {
            string file = File.ReadAllText("C:\\cache\\" + name);
            return JsonConvert.DeserializeObject<Cache>(file);
        }

        public static bool ContainsAny(string str, params char[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (char value in values)
                {
                    if (str.Contains(value))
                        return true;
                }
            }

            return false;
        }
    
        public class ItemSort : IComparer<CacheItem>
        {
            public string like;

            public ItemSort(string like)
            {
                this.like = like;
            }

            public int Compare(CacheItem x, CacheItem y)
            {
                bool xFlag = x.Title.Contains(like);
                bool yFlag = y.Title.Contains(like);

                return xFlag == yFlag ? 0 : (xFlag && !yFlag) ? -1 : 1;
            }
        }
    }    
}