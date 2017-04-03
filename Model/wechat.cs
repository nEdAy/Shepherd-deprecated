using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class wechat
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string objectId { get; set; }
        /// <summary>
        /// 微信openId
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 邀请人openId
        /// </summary>
        public string inopenId { get; set; }
    }
}
