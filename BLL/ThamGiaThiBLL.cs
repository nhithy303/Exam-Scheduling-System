using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ThamGiaThiBLL
    {
        ThamGiaThiDAL tgt_dal = new ThamGiaThiDAL();
        public ThamGiaThi[] GetList(string condition)
        {
            return tgt_dal.GetList(condition);
        }
        public void Insert(ThamGiaThi tgt)
        {
            tgt_dal.Insert(tgt);
        }
        public void Delete(string condition)
        {
            tgt_dal.Delete(condition);
        }
        public void Update(ThamGiaThi tgt)
        {
            tgt_dal.Update(tgt);
        }
        public bool IsConflicting(string monthi1, string monthi2)
        {
            return tgt_dal.IsConflicting(monthi1, monthi2);
        }
    }
}
