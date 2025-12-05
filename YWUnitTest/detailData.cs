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

            }
            else
            {
                string isbn = selectedRow["ISBN"].ToString();
                string title = selectedRow["Title"].ToString();
                string price = selectedRow["Price"].ToString() ;
                if (string.IsNullOrEmpty(isbn) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(price))
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
}


