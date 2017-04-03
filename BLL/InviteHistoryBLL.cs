using MvcApplication1.DAO;
using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.BLL
{
    public class InviteHistoryBLL
    {
        private InviteHistoryDAO _dao = new InviteHistoryDAO();
        public bool QueryExitByUsername(string username)
        {
            List<Wheres> list = new List<Wheres>();
            Wheres wh = new Wheres();
            wh.setField("username", "=", username, "");
            list.Add(wh);
            return _dao.QueryCount(wh) > 0;
        }

        

    }
}
