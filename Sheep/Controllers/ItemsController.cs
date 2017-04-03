using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class ItemsController : baseApiController
    {
        /// <summary>
        /// 业务处理对象
        /// </summary>
        itemBLL bll = new itemBLL();
        /// <summary>
        /// 得到单个用户
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="objectId">主键</param>
        /// <returns>用户json</returns>
        public IHttpActionResult GetItem(string v1, string objectId)
        {
            try
            {
                if (string.IsNullOrEmpty(objectId))
                {
                    return invildRequest("itemId不能为空");
                }
                var model = bll.QuerySingleById(objectId);
                if (model == null)
                {
                    return notFound("没有该记录");
                }
                return ok(model);
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        /// <summary>
        /// 得到所有用户
        /// </summary>
        /// <param name="v1">版本</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <param name="include">列集合</param>
        /// <param name="limit">页数</param>
        /// <param name="page">页码</param>
        /// <returns>所有用户json集合</returns>
        public IHttpActionResult GetItems(string v1, int limit = 10, int page = 0, string where = null, string include = null, string order = null)
        {
            try
            {
                List<Wheres> list = new List<Wheres>();
                Boolean isDesc = true;
                string orderField = "updatedAt";
                //条件
                if (!string.IsNullOrEmpty(where))
                {
                    list = JsonHelper.Deserialize<List<Wheres>>(where);
                }
                //判断页码
                if (limit == 0)
                {
                    int num = bll.QueryCount(list);
                    return ok(new { results = new string[] { }, count = num });
                }
                //排序
                if (!string.IsNullOrEmpty(order))
                {
                    string descStr = order.Substring(0, 1);
                    if (descStr.Equals("-"))
                    {
                        isDesc = true;
                    }
                    else
                    {
                        isDesc = false;
                    }

                    orderField = order.Substring(1);
                }
                //列集合
                if (string.IsNullOrEmpty(include))
                {
                    //为空时，查询所有列
                    var models = bll.QueryList(page, limit, list, orderField, isDesc);
                    return ok(new { results = models });
                }
                else
                {
                    //非空时，解析所有列
                    Dictionary<string, string[]> columns = new Dictionary<string, string[]>();
                    string includeInit = include.Substring(0, include.Count() - 1);
                    string[] cols = includeInit.Split(new string[] { "]," }, StringSplitOptions.None);
                    foreach (var col in cols)
                    {
                        string[] cols1 = col.Split('[');
                        columns.Add(cols1[0], cols1[1].Split('|'));
                    }
                    var models = bll.QueryListX(page, limit, columns, list, orderField, isDesc);
                    return ok(new { results = models });
                }
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }

        public IHttpActionResult PostItem(string v1, [FromBody]item model)
        {
            try
            {
                //表单验证
                if (isNUll(model._User.objectId, model.title, model.price, model.discount, model.type, model.mall_name, model.url))
                {
                    return invildRequest("参数不能为空");
                }
                //主键
                Guid guid = Guid.NewGuid();
                model.objectId = guid.ToString();
                //时间
                model.createdAt = DateTime.Now;
                model.updatedAt = DateTime.Now;

                bool result = bll.Insert(model);
                if (result)
                {
                    return create(model.updatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

                }
                return notFound("注册失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        public IHttpActionResult DeleteItem(string v1, string objectId)
        {
            try
            {
                //表单验证
                if (isNUll(objectId))
                {
                    return invildRequest("参数不能为空");
                }

                bool result = bll.Delete(objectId);
                if (result)
                {
                    return ok("ok");

                }
                return notFound("删除失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        public IHttpActionResult PutItem(string v1, [FromBody]item model)
        {
            try
            {
                model.updatedAt = DateTime.Now;
                //表单验证
                bool result = bll.Update(model);
                if (result)
                {
                    return ok(model.updatedAt.ToString("yyyy-mm-dd HH:mm:ss"));
                }
                return notFound("修改失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        public IHttpActionResult PutHot(string v1, string objectId)
        {
            try
            {
                if (string.IsNullOrEmpty(objectId))
                {
                    return invildRequest("itemId不能为空");
                }
                DateTime upadatedAt = DateTime.Now;
                bool result = bll.UpdateHot(objectId);
                if (result)
                {
                    return create(upadatedAt.ToString("yyyy-mm-dd HH:mm:ss"));
                }
                return notFound("修改失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        
    }
}