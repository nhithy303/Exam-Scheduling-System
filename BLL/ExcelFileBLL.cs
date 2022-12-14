using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ExcelFileBLL
    {
        ExcelFileDAL excel_dal = new ExcelFileDAL();
        public bool Import(string filepath, string tablename, bool overwrite)
        {
            return excel_dal.Import(filepath, tablename, overwrite);
        }
    }
}
