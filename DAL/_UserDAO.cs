using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using MvcApplication1.Model;
using MvcApplication1.Utility;
using Model;

namespace MvcApplication1.DAO
{
    public partial class _UserDAO
    {
        #region 向数据库中添加一条记录 +bool Insert(_User model)
        /// <summary>
        /// 手机端，没有邀请人注册
        /// </summary>
        /// <param name="model"></param>
        /// <param name="history"></param>
        /// <returns></returns>
        public bool Insert1(_User model,CreditsHistory history)
        {
            const string sql = @"INSERT INTO [dbo].[_User] (objectId,updatedAt,createdAt,username,password,transaction_password,sessionToken,nickname,credit,overage,avatar,sign_in,shake_times) VALUES (@objectId,@updatedAt,@createdAt,@username,@password,@transaction_password,@sessionToken,@nickname,@credit,@overage,@avatar,@sign_in,@shake_times)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@username", model.username.ToDBValue()), new SqlParameter("@password", model.password.ToDBValue()), new SqlParameter("@transaction_password", model.transaction_password.ToDBValue()), new SqlParameter("@sessionToken", model.sessionToken.ToDBValue()), new SqlParameter("@nickname", model.nickname.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()), new SqlParameter("@overage", model.overage.ToDBValue()), new SqlParameter("@avatar", model.avatar.ToDBValue()), new SqlParameter("@sign_in", model.sign_in.ToDBValue()), new SqlParameter("@shake_times", model.shake_times.ToDBValue()) };
            const string sql1 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms1 = { new SqlParameter("@objectId", history.objectId), new SqlParameter("@createdAt", history.createdAt), new SqlParameter("@updatedAt", history.updatedAt), new SqlParameter("@userId", model.objectId), new SqlParameter("@type", history.type), new SqlParameter("@change", history.change), new SqlParameter("@credit", history.credit) };
            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, parms1);
            return res > 1;
        }
        /// <summary>
        /// 手机端有邀请人注册
        /// </summary>
        /// <param name="model"></param>
        /// <param name="history"></param>
        /// <param name="history1"></param>
        /// <returns></returns>
        public bool Insert1(_User model, CreditsHistory history,CreditsHistory history1,string inviteCode)
        {
            const string sql = @"INSERT INTO [dbo].[_User] (objectId,updatedAt,createdAt,username,password,transaction_password,sessionToken,nickname,credit,overage,avatar,sign_in,shake_times) VALUES (@objectId,@updatedAt,@createdAt,@username,@password,@transaction_password,@sessionToken,@nickname,@credit,@overage,@avatar,@sign_in,@shake_times)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@username", model.username.ToDBValue()), new SqlParameter("@password", model.password.ToDBValue()), new SqlParameter("@transaction_password", model.transaction_password.ToDBValue()), new SqlParameter("@sessionToken", model.sessionToken.ToDBValue()), new SqlParameter("@nickname", model.nickname.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()), new SqlParameter("@overage", model.overage.ToDBValue()), new SqlParameter("@avatar", model.avatar.ToDBValue()), new SqlParameter("@sign_in", model.sign_in.ToDBValue()), new SqlParameter("@shake_times", model.shake_times.ToDBValue()) };
            const string sql1 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms1 = { new SqlParameter("@objectId", history.objectId), new SqlParameter("@createdAt", history.createdAt), new SqlParameter("@updatedAt", history.updatedAt), new SqlParameter("@userId", model.objectId), new SqlParameter("@type", history.type), new SqlParameter("@change", history.change), new SqlParameter("@credit", history.credit) };
            const string sql2 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms2 = { new SqlParameter("@objectId", history1.objectId), new SqlParameter("@createdAt", history1.createdAt), new SqlParameter("@updatedAt", history1.updatedAt), new SqlParameter("@userId", history1.userId), new SqlParameter("@type", history1.type), new SqlParameter("@change", history1.change), new SqlParameter("@credit", history1.credit) };
            const string sql3 = @"INSERT INTO InviteHistory(username,inUsername,createdAt) values (@username,@inUsername,@createdAt)";
            SqlParameter[] parms3 = { new SqlParameter("@username", model.username), new SqlParameter("@inUsername", inviteCode), new SqlParameter("createdAt", DateTime.Now.ToString()) };
            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, parms1,sql2,parms2,sql3,parms3);
            return res > 3;
        }

        //没有邀请人
        public bool Insert(_User model, CreditsHistory history)
        {
            const string sql = @"INSERT INTO [dbo].[wechat] (objectId,openId,inopenId) VALUES (@objectId,@openId,@inopenId)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.authData.wechat.objectId.ToDBValue()), new SqlParameter("@openId", model.authData.wechat.openId.ToDBValue()), new SqlParameter("@inopenId", model.authData.wechat.inopenId.ToDBValue()) };

            const string sql1 = @"INSERT INTO [dbo].[authData] (objectId,wechatId) VALUES (@objectId,@wechatId)";
            //SqlParameter[] parms1 = { new SqlParameter("@objectId", model.authData.objectId.ToDBValue()), new SqlParameter("@weiboId", model.authData.weibo.objectId.ToDBValue()), new SqlParameter("@qqId", model.authData.qq.objectId.ToDBValue()), new SqlParameter("@alibabaId", model.authData.alibaba.objectId.ToDBValue()), new SqlParameter("@wechatId", model.authData.wechat.objectId.ToDBValue()) };
            SqlParameter[] parms1 = { new SqlParameter("@objectId", model.authData.objectId.ToDBValue()), new SqlParameter("@wechatId", model.authData.wechat.objectId.ToDBValue()) };

            const string sql2 = @"INSERT INTO [dbo].[_User] (objectId,updatedAt,createdAt,username,password,transaction_password,sessionToken,nickname,credit,overage,avatar,sign_in,shake_times,authDataId) VALUES (@objectId,@updatedAt,@createdAt,@username,@password,@transaction_password,@sessionToken,@nickname,@credit,@overage,@avatar,@sign_in,@shake_times,@authDataId)";
            SqlParameter[] parms2 = { new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@username", model.username.ToDBValue()), new SqlParameter("@password", model.password.ToDBValue()), new SqlParameter("@transaction_password", model.transaction_password.ToDBValue()), new SqlParameter("@sessionToken", model.sessionToken.ToDBValue()), new SqlParameter("@nickname", model.nickname.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()), new SqlParameter("@overage", model.overage.ToDBValue()), new SqlParameter("@avatar", model.avatar.ToDBValue()), new SqlParameter("@sign_in", model.sign_in.ToDBValue()), new SqlParameter("@shake_times", model.shake_times.ToDBValue()), new SqlParameter("@authDataId", model.authData.objectId.ToDBValue()) };            

            const string sql3 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms3 = { new SqlParameter("@objectId", history.objectId), new SqlParameter("@createdAt", history.createdAt), new SqlParameter("@updatedAt", history.updatedAt), new SqlParameter("@userId", model.objectId), new SqlParameter("@type", history.type), new SqlParameter("@change", history.change), new SqlParameter("@credit", history.credit) };           

            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, parms1, sql2, parms2, sql3, parms3);
            return res > 3;
        }

        /// <summary>
        /// 有邀请人
        /// </summary>
        /// <param name="model"></param>
        /// <param name="history">注册用户记录</param>
        /// <param name="history1">邀请者记录</param>       
        /// <returns></returns>
        public bool Insert(_User model,CreditsHistory history, CreditsHistory history1)
        {
            const string sql = @"INSERT INTO [dbo].[wechat] (objectId,openId,inopenId) VALUES (@objectId,@openId,@inopenId)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.authData.wechat.objectId.ToDBValue()), new SqlParameter("@openId", model.authData.wechat.openId.ToDBValue()), new SqlParameter("@inopenId", model.authData.wechat.inopenId.ToDBValue()) };

            const string sql1 = @"INSERT INTO [dbo].[authData] (objectId,wechatId) VALUES (@objectId,@wechatId)";
            //SqlParameter[] parms1 = { new SqlParameter("@objectId", model.authData.objectId.ToDBValue()), new SqlParameter("@weiboId", model.authData.weibo.objectId.ToDBValue()), new SqlParameter("@qqId", model.authData.qq.objectId.ToDBValue()), new SqlParameter("@alibabaId", model.authData.alibaba.objectId.ToDBValue()), new SqlParameter("@wechatId", model.authData.wechat.objectId.ToDBValue()) };
            SqlParameter[] parms1 = { new SqlParameter("@objectId", model.authData.objectId.ToDBValue()), new SqlParameter("@wechatId", model.authData.wechat.objectId.ToDBValue()) };

            const string sql2 = @"INSERT INTO [dbo].[_User] (objectId,updatedAt,createdAt,username,password,transaction_password,sessionToken,nickname,credit,overage,avatar,sign_in,shake_times,authDataId) VALUES (@objectId,@updatedAt,@createdAt,@username,@password,@transaction_password,@sessionToken,@nickname,@credit,@overage,@avatar,@sign_in,@shake_times,@authDataId)";
            SqlParameter[] parms2 = { new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@username", model.username.ToDBValue()), new SqlParameter("@password", model.password.ToDBValue()), new SqlParameter("@transaction_password", model.transaction_password.ToDBValue()), new SqlParameter("@sessionToken", model.sessionToken.ToDBValue()), new SqlParameter("@nickname", model.nickname.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()), new SqlParameter("@overage", model.overage.ToDBValue()), new SqlParameter("@avatar", model.avatar.ToDBValue()), new SqlParameter("@sign_in", model.sign_in.ToDBValue()), new SqlParameter("@shake_times", model.shake_times.ToDBValue()), new SqlParameter("@authDataId", model.authData.objectId.ToDBValue()) };

            const string sql3 = @"UPDATE _User set credit+=@credit where username=@inopenId";
            SqlParameter[] parms3 = { new SqlParameter("@credit", history1.change), new SqlParameter("@inopenId", model.authData.wechat.inopenId) };

            const string sql4 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms4 = { new SqlParameter("@objectId", history.objectId), new SqlParameter("@createdAt", history.createdAt), new SqlParameter("@updatedAt", history.updatedAt),new SqlParameter("@userId",model.objectId), new SqlParameter("@type", history.type), new SqlParameter("@change", history.change),new SqlParameter("@credit",history.credit) };

            const string sql5 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms5 = { new SqlParameter("@objectId", history1.objectId), new SqlParameter("@createdAt", history1.createdAt), new SqlParameter("@updatedAt", history1.updatedAt), new SqlParameter("@userId", history1.userId), new SqlParameter("@type", history1.type), new SqlParameter("@change", history1.change), new SqlParameter("@credit", history1.credit) };

            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, parms1, sql2, parms2,sql3,parms3,sql4,parms4,sql5,parms5);
            return res > 5;
        }





        /// <summary>
        /// 微信绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateInsert(_User model,CreditsHistory history)
        {
            const string sql = @"INSERT INTO [dbo].[wechat] (objectId,openId,inopenId) VALUES (@objectId,@openId,@inopenId)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.authData.wechat.objectId.ToDBValue()), new SqlParameter("@openId", model.authData.wechat.openId.ToDBValue()), new SqlParameter("@inopenId", model.authData.wechat.inopenId.ToDBValue()) };

            const string sql1 = @"INSERT INTO [dbo].[authData] (objectId,wechatId) VALUES (@objectId,@wechatId)";
            //SqlParameter[] parms1 = { new SqlParameter("@objectId", model.authData.objectId.ToDBValue()), new SqlParameter("@weiboId", model.authData.weibo.objectId.ToDBValue()), new SqlParameter("@qqId", model.authData.qq.objectId.ToDBValue()), new SqlParameter("@alibabaId", model.authData.alibaba.objectId.ToDBValue()), new SqlParameter("@wechatId", model.authData.wechat.objectId.ToDBValue()) };
            SqlParameter[] parms1 = { new SqlParameter("@objectId", model.authData.objectId.ToDBValue()), new SqlParameter("@wechatId", model.authData.wechat.objectId.ToDBValue()) };

            const string sql2 = @"UPDATE _User set authDataId=@authDataId where username=@username";
            SqlParameter[] parms2 = { new SqlParameter("authDataId", model.authData.objectId), new SqlParameter("@username", model.username) };

            const string sql3 = @"UPDATE _User set credit+=@credit where username=@inopenId";
            SqlParameter[] parms3 = { new SqlParameter("@credit", history.change), new SqlParameter("@inopenId", model.authData.wechat.inopenId) };

            const string sql4 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms4 = { new SqlParameter("@objectId", history.objectId), new SqlParameter("@createdAt", history.createdAt), new SqlParameter("@updatedAt", history.updatedAt), new SqlParameter("@userId", history.userId), new SqlParameter("@type", history.type), new SqlParameter("@change", history.change), new SqlParameter("@credit", history.credit) };            

            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, parms1, sql2, parms2, sql3, parms3,sql4,parms4);
            return res > 4;
        }

        /// <summary>
        /// 微信绑定，无邀请码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateInsert1(_User model)
        {
            const string sql = @"INSERT INTO [dbo].[wechat] (objectId,openId) VALUES (@objectId,@openId)";
            SqlParameter[] parms = { new SqlParameter("@objectId", model.authData.wechat.objectId.ToDBValue()), new SqlParameter("@openId", model.authData.wechat.openId.ToDBValue()) };

            const string sql1 = @"INSERT INTO [dbo].[authData] (objectId,wechatId) VALUES (@objectId,@wechatId)";            
            SqlParameter[] parms1 = { new SqlParameter("@objectId", model.authData.objectId.ToDBValue()), new SqlParameter("@wechatId", model.authData.wechat.objectId.ToDBValue()) };

            const string sql2 = @"UPDATE _User set authDataId=@authDataId where username=@username";
            SqlParameter[] parms2 = { new SqlParameter("authDataId", model.authData.objectId), new SqlParameter("@username", model.username) };            

            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, parms1, sql2, parms2);
            return res > 2;
        }

        /// <summary>
        /// 根据username查询用户是否存在绑定信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int QueryBandByUsername(string username)
        {
            string sql = "select COUNT(1) from _User,authData,wechat where _User.authDataId=authData.objectId and authData.wechatId=wechat.objectId and username = @username";
            SqlParameter[] parms = { new SqlParameter("@username", username) };
            var res = SqlHelper.ExecuteScalar(sql, parms);
            return res == null ? 0 : Convert.ToInt32(res);
        }

        #endregion
        #region 删除一条记录 +bool Delete(string objectId)
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="objectId">主键</param>
        /// <returns>是否成功</returns>
        public bool Delete(string objectId)
        {
            const string sql = "DELETE FROM [dbo].[_User] WHERE [objectId] = @objectId";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@objectId", objectId)) > 0;
        }
        #endregion
        #region 根据主键ID更新一条记录 +bool Update(_User model)
        /// <summary>
        /// 根据主键更新一条记录
        /// </summary>
        /// <param name="model">更新后的实体</param>
        /// <returns>是否成功</returns>
        public bool Update(_User model)
        {
            const string sql = @"UPDATE [dbo].[_User] SET  updatedAt=@updatedAt,createdAt=@createdAt,username=@username,password=@password,transaction_password=@transaction_password,sessionToken=@sessionToken,nickname=@nickname,credit=@credit,overage=@overage,avatar=@avatar,sign_in=@sign_in,shake_times=@shake_times,authDataId=@authDataId  WHERE [objectId] = @objectId";
            return SqlHelper.ExecuteNonQuery(sql, new SqlParameter("@objectId", model.objectId.ToDBValue()), new SqlParameter("@updatedAt", model.updatedAt.ToDBValue()), new SqlParameter("@createdAt", model.createdAt.ToDBValue()), new SqlParameter("@username", model.username.ToDBValue()), new SqlParameter("@password", model.password.ToDBValue()), new SqlParameter("@transaction_password", model.transaction_password.ToDBValue()), new SqlParameter("@sessionToken", model.sessionToken.ToDBValue()), new SqlParameter("@nickname", model.nickname.ToDBValue()), new SqlParameter("@credit", model.credit.ToDBValue()), new SqlParameter("@overage", model.overage.ToDBValue()), new SqlParameter("@avatar", model.avatar.ToDBValue()), new SqlParameter("@sign_in", model.sign_in.ToDBValue()), new SqlParameter("@shake_times", model.shake_times.ToDBValue()), new SqlParameter("@authDataId", model.authData.objectId.ToDBValue())) > 0;
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
            string sql = "SELECT COUNT(1) from _User " + str; var res = SqlHelper.ExecuteScalar(sql, list.ToArray());
            return res == null ? 0 : Convert.ToInt32(res);
        }
        #endregion
        #region 查询单个模型实体 +_User QuerySingleById(string objectId)
        /// <summary>
        /// 查询单个模型实体
        /// </summary>
        /// <param name="id">objectId</param>);
        /// <returns>实体</returns>);
        public _User QuerySingleById(string objectId)
        {
            ///const string sql = "SELECT TOP 1 objectId,updatedAt,createdAt,username,password,transaction_password,sessionToken,nickname,credit,overage,avatar,sign_in,shake_times,authDataId from _User WHERE [objectId] = @objectId";
            const string sql = "SELECT TOP 1 objectId,updatedAt,createdAt,username,sessionToken,nickname,credit,overage,avatar,sign_in,shake_times,authDataId from _User WHERE [objectId] = @objectId";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@objectId", objectId)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    _User model = SqlHelper.MapEntity<_User>(reader);
                    if (reader["authDataId"] != DBNull.Value)
                    {
                        authDataDAO authDataDAO = new authDataDAO();
                        model.authData = authDataDAO.QuerySingleById((string)reader["authDataId"]);
                    }
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
        ///<param name=objectId>主键</param>);
        ///<returns>实体</returns>);
        public Dictionary<string, object> QuerySingleByIdX(string objectId, object columns)
        {
            Dictionary<string, string[]> li;
            string[] clumns = new String[] { "objectId", "updatedAt", "createdAt", "username", "password", "transaction_password", "sessionToken", "nickname", "credit", "overage", "avatar", "sign_in", "shake_times" };
            string[] cls = columns.parseColumnsX(clumns, "_User", out li);
            string sql = "SELECT TOP 1 " + string.Join(",", li["_User"]) + " from _User WHERE [objectId] = @objectId";
            using (var reader = SqlHelper.ExecuteReader(sql, new SqlParameter("@objectId", objectId)))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    Dictionary<string, object> model = SqlHelper.MapEntity(reader, cls);
                    if (li.ContainsKey("authData"))
                    {
                        if (reader["authDataId"] != DBNull.Value)
                        {
                            authDataDAO authDataDAO = new authDataDAO();
                            model["authData"] = authDataDAO.QuerySingleByIdX((string)reader["authDataId"], li["authData"]);
                        }
                        else
                        {
                            model["authData"] = null;
                        }
                    }

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
        public _User QuerySingleByWheres(object wheres)
        {
            var list = QueryList(1, 1, wheres);
            return list != null && list.Any() ? list.FirstOrDefault() : null;
        }

        ///// <summary>
        ///// 根据openId修改积分
        ///// </summary>
        ///// <param name="openId">OpenId</param>
        ///// <param name="credit">修改积分</param>
        ///// <returns></returns>
        //public bool UpdateCreditByOpenId(string openId,int credit)
        //{
        //    string sql = "UPDATE _User set credit+=@credit from _User,authData,wechat where _User.authDataId=authData.objectId and authData.wechatId=wechat.objectId and openId=@openId";
        //    SqlParameter[] paras = { new SqlParameter("@credit", credit), new SqlParameter("openId", openId) };
        //    int res = SqlHelper.ExecuteNonQuery(sql, paras);
        //    return res > 0;
        //}

        public bool UpdateCreditByObjectId(string objectId, CreditsHistory history)
        {
            const string sql = @"UPDATE _User set credit+=@credit where objectId = @objectId";
            SqlParameter[] parms = { new SqlParameter("@credit",history.change), new SqlParameter("@objectId", objectId) };

            const string sql1 = @"INSERT INTO CreditsHistory (objectId,createdAt,updatedAt,userId,type,change,credit) values (@objectId,@createdAt,@updatedAt,@userId,@type,@change,@credit)";
            SqlParameter[] parms1 = { new SqlParameter("@objectId", history.objectId), new SqlParameter("@createdAt", history.createdAt), new SqlParameter("@updatedAt", history.updatedAt), new SqlParameter("@userId", objectId), new SqlParameter("@type", history.type), new SqlParameter("@change", history.change), new SqlParameter("@credit", history.credit) };

            int res = SqlHelper.ExecuteNonQuerysTransaction(sql, parms, sql1, parms1);
            return res > 1;
        }

        public bool QueryExit(string username,string openId)
        {
            string sql = "select * from _User,authData,wechat where _User.authDataId=authData.objectId and authData.wechatId=wechat.objectId and username = @username and openId=@openId";
            SqlParameter[] paras = { new SqlParameter("@username", username), new SqlParameter("openId", openId) };
            int res = SqlHelper.ExcuteCounter(sql, paras);
            return res > 0;
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
            string[] clumns = new String[] { "objectId", "updatedAt", "createdAt", "username", "password", "transaction_password", "sessionToken", "nickname", "credit", "overage", "avatar", "sign_in", "shake_times" };
            string[] cls = columns.parseColumnsX(clumns, "_User", out li);
            string sql = "SELECT TOP 1 " + string.Join(",", li["_User"]) + " from _User" + where;
            using (var reader = SqlHelper.ExecuteReader(sql, list.ToArray()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    Dictionary<string, object> model = SqlHelper.MapEntity(reader, cls);
                    if (li.ContainsKey("authData"))
                    {
                        if (reader["authDataId"] != DBNull.Value)
                        {
                            authDataDAO authDataDAO = new authDataDAO();
                            model["authData"] = authDataDAO.QuerySingleByIdX((string)reader["authDataId"], li["authData"]);
                        }
                        else
                        {
                            model["authData"] = null;
                        }
                    }

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
        public IEnumerable<_User> QueryList(int index, int size, object wheres = null, string orderField = "objectId", bool isDesc = true)
        {
            List<SqlParameter> list = null;
            string where = wheres.parseWheres(out list);
            orderField = string.IsNullOrEmpty(orderField) ? "objectId" : orderField;
            var sql = SqlHelper.GenerateQuerySql("_User", new string[] { "objectId", "updatedAt", "createdAt", "username", "password", "transaction_password", "sessionToken", "nickname", "credit", "overage", "avatar", "sign_in", "shake_times", "authDataId" }, index, size, where, orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, list.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _User model = SqlHelper.MapEntity<_User>(reader);
                        if (reader["authDataId"] != DBNull.Value)
                        {
                            authDataDAO authDataDAO = new authDataDAO();
                            model.authData = authDataDAO.QuerySingleById((string)reader["authDataId"]);
                        }
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
        public IEnumerable<Dictionary<string, object>> QueryListX(int index, int size, object columns = null, object wheres = null, string orderField = "objectId", bool isDesc = true)
        {
            List<SqlParameter> list = null;
            string where = wheres.parseWheres(out list);
            orderField = string.IsNullOrEmpty(orderField) ? "objectId" : orderField;
            Dictionary<string, string[]> li;
            string[] clumns = new String[] { "objectId", "updatedAt", "createdAt", "username", "password", "transaction_password", "sessionToken", "nickname", "credit", "overage", "avatar", "sign_in", "shake_times" };
            string[] cls = columns.parseColumnsX(clumns, "_User", out li);
            var sql = SqlHelper.GenerateQuerySql("_User", li["_User"], index, size, where, orderField, isDesc);
            using (var reader = SqlHelper.ExecuteReader(sql, list.ToArray()))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> model = SqlHelper.MapEntity(reader, cls);
                        if (li.ContainsKey("authData"))
                        {
                            if (reader["authDataId"] != DBNull.Value)
                            {
                                authDataDAO authDataDAO = new authDataDAO();
                                model["authData"] = authDataDAO.QuerySingleByIdX((string)reader["authDataId"], li["authData"]);
                            }
                            else
                            {
                                model["authData"] = null;
                            }
                        }

                        yield return model;
                    }
                }
            }
        }
        #endregion
        #region 根据主键修改指定列 +bool UpdateById(string objectId,object columns)
        /// <summary>
        /// 根据主键更新指定记录
        /// </summary>
        /// <param name="objectId">主键</param>
        /// <param name="columns">列集合对象</param>
        /// <returns>是否成功</returns>
        public bool UpdateById(string objectId, object columns)
        {
            List<SqlParameter> list = null;
            string[] column = columns.parseColumns(out list);
            list.Add(new SqlParameter("@objectId", objectId.ToDBValue()));
            string sql = string.Format(@"UPDATE [dbo].[_User] SET  {0}  WHERE [{1}] = @{1}", string.Join(",", column), "objectId");
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
            string sql = string.Format(@"UPDATE [dbo].[_User] SET  {0} {1}", string.Join(",", column), where);
            return SqlHelper.ExecuteNonQuery(sql, list.ToArray()) > 0;
        }
        #endregion
    }
}