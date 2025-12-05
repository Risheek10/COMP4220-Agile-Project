using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ywBookStoreLIB
{
    public class PriceFilterData
    {
        public string minPrice { set; get; }
        public string maxPrice { set; get; }
        public DataSet result { set; get; }
        public int filter(string min, string max)
        {
            DALPriceFilter priceFilter = new DALPriceFilter();
            if (String.IsNullOrEmpty(min) && String.IsNullOrEmpty(max))
            {
                return 1;
            }
            else
            {
                Regex regex = new Regex(@"^(\d+(\.\d{1,2})?|)$");
                if (!regex.IsMatch(min) || !regex.IsMatch(max))//check format
                {
                    return 2;
                }

                if (!String.IsNullOrEmpty(min) && !String.IsNullOrEmpty(max))
                {//make sure min < max
                    if (Double.Parse(min) > Double.Parse(max))
                    {
                        return 3;
                    }
                }
                result = priceFilter.PriceFilter(min, max); //get dataset
                return 0;
            }
        }
    }
}
