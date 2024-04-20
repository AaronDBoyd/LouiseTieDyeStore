using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public bool IsMainImage { get; set; }
    }
}
