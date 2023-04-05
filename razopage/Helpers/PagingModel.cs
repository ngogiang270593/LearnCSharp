using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    public class PagingModel
    {
        public int CurrentPage {get;set;}
        public int CountPages {get;set;}
        public Func<int?,string> GenerateUrl {get;set;} = default!;
    }
}