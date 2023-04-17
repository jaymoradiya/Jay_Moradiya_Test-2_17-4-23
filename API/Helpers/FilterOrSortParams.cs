using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class FilterOrSortParams
    {
        public string Sort { get; set; }
        public string OrderBy { get; set; } = "desc";
        public string[] Departments { get; set; }
        public string[] Genders { get; set; }
    }
}