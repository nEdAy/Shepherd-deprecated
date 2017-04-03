using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Sheep.Models.ItemCache
{
    public class Cache
    {
        public Cache()
        {
            result = new List<CacheItem>();
            data = new Data() { total_num = 0, result = result };
        }
        public Data data { get; set; }

        public List<CacheItem> result
        {
            get
            {
                if (this._result == null)
                    return data.result;
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        private List<CacheItem> _result;

        public override string ToString()
        {
            string infos = "total_num=" + this.data.total_num + "\n\n";
            //foreach(CasheItem item in result)
            //{
            //    infos += item.ToString();
            //}
            
            return infos;
        }

        public void check()
        {
            for(int i = 0; i < result.Count; i++)
            {
                CacheItem item = this.result[i];
                if (item.Commission_queqiao * 0.85 > item.Commission_jihua)
                {
                    item.Commission_check = "true";
                }else
                    item.Commission_check = "false";

                item.Commission_queqiao = Double.NaN;
                item.Commission_jihua = Double.NaN;
            }
        }
    }
    

    public class Data
    {
        public int total_num { get; set; }
        public List<CacheItem> result { get; set; }
    }

    public class CacheItem
    {
        //商品id
        public string ID { get; set; }
        //商品淘宝id
        public string GoodsID { get; set; }
        //商品标题
        public string Title { get; set; }
        //商品主图
        public string Pic { get; set; }
        //分类ID
        public string Cid { get; set; }
        //正常售价
        public string Org_Price { get; set; }
        //券后价
        public string Price { get; set; }
        //是否天猫
        public string IsTmall { get; set; }
        //商品销量
        public string Sales_num { get; set; }
        //商品描述分
        public string Dsr { get; set; }
        //商品文案
        public string Introduce { get; set; }
        //优惠券金额
        public string Quan_price { get; set; }
        //优惠券结束时间
        public string Quan_time { get; set; }
        //优惠券剩余数量
        public string Quan_surplus { get; set; }
        //已领券数量
        public string Quan_receive { get; set; }
        //优惠券使用条件
        public string Quan_condition { get; set; }
        //手机券链接
        public string Quan_link { get; set; }
        //阿里链接
        public string ali_click { get; set; }

        public double Commission_jihua { get; set; }

        public double Commission_queqiao { get; set; }

        public string Commission_check { get; set; }

        public override string ToString()
        {
            Type t = typeof(CacheItem);
            System.Reflection.PropertyInfo[] properties = t.GetProperties();
            string infos = "";
            foreach (System.Reflection.PropertyInfo info in properties)
            {
                infos += info.Name + "=" + GetObjectPropertyValue<CacheItem>(this, info.Name) + "\n";
            }
            return infos;
        }

        /// <summary>
        /// 没有什么卵用的反射获取对象属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);

            System.Reflection.PropertyInfo property = type.GetProperty(propertyname);

            if (property == null) return string.Empty;

            object o = property.GetValue(t, null);

            if (o == null) return string.Empty;

            return o.ToString();
        }

        public void SetObjectPropertyValue<T>(T t, string propertyname, object propertyvalue)
        {
            Type type = typeof(T);
            PropertyInfo propertyInfo = type.GetProperty(propertyname);
            propertyInfo.SetValue(this, propertyvalue);
        }

    }
}