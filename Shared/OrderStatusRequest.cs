﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public class OrderStatusRequest
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
