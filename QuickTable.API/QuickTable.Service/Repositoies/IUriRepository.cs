using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies
{
    public interface IUriRepository
    {
        public Uri GetPageUri(string route);
    }
}
