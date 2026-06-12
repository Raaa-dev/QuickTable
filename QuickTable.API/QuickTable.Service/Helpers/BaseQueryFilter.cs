using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Helpers
{
    public class BaseQueryFilter
    {
        private int _pageNo = 1;
        private int _pageSize = 10;

        public int PageNo
        {
            get => _pageNo;
            set => _pageNo = value <= 0 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 10 : value;
        }
    }
}
