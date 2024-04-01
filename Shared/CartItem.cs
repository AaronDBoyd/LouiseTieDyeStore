using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class CartItem
    {
        public string UserEmail { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
