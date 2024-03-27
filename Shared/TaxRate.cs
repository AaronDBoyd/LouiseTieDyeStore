using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class TaxRate
    {
        public int Id { get; set; }
        public string State { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,3)")]
        public decimal Rate { get; set; }
    }
}
