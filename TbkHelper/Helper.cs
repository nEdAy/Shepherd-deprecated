using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using MvcApplication1.Model;
using Top.Api.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MvcApplication1.Utility;

namespace TbkHelper
{
    //0 9 15 20 
    public class Helper
    {
        private DefaultTopClient topClient;
        private long adzone_id;
        public Helper()
        {
            topClient = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23604371", "3c14f6fe16897babe1209d6ef3e5777d", "json");
            adzone_id = 70484723;
        }

        public IEnumerable<Favorite> getFavoriteList(string fields = "favorites_title,favorites_id,type", int page_no = 1, int page_size = 200, int type = -1)
        {
            TbkUatmFavoritesGetRequest req = new TbkUatmFavoritesGetRequest();
            //req.Fields = fields == FavoritesFields.all ? FavoritesFields.favorites_id.ToString() + "," + FavoritesFields.favorites_title.ToString() + "," + FavoritesFields.type.ToString() : fields.ToString();
            req.Fields = fields;
            req.PageNo = page_no;
            req.PageSize = page_size;
            req.Type = type;
            TbkUatmFavoritesGetResponse response = topClient.Execute(req);
            JToken tokenList = JsonConvert.DeserializeObject<JObject>(response.Body)["tbk_uatm_favorites_get_response"]["results"]["tbk_favorites"];

            foreach (JToken item in tokenList)
            {
                yield return new Favorite()
                {
                    favorites_id = Convert.ToInt64(item["favorites_id"].ToString()),
                    favorites_title = item["favorites_title"].ToString(),
                    type = Convert.ToInt64(item["type"].ToString())
                };
            }
        }

        public IEnumerable<FavoriteItem> getFavoriteitems(long favorites_id, int platform = 2, int page_no = 1, int page_size = 200)
        {
            TbkUatmFavoritesItemGetRequest req = new TbkUatmFavoritesItemGetRequest();
            req.Platform = platform;
            req.PageSize = page_size;
            req.AdzoneId = adzone_id;
            req.FavoritesId = favorites_id;
            req.PageNo = page_no;
            req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,click_url,seller_id,volume,nick,shop_title,zk_final_price_wap,event_start_time,event_end_time,tk_rate,status,type";
            TbkUatmFavoritesItemGetResponse response = topClient.Execute(req);
            JToken token = JsonConvert.DeserializeObject<JObject>(response.Body)["tbk_uatm_favorites_item_get_response"]["results"];
            if (token["uatm_tbk_item"] == null)
                yield break;
            JToken tokenList = token["uatm_tbk_item"];
            foreach (JToken item in tokenList)
            {
                yield return new FavoriteItem()
                {
                    favorites_id = favorites_id,
                    num_iid = Convert.ToInt64(item["num_iid"].ToString()),
                    title = item["title"] == null ? "" : item["title"].ToString(),
                    pict_url = item["pict_url"] == null ? "" : item["pict_url"].ToString(),
                    small_images = item["small_images"] == null || item["small_images"].ToString() == "{}" ? "" : item["small_images"]["string"].ToString(),
                    reserve_price = item["reserve_price"].ToString(),
                    zk_final_price = item["zk_final_price"] == null ? "" : item["zk_final_price"].ToString(),
                    user_type = Convert.ToInt64(item["user_type"].ToString()),
                    provcity = item["provcity"] == null ? "" : item["provcity"].ToString(),
                    item_url = item["item_url"] == null ? "" : item["item_url"].ToString(),
                    click_url = item["click_url"] == null ? "" : item["click_url"].ToString(),
                    volume = Convert.ToInt64(item["volume"].ToString()),
                    tk_rate = item["tk_rate"] == null ? "" : item["tk_rate"].ToString(),
                    zk_final_price_wap = item["zk_final_price_wap"] == null ? "" : item["zk_final_price_wap"].ToString(),
                    event_start_time = DateTime.Parse(item["event_start_time"].ToString()),
                    event_end_time = DateTime.Parse(item["event_end_time"].ToString())
                };
            }

        }

        public List<TbkItem> getTbkitems(string sort, int page_no, int page_size = 100, long start_tk_rate = 3000, long end_tk_rate = 0)
        {
            TbkItemGetRequest req = new TbkItemGetRequest();
            req.Fields = "num_iid,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,seller_id,volume,nick";
            req.Sort = sort;
            req.IsTmall = true;
            req.StartTkRate = start_tk_rate;
            req.EndTkRate = end_tk_rate;
            req.Platform = 2;
            req.PageNo = page_no;
            req.PageSize = page_size;
            req.Cat = get_cats();
            TbkItemGetResponse response = topClient.Execute(req);
            JToken token = JsonConvert.DeserializeObject<JObject>(response.Body)["tbk_item_get_response"];
            if (token["results"]["n_tbk_item"] == null)
                return null;
            JToken tokenList = token["results"]["n_tbk_item"];
            List<TbkItem> result = new List<TbkItem>();
            foreach (JToken item in tokenList)
            {

                result.Add(new TbkItem()
                {
                    num_iid = Convert.ToInt64(item["num_iid"].ToString()),
                    title = item["title"] == null ? "" : item["title"].ToString(),
                    pict_url = item["pict_url"] == null ? "" : item["pict_url"].ToString(),
                    small_images = item["small_images"] == null || item["small_images"].ToString() == "{}" ? "" : item["small_images"]["string"].ToString().Replace("\n", "").Replace("\r", ""),
                    reserve_price = item["reserve_price"].ToString(),
                    zk_final_price = item["zk_final_price"] == null ? "" : item["zk_final_price"].ToString(),
                    user_type = Convert.ToInt64(item["user_type"].ToString()),
                    provcity = item["provcity"] == null ? "" : item["provcity"].ToString(),
                    item_url = item["item_url"] == null ? "" : item["item_url"].ToString(),
                    nick = item["nick"] == null ? "" : item["nick"].ToString(),
                    seller_id = Convert.ToInt64(item["seller_id"].ToString()),
                    volume = Convert.ToInt64(item["volume"].ToString())
                });
            }

            return result;
        }

        public List<TqgItem> getTqgItem(int page, DateTime startTime, DateTime endTime)
        {
            TbkJuTqgGetRequest req = new TbkJuTqgGetRequest();
            req.AdzoneId = adzone_id;
            req.Fields = "num_iid,click_url,pic_url,reserve_price,zk_final_price,total_amount,sold_num,title,category_name,start_time,end_time";
            req.PageNo = page;
            req.StartTime = startTime;
            req.EndTime = endTime;
            TbkJuTqgGetResponse response = topClient.Execute(req);
            JToken token = JsonConvert.DeserializeObject<JObject>(response.Body)["tbk_ju_tqg_get_response"];
            List<TqgItem> result = new List<TqgItem>();
            if (token["results"] == null)
                return null;
            JToken tokenList = token["results"]["results"];

            foreach (JToken item in tokenList)
            {
                string click_url = item["click_url"] == null ? "" : item["click_url"].ToString();
                if (Convert.ToDouble(item["zk_final_price"].ToString()) > 5 && click_url.Length > 50 )
                {
                    result.Add(new TqgItem()
                    {
                        title = item["title"] == null ? "" : item["title"].ToString(),
                        total_amount = Convert.ToInt32(item["total_amount"].ToString()),
                        click_url = click_url,
                        category_name = item["category_name"] == null ? "" : item["category_name"].ToString(),
                        zk_final_price = item["zk_final_price"] == null ? "" : item["zk_final_price"].ToString(),
                        end_time = Convert.ToDateTime(item["end_time"].ToString()),
                        sold_num = Convert.ToInt32(item["sold_num"].ToString()),
                        start_time = Convert.ToDateTime(item["start_time"].ToString()),
                        pic_url = item["pic_url"] == null ? "" : item["pic_url"].ToString(),
                        num_iid = Convert.ToInt64(item["total_amount"].ToString()),
                        reserve_price = item["reserve_price"] == null ? "" : item["reserve_price"].ToString(),
                    });
                }
            }
            return result;
        }

        private string get_cats()
        {
            string[] all_cats = JsonHelper.readCatsFromLocal();
            int[] cats_key = JsonHelper.GetRandomArray(10, 0, 50);
            string result = all_cats[cats_key[0]];
            for(int i = 1; i < cats_key.Length; i++)
            {
                result += ("," + all_cats[cats_key[i]]);
            }
            return result;
        }
    }
}
