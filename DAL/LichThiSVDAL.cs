using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LichThiSVDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        private LichThiSV GetLichThiSVFromDataRow(DataRow row)
        {
            LichThiSV ltsv = new LichThiSV();
            ltsv.MonThi = row["TenMon"].ToString();
            ltsv.PhongThi = row["MaPhong"].ToString();
            ltsv.NgayThi = DateTime.Parse(row["NgayThi"].ToString()).ToString("dd/MM/yyyy");
            ltsv.BuoiThi = row["BuoiThi"].ToString();
            return ltsv;
        }
        public LichThiSV[] GetList(string mssv, string ngaythi)
        {
            LichThiSV[] list = null;
            DataTable table = null;
            int n = 0;
            string query = "select mt.TenMon, tgt.MaPhong, ct.NgayThi, ct.BuoiThi"
                + " from ThamGiaThi tgt inner join MonThi mt on tgt.MaMon = mt.MaMon"
                + " inner join CaThi ct on mt.MaCa = ct.MaCa"
                + " where tgt.MSSV = '" + mssv + "'";
            if (ngaythi != String.Empty)
            {
                query += " and ct.NgayThi = '" + ngaythi + "'";
            }
            query += " order by ct.NgayThi, ct.BuoiThi desc";
            table = da.ExecuteQuery(query);
            n = table.Rows.Count;
            if (n == 0)
            {
                return null;
            }
            list = new LichThiSV[n];
            for (int i = 0; i < n; i++)
            {
                list[i] = GetLichThiSVFromDataRow(table.Rows[i]);
            }
            return list;
        }
    }
}
