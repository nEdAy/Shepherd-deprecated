using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using System.Text;

namespace MvcApplication1.DAO
{
    public partial class TqgItemDAO
    {
        #region 向数据库中添加一条记录 +bool Insert(TqgItem model)
        ///<summary>
        ///向数据库中添加一条记录
        ///</summary>
        ///<param name="model">要添加的实体</param>
        public bool Insert(TqgItem model)
        {
            const string sql = @"INSERT INTO [dbo].[TqgItem] (num_iid,title,total_amount,click_url,category_name,zk_final_price,end_time,sold_num,start_time,reserve_price,pic_url) VALUES (@num_iid,@title,@total_amount,@click_url,@category_name,@zk_final_price,@end_time,@sold_num,@start_time,@reserve_price,@pic_url)";
            int res = SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@num_iid", model.num_iid.ToDBValue()), new SqlParameter("@title", model.title.ToDBValue()), new SqlParameter("@total_amount", model.total_amount.ToDBValue()), new SqlParameter("@click_url", model.click_url.ToDBValue()), new SqlParameter("@category_name", model.category_name.ToDBValue()), new SqlParameter("@zk_final_price", model.zk_final_price.ToDBValue()), new SqlParameter("@end_time", model.end_time.ToDBValue()), new SqlParameter("@sold_num", model.sold_num.ToDBValue()), new SqlParameter("@start_time", model.start_time.ToDBValue()), new SqlParameter("@reserve_price", model.reserve_price.ToDBValue()), new SqlParameter("@pic_url", model.pic_url.ToDBValue()));
            return res > 0;
        }

        public bool InsertSome(List<TqgItem> itemList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"INSERT INTO [dbo].[TqgItem] (num_iid,title,total_amount,click_url,category_name,zk_final_price,end_time,sold_num,start_time,reserve_price,pic_url) VALUES ");
            int length = itemList.Count();
            for (int i = 0; i < length; i++)
            {
                sb.Append("(@num_iid_" + i);
                sb.Append(",@title_" + i);
                sb.Append(",@total_amount_" + i);
                sb.Append(",@click_url_" + i);
                sb.Append(",@category_name_" + i);
                sb.Append(",@zk_final_price_" + i);
                sb.Append(",@end_time_" + i);
                sb.Append(",@sold_num_" + i);
                sb.Append(",@start_time_" + i);
                sb.Append(",@reserve_price_" + i);
                if (i == length - 1)
                    sb.Append(",@pic_url_" + i + ")");
                else
                    sb.Append(",@pic_url_" + i + "),");
            }
            string sql = sb.ToString();
            List<SqlParameter> param = new List<SqlParameter>();
            int j = 0;
            foreach (TqgItem model in itemList)
            {
                param.Add(new SqlParameter("@num_iid_" + j, model.num_iid.ToDBValue()));
                param.Add(new SqlParameter("@title_" + j, model.title.ToDBValue()));
                param.Add(new SqlParameter("@total_amount_" + j, model.total_amount.ToDBValue()));
                param.Add(new SqlParameter("@click_url_" + j, model.click_url.ToDBValue()));
                param.Add(new SqlParameter("@category_name_" + j, model.category_name.ToDBValue()));
                param.Add(new SqlParameter("@zk_final_price_" + j, model.zk_final_price.ToDBValue()));
                param.Add(new SqlParameter("@end_time_" + j, model.end_time.ToDBValue()));
                param.Add(new SqlParameter("@sold_num_" + j, model.sold_num.ToDBValue()));
                param.Add(new SqlParameter("@start_time_" + j, model.start_time.ToDBValue()));
                param.Add(new SqlParameter("@reserve_price_" + j, model.reserve_price.ToDBValue()));
                param.Add(new SqlParameter("@pic_url_" + j, model.pic_url.ToDBValue()));
                j++;
            }
            int res = SqlHelper.ExecuteNonQuery(sql, param.ToArray());
            return res > 0;
        }
        #endregion
        #region 删除一条记录 +bool Delete(long num_iid)
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="num_iid">主键</param>
        /// <returns>是否成功</returns>
        public bool Delete(long num_iid)
        {
            const string sql = "DELETE FROM [dbo].[TqgItem] WHERE [num_iid] = @num_iid";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@num_iid", num_iid)) > 0;
        }
        /// <summary>
        /// 删除所有记录
        /// </summary>
        /// <returns>是否成功</returns>
        public bool DeleteAll()
        {
            const string sql = "truncate table [dbo].[TqgItem]";
            //const string sql = "DELETE FROM [dbo].[FavoriteItem]";
            return SqlHelper.ExecuteNonQuery(sql, null) == -1;
        }
        #endregion
        #region 根据主键ID更新一条记录 +bool Update(TqgItem model)
        /// <summary>
        /// 根据主键更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>是否成功</returns>
        public bool Update(TqgItem model)
        {
            const string sql = @"UPDATE [dbo].[TqgItem] SET  title=@title,total_amount=@total_amount,click_url=@click_url,category_name=@category_name,zk_final_price=@zk_final_price,end_time=@end_time,sold_num=@sold_num,start_time=@start_time,reserve_price=@reserve_price,pic_url=@pic_url  WHERE [num_iid] = @num_iid";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@num_iid", model.num_iid.ToDBValue()), new SqlParameter("@title", model.title.ToDBValue()), new SqlParameter("@total_amount", model.total_amount.ToDBValue()), new SqlParameter("@click_url", model.click_url.ToDBValue()), new SqlParameter("@category_name", model.category_name.ToDBValue()), new SqlParameter("@zk_final_price", model.zk_final_price.ToDBValue()), new SqlParameter("@end_time", model.end_time.ToDBValue()), new SqlParameter("@sold_num", model.sold_num.ToDBValue()), new SqlParameter("@start_time", model.start_time.ToDBValue()), new SqlParameter("@reserve_price", model.reserve_price.ToDBValue()), new SqlParameter("@pic_url", model.pic_url.ToDBValue())) > 0;
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
            string sql = "SELECT COUNT(1) from TqgItem " + str; var res = SqlHelper.ExecuteScalar(sql, list.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion
        #region 查询单个模型实体 +TqgItem QuerySingleById(long num_iid)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="id">num_iid</param>);
        /// <returns>实体</returns>);
        public TqgItem QuerySingleById(long num_iid)
        {
            const string sql = "SELECT TOP 1 num_iid,title,total_amount,click_url,category_name,zk_final_price,end_time,sold_num,start_time,reserve_price,pic_url from TqgItem WHERE [num_iid] = @num_iid";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@num_iid", num_iid)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    TqgItem model = SqlHelper.MapEntity<TqgItem>(reader);
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
        ///<param name=num_iid>主键</param>);
        ///<returns>实体</returns>);
        public Dictionary<string, object> QuerySingleByIdX(long num_iid, object columns)
        {
            Dictionary<string, string[]> li;
            string[] clumns = new String[] { "num_iid", "title", "total_amount", "click_url", "category_name", "zk_final_price", "end_time", "sold_num", "start_time", "reserve_price", "pic_url" };
            string[] cls = columns.parseColumnsX(clumns, "TqgItem", out li);
            string sql = "SELECT TOP 1 " + string.Join(",", li["TqgItem"]) + " from TqgItem WHERE [objectId] = @objectId";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@num_iid", num_iid)))
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
        public TqgItem QuerySingleByWheres(object wheres)
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
            string[] clumns = new String[] { "num_iid", "title", "total_amount", "click_url", "category_name", "zk_final_price", "end_time", "sold_num", "start_time", "reserve_price", "pic_url" };
            string[] cls = columns.parseColumnsX(clumns, "TqgItem", out li);
            string sql = "SELECT TOP 1 " + string.Join(",", li["TqgItem"]) + " from TqgItem" + where;
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
        public IEnumerable<TqgItem> QueryList(int index, int size, object wheres = null, string orderField = "uuid", bool isDesc = true)
        {
            List<SqlParameter> list = null;
            string where = wheres.parseWheres(out list);
            orderField = string.IsNullOrEmpty(orderField) ? "uuid" : orderField;
            var sql = SqlHelper.GenerateQuerySql("TqgItem", new string[] { "num_iid", "title", "total_amount", "click_url", "category_name", "zk_final_price", "end_time", "sold_num", "start_time", "reserve_price", "pic_url" }, index, size, where, orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, list.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TqgItem model = SqlHelper.MapEntity<TqgItem>(reader);
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
        public IEnumerable<Dictionary<string, object>> QueryListX(int index, int size, object columns = null, object wheres = null, string orderField = "num_iid", bool isDesc = true)
        {
            List<SqlParameter> list = null;
            string where = wheres.parseWheres(out list);
            orderField = string.IsNullOrEmpty(orderField) ? "num_iid" : orderField;
            Dictionary<string, string[]> li;
            string[] clumns = new String[] { "num_iid", "title", "total_amount", "click_url", "category_name", "zk_final_price", "end_time", "sold_num", "start_time", "reserve_price", "pic_url" };
            string[] cls = columns.parseColumnsX(clumns, "TqgItem", out li);
            var sql = SqlHelper.GenerateQuerySql("TqgItem", li["TqgItem"], index, size, where, orderField, isDesc);
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
        #region 根据主键修改指定列 +bool UpdateById(long num_iid,object columns)
        /// <summary>
        /// 根据主键更新指定记录
        /// </summary>
        /// <param name="num_iid">主键</param>
        /// <param name="columns">列集合对象</param>
        /// <returns>是否成功</returns>
        public bool UpdateById(long num_iid, object columns)
        {
            List<SqlParameter> list = null;
            string[] column = columns.parseColumns(out list);
            list.Add(new SqlParameter("@num_iid", num_iid.ToDBValue()));
            string sql = string.Format(@"UPDATE [dbo].[TqgItem] SET  {0}  WHERE [{1}] = @{1}", string.Join(",", column), "num_iid");
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
            string sql = string.Format(@"UPDATE [dbo].[TqgItem] SET  {0} {1}", string.Join(",", column), where);
            return SqlHelper.ExecuteNonQuery(sql, list.ToArray()) > 0;
        }
        #endregion
    }
}