using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ywBookStoreLIB
{
    public class searchData
    {
        public int keyword { set; get; }
        public string category { set; get; }
        public DataSet result { set; get; }


        public int search(string keyword, string category)
        {
            if (String.IsNullOrEmpty(keyword) || String.IsNullOrWhiteSpace(keyword))
            {
                return 1;
            }
            else
            {
                for (int i = 0; i < keyword.Length; i++)
                {
                    if (keyword[i] < 48 || (keyword[i] > 57 && keyword[i] < 65) || (keyword[i] > 90 && keyword[i] < 97) || keyword[i] > 122)
                    {
                        return 3;//format fail
                    }
                    else
                    {
                        continue;
                    }
                }
                if (category=="Year" || category=="Edition")
                {
                    for (int i = 0; i < keyword.Length; i++)
                    {
                        if (keyword[i]< 48 || keyword[i] > 57)
                        {
                            return 4;//format fail
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                var srh_function = new search();
                result = srh_function.search_keyword(category, keyword);
                return 0; 
            }
            
                    
        }
    }
}
