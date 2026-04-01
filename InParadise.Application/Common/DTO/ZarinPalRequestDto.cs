using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Common.DTO
{
    public class ZarinPalRequestDto
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public string callback_url { get; set; }
    }
}