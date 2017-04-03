using System.Linq;
using System.Collections.Generic;
using MvcApplication1.DAO;
using MvcApplication1.Model;
namespace MvcApplication1.BLL
{
    public partial class TqgItemBLL
    {
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private TqgItemDAO _dao = new TqgItemDAO();
        #region 向数据库中添加一条记录 +bool; Insert(TqgItem model)
        /// <summary>
        /// 向数据库中添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体</param>
        /// <returns>是否成功</returns>
        public bool Insert(TqgItem model)
        {
            return _dao.Insert(model);
        }
        public bool InsertSome(List<TqgItem> list)
        {
            return _dao.InsertSome(list);
        }

        public bool DeleteAll()
        {
            return _dao.DeleteAll();
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
            return _dao.Delete(num_iid);
        }
        #endregion
        #region 根据主键ID更新一条记录 +bool Update(TqgItem model)
        /// <summary>
        /// 根据主键更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>执行结果受影响行数</returns>
        public bool Update(TqgItem model)
        {
            return _dao.Update(model);
        }
        #endregion
        #region 查询条数 +int QueryCount(object wheres)
        /// <summary>
        /// 查询条数
        /// </summary>
        /// <param name="wheres">查询条件</param>
        /// <returns>实体</returns>
        public int QueryCount(object wheres)
        {
            return _dao.QueryCount(wheres);
        }
        #endregion

        #region 分页查询一个集合 +IEnumerable<Users> QueryList(int index, int size, object wheres=null, string orderField=null, bool isDesc = false)
        ///<summary>
        ///分页查询一个集合
        ///</summary>
        ///<param name="index">页码</param>
        ///<param name="size">页大小</param>
        ///<param name="wheres">条件匿名类</param>
        ///<param name="orderField">排序字段</param>
        ///<param name="isDesc">是否降序排序</param>
        ///<returns>实体集合</returns>
        public IEnumerable<TqgItem> QueryList(int index, int size = 10, object wheres = null, string orderField = null, bool isDesc = false)
        {
            return _dao.QueryList(index, size, wheres, orderField, isDesc);
        }
        #endregion
        #region 分页查询一个集合 +IEnumerable<Users> QueryListX(int index, int size,object columns=null, object wheres=null, string orderField=null, bool isDesc = false)
        ///<summary>
        ///分页查询一个集合
        ///</summary>
        ///<param name="index">页码</param>
        ///<param name="size">页大小</param>
        ///<param name="wheres">条件匿名类</param>
        ///<param name="orderField">排序字段</param>
        ///<param name="isDesc">是否降序排序</param>
        ///<returns>实体集合</returns>
        public IEnumerable<Dictionary<string, object>> QueryListX(int index, int size = 10, object columns = null, object wheres = null, string orderField = null, bool isDesc = false)
        {
            return _dao.QueryListX(index, size, columns, wheres, orderField, isDesc);
        }
        #endregion
        #region 查询单个模型实体 +TqgItem QuerySingleByWheres(object wheres)
        ///<summary>
        ///查询单个模型实体
        ///</summary>
        ///<param name="wheres">条件匿名类</param>
        ///<returns>实体</returns>
        public TqgItem QuerySingleByWheres(object wheres)
        {
            return _dao.QuerySingleByWheres(wheres);
        }
        #endregion
        #region 查询单个模型实体 +Dictionary<string,object> QuerySingleByWheresX(object wheres,object columns)
        ///<summary>
        ///查询单个模型实体
        ///</summary>
        ///<param name="wheres">条件</param>
        ///<param name="columns">列集合</param>
        ///<returns>实体</returns>
        public Dictionary<string, object> QuerySingleByWheresX(object wheres, object columns)
        {
            return _dao.QuerySingleByWheresX(wheres, columns);
        }
        #endregion
        #region 查询单个模型实体 +TqgItem QuerySingleById(long num_iid)
        ///<summary>
        ///查询单个模型实体
        ///</summary>
        ///<param name="num_iid">key</param>
        ///<returns>实体</returns>
        public TqgItem QuerySingleById(long num_iid)
        {
            return _dao.QuerySingleById(num_iid);
        }
        #endregion
        #region 查询单个模型实体 +Dictionary<string,object> QuerySingleByIdX(long num_iid,object columns)///<summary>
        ///查询单个模型实体
        ///</summary>
        ///<param name=num_iid>key</param>
        ///<returns>实体</returns>
        ///<returns>实体</returns>
        public Dictionary<string, object> QuerySingleByIdX(long num_iid, object columns)
        {
            return _dao.QuerySingleByIdX(num_iid, columns);
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
            return _dao.UpdateById(num_iid, columns);
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
            return _dao.UpdateByWheres(wheres, columns);
        }
        #endregion
    }
}