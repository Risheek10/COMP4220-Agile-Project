using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ywBookStoreLIB
{
    public class detailData
    {
        public Boolean detailCheck(DataRowView selectedRow)
        {
            if (selectedRow == null) { 
                return false;
            
            }else if (selectedRow.Row.IsNull("Title") || selectedRow.Row.IsNull("Price") || selectedRow.Row.IsNull("ISBN"))
            {
                return false;
            }
            else
            {
                return true;
            }
   
        }
        
    }
}
