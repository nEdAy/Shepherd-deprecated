using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using MvcApplication1.Model;
using MvcApplication1.Utility;
namespace MvcApplication1.DAO
{
    public partial class FavoriteItemDAO
    {
        #region 向数据库中添加一条记录 +bool Insert(FavoriteItem model)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool Insert(FavoriteItem model)
        {
            const string sql = @"INSERT INTO [dbo].[FavoriteItem] (num_iid,favorites_id,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,click_url,volume,tk_rate,zk_final_price_wap,event_start_time,event_end_time) VALUES (@num_iid,@favorites_id,@title,@pict_url,@small_images,@reserve_price,@zk_final_price,@user_type,@provcity,@item_url,@click_url,@volume,@tk_rate,@zk_final_price_wap,@event_start_time,@event_end_time)";
            int res = SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@num_iid", model.num_iid.ToDBValue()), new SqlParameter("@favorites_id", model.favorites_id.ToDBValue()), new SqlParameter("@title", model.title.ToDBValue()), new SqlParameter("@pict_url", model.pict_url.ToDBValue()), new SqlParameter("@small_images",  model.small_images.Replace("\n","").Replace("\r","").ToDBValue()), new SqlParameter("@reserve_price", model.reserve_price.ToDBValue()), new SqlParameter("@zk_final_price", model.zk_final_price.ToDBValue()), new SqlParameter("@user_type", model.user_type.ToDBValue()), new SqlParameter("@provcity", model.provcity.ToDBValue()), new SqlParameter("@item_url", model.item_url.ToDBValue()), new SqlParameter("@click_url", model.click_url.ToDBValue()), new SqlParameter("@volume", model.volume.ToDBValue()), new SqlParameter("@tk_rate", model.tk_rate.ToDBValue()), new SqlParameter("@zk_final_price_wap", model.zk_final_price_wap.ToDBValue()), new SqlParameter("@event_start_time", model.event_start_time.ToDBValue()), new SqlParameter("@event_end_time", model.event_end_time.ToDBValue()));
            return res > 0;
        }

        /// <summary>
        /// 删除所有记录
        /// </summary>
        /// <returns>是否成功</returns>
        public bool DeleteAll()
        {
            const string sql = "truncate table [dbo].[FavoriteItem]";
            //const string sql = "DELETE FROM [dbo].[FavoriteItem]";
            return SqlHelper.ExecuteNonQuery(sql, null) == -1;
        }

        public bool Delete(object wheres)
        {
            List<SqlParameter> list = null;
            string str = wheres.parseWheres(out list);
            str = str == "" ? str : "where " + str;
            string sql = "DELETE FROM [dbo].[FavoriteItem] " + str; var res = SqlHelper.ExecuteNonQuery(sql, list.ToArray());
            return res > 0;
        }
        #endregion
        #region 删除一条记录 +bool Delete(long uuid)
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="uuid">主键</param>
        /// <returns>是否成功</returns>
        public bool Delete(long uuid)
        {
            const string sql = "DELETE FROM [dbo].[FavoriteItem] WHERE [uuid] = @uuid";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@uuid", uuid)) > 0;
        }
        #endregion
        #region 根据主键ID更新一条记录 +bool Update(FavoriteItem model)
        /// <summary>
        /// 根据主键更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>是否成功</returns>
        public bool Update(FavoriteItem model)
        {
            const string sql = @"UPDATE [dbo].[FavoriteItem] SET  num_iid=@num_iid,favorites_id=@favorites_id,title=@title,pict_url=@pict_url,small_images=@small_images,reserve_price=@reserve_price,zk_final_price=@zk_final_price,user_type=@user_type,provcity=@provcity,item_url=@item_url,click_url=@click_url,volume=@volume,tk_rate=@tk_rate,zk_final_price_wap=@zk_final_price_wap,event_start_time=@event_start_time,event_end_time=@event_end_time  WHERE [uuid] = @uuid";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@uuid", model.uuid.ToDBValue()), new SqlParameter("@num_iid", model.num_iid.ToDBValue()), new SqlParameter("@favorites_id", model.favorites_id.ToDBValue()), new SqlParameter("@title", model.title.ToDBValue()), new SqlParameter("@pict_url", model.pict_url.ToDBValue()), new SqlParameter("@small_images", model.small_images.ToDBValue()), new SqlParameter("@reserve_price", model.reserve_price.ToDBValue()), new SqlParameter("@zk_final_price", model.zk_final_price.ToDBValue()), new SqlParameter("@user_type", model.user_type.ToDBValue()), new SqlParameter("@provcity", model.provcity.ToDBValue()), new SqlParameter("@item_url", model.item_url.ToDBValue()), new SqlParameter("@click_url", model.click_url.ToDBValue()), new SqlParameter("@volume", model.volume.ToDBValue()), new SqlParameter("@tk_rate", model.tk_rate.ToDBValue()), new SqlParameter("@zk_final_price_wap", model.zk_final_price_wap.ToDBValue()), new SqlParameter("@event_start_time", model.event_start_time.ToDBValue()), new SqlParameter("@event_end_time", model.event_end_time.ToDBValue())) > 0;
        }
        #endregion
        #region 查询条数 +int QueryCount(object wheres)
        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="wheres">查询条件</param>
        /// <returns>条数</returns>
        public int QueryCount(object wheres)
        {
            List<SqlParameter> list = null;
            string str = wheres.parseWheres(out list);
            str = str == "" ? str : "where " + str;
            string sql = "SELECT COUNT(1) from FavoriteItem " + str; var res = SqlHelper.ExecuteScalar(sql, list.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion
        #region 查询单个模型实体 +FavoriteItem QuerySingleById(long uuid)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="id">uuid</param>);
        /// <returns>实体</returns>);
        public FavoriteItem QuerySingleById(long uuid)
        {
            const string sql = "SELECT TOP 1 uuid,num_iid,favorites_id,title,pict_url,small_images,reserve_price,zk_final_price,user_type,provcity,item_url,click_url,volume,tk_rate,zk_final_price_wap,event_start_time,event_end_time from FavoriteItem WHERE [uuid] = @uuid";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@uuid", uuid)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    FavoriteItem model = SqlHelper.MapEntity<FavoriteItem>(reader);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region 查询单个模型实体 +_User QuerySingleByIdX(string objectId,string columns){
        ///<summary>
        ///查询单个模型实体
        ///</summary>
        ///<param name=uuid>主键</param>);
        ///<returns>实体</returns>);
        public Dictionary<string, object> QuerySingleByIdX(long uuid, object columns)
        {
            Dictionary<string, string[]> li;
            string[] clumns = new String[] { "uuid", "num_iid", "favorites_id", "title", "pict_url", "small_images", "reserve_price", "zk_final_price", "user_type", "provcity", "item_url", "click_url", "volume", "tk_rate", "zk_final_price_wap", "event_start_time", "event_end_time" };
            string[] cls = columns.parseColumnsX(clumns, "FavoriteItem", out li);
            string sql = "SELECT TOP 1 " + string.Join(",", li["FavoriteItem"]) + " from FavoriteItem WHERE [objectId] = @objectId";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@uuid", uuid)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    Dictionary<string, object> model = SqlHelper.MapEntity(reader, cls);

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region 查询单个模型实体 +Users QuerySingleByWheres(object wheres)
        ///<summary>
        ///查询单个模型实体
        ///</summary>
        ///<param name="wheres">条件匿名类</param>
        ///<returns>实体</returns>
        public FavoriteItem QuerySingleByWheres(object wheres)
        {
            var list = QueryList(1, 1, wheres);
            return list != null && list.Any() ? list.FirstOrDefault() : null;
        }
        #endregion
        #region 查询单个模型列集合 +Dictionary<string, object> QuerySingleByWheresX(object wheres,object columns)
        ///<summary>
        ///查询单个模型实体
        ///</summary>
        ///<param name="wheres">条件</param>
        ///<param name="columns">列集合</param>
        ///<returns>实体</returns>
        public Dictionary<string, object> QuerySingleByWheresX(object wheres, object columns)
        {
            List<SqlParameter> list = null;
            string where = wheres.parseWheres(out list);
            where = string.IsNullOrEmpty(where) ? "" : " where " + where;
            Dictionary<string, string[]> li;
            string[] clumns = new String[] { "uuid", "num_iid", "favorites_id", "title", "pict_url", "small_images", "reserve_price", "zk_final_price", "user_type", "provcity", "item_url", "click_url", "volume", "tk_rate", "zk_final_price_wap", "event_start_time", "event_end_time" };
            string[] cls = columns.parseColumnsX(clumns, "FavoriteItem", out li);
            string sql = "SELECT TOP 1 " + string.Join(",", li["FavoriteItem"]) + " from FavoriteItem" + where;
            using (var reader = SqlHelper.ExecuteReader(sql, list.ToArray()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    Dictionary<string, object> model = SqlHelper.MapEntity(reader, cls);

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region 分页查询一个集合 +IEnumerable<Users> QueryList(int index, int size, object wheres=null, string orderField=id, bool isDesc = true)
        ///<summary>
        ///分页查询一个集合
        ///</summary>
        ///<param name="index">页码</param>
        ///<param name="size">页大小</param>
        ///<param name="wheres">条件匿名类</param>
        ///<param name="orderField">排序字段</param>
        ///<param name="isDesc">是否降序排序</param>
        ///<returns>实体集合</returns>
        public IEnumerable<FavoriteItem> QueryList(int index, int size, object wheres = null, string orderField = "uuid", bool isDesc = true)
        {
            List<SqlParameter> list = null;
            string where = wheres.parseWheres(out list);
            orderField = string.IsNullOrEmpty(orderField) ? "uuid" : orderField;
            var sql = SqlHelper.GenerateQuerySql("FavoriteItem", new string[] { "uuid", "num_iid", "favorites_id", "title", "pict_url", "small_images", "reserve_price", "zk_final_price", "user_type", "provcity", "item_url", "click_url", "volume", "tk_rate", "zk_final_price_wap", "event_start_time", "event_end_time" }, index, size, where, orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, list.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FavoriteItem model = SqlHelper.MapEntity<FavoriteItem>(reader);
                        yield return model;
                    }
                }
            }
        }
        #endregion
        #region 分页查询一个集合 +IEnumerable<Dictionary<string, object>> QueryListX(int index, int size, object columns = null, object wheres = null, string orderField=id, bool isDesc = true)
        ///<summary>
        ///分页查询一个集合
        ///</summary>
        ///<param name="index">页码</param>
        ///<param name="size">页大小</param>
        ///<param name="columns">指定的列</param>
        ///<param name="wheres">条件匿名类</param>
        ///<param name="orderField">排序字段</param>
        ///<param name="isDesc">是否降序排序</param>
        ///<returns>实体集合</returns>
        public IEnumerable<Dictionary<string, object>> QueryListX(int index, int size, object columns = null, object wheres = null, string orderField = "uuid", bool isDesc = true)
        {
            List<SqlParameter> list = null;
            string where = wheres.parseWheres(out list);
            orderField = string.IsNullOrEmpty(orderField) ? "uuid" : orderField;
            Dictionary<string, string[]> li;
            string[] clumns = new String[] { "uuid", "num_iid", "favorites_id", "title", "pict_url", "small_images", "reserve_price", "zk_final_price", "user_type", "provcity", "item_url", "click_url", "volume", "tk_rate", "zk_final_price_wap", "event_start_time", "event_end_time" };
            string[] cls = columns.parseColumnsX(clumns, "FavoriteItem", out li);
            var sql = SqlHelper.GenerateQuerySql("FavoriteItem", li["FavoriteItem"], index, size, where, orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, list.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> model = SqlHelper.MapEntity(reader, cls);

                        yield return model;
                    }
                }
            }
        }
        #endregion
        #region 根据主键修改指定列 +bool UpdateById(long uuid,object columns)
        /// <summary>
        /// 根据主键更新指定记录
        /// </summary>
        /// <param name="uuid">主键</param>
        /// <param name="columns">列集合对象</param>
        /// <returns>是否成功</returns>
        public bool UpdateById(long uuid, object columns)
        {
            List<SqlParameter> list = null;
            string[] column = columns.parseColumns(out list);
            list.Add(new SqlParameter("@uuid", uuid.ToDBValue()));
            string sql = string.Format(@"UPDATE [dbo].[FavoriteItem] SET  {0}  WHERE [{1}] = @{1}", string.Join(",", column), "uuid");
            return SqlHelper.ExecuteNonQuery(sql, list.ToArray()) > 0;
        }
        #endregion
        #region 根据条件更新记录+bool UpdateByWheres(object wheres, object columns)
        /// <summary>
        /// 根据条件更新记录
        /// </summary>
        /// <param name="wheres">条件集合实体实体</param>
        /// <param name="columns">列集合对象</param>
        /// <returns>是否成功</returns>
        public bool UpdateByWheres(object wheres, object columns)
        {
            List<SqlParameter> list = null;
            string[] column = columns.parseColumns(out list);
            List<SqlParameter> list1 = null;
            string where = wheres.parseWheres(out list1);
            where = string.IsNullOrEmpty(where) ? string.Empty : "where " + where;
            list.AddRange(list1);
            string sql = string.Format(@"UPDATE [dbo].[FavoriteItem] SET  {0} {1}", string.Join(",", column), where);
            return SqlHelper.ExecuteNonQuery(sql, list.ToArray()) > 0;
        }
        #endregion
    }
}