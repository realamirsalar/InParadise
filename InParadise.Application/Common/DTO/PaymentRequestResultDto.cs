using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Common.DTO
{
    public class PaymentRequestResultDto
    {
        public bool IsSuccess { get; set; }
        public string? PaymentUrl { get; set; }
        public string? ErrorMessage { get; set; }
    }
}