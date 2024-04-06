using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class OrderPageRequest
    {
        public int Page { get; set; } 
        public string? StatusFilter { get; set; }
        public bool OrderByNewest { get; set; } = true;
    }
}
