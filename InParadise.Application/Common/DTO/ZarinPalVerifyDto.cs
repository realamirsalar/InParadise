using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Common.DTO
{
    public class ZarinPalVerifyDto
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string authority { get; set; }
    }
}