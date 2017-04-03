using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Sheep.Controllers
{
    public class itemUsersController : baseApiController
    {
        ItemUserBLL bll = new ItemUserBLL();
        //
        // GET: /itemUsers/
        public IHttpActionResult GetItemUser(string v1,string where)
        {
            try
            {
                //判断参数是否为空
                if (isNUll(where))
                {
                    invildRequest("条件不能为空");
                }
                var wheres = JsonHelper.Deserialize<List<Wheres>>(where);
                var model = bll.QuerySingleByWheresX(wheres, null);
                if (model == null)
                    return notFound("不存在指定信息");
                return ok(model);
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }
        public IHttpActionResult PostItemUser(string v1,[FromBody]ItemUser model)
        {
            try
            {
                //查询是否存在
                var IsHas = bll.QuerySingleByWheres(new List<Wheres> { new Wheres("itemId", "=", model.item.objectId), new Wheres("_UserId", "=", model._User.objectId,"and") });
                if (IsHas != null) {
                    model.updatedAt = DateTime.Now;
                    //表单验证
                    bool result = bll.UpdateByWheres(new List<Wheres> { new Wheres("itemId", "=", model.item.objectId), new Wheres("_UserId", "=", model._User.objectId,"and") }, new Dictionary<string, object>() { { "love", model.love }, { "dislove", model.dislove }, { "fav", model.fav } });
                    if (result)
                    {
                        return ok(model.updatedAt.ToString("yyyy-mm-dd HH:mm:ss"));
                    }
                    notFound("修改失败");
                }
                //主键
                Guid guid = Guid.NewGuid();
                model.objectId = guid.ToString();
                //时间
                model.createdAt = DateTime.Now;
                model.updatedAt = DateTime.Now;

                ////外键
                //_User modelUser = new _User();
                //model._User = modelUser;
                //item modelitem = new item();
                //model.item = modelitem;
                ////属性
                //model.love = love;
                //model.dislove = dislove;
                //model.fav = fav;
                //保存
                if (!bll.Insert(model))
                    return notFound("添加失败");
                return create(model.updatedAt.ToString("yyyy-mm-dd HH:mm:ss"));
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }
        public IHttpActionResult PutItemUser(string v1,string objectId, Boolean love,Boolean dislove,Boolean fav)
        {
            try
            {
                //判断参数是否为空
                if (isNUll(objectId))
                {
                    invildRequest("主键不能为空");
                }
                //查询是否存在
                var model = bll.QuerySingleById(objectId);
                if (model == null)
                {
                    notFound("不存在该记录");
                }
                model.updatedAt = DateTime.Now;
                //表单验证
                bool result = bll.Update(model);
                if (result)
                {
                    return create(model.updatedAt.ToString("yyyy-mm-dd HH:mm:ss"));
                }
                return notFound("不存在该记录");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
	}
}