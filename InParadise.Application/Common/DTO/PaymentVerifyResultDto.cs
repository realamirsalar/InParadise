using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Common.DTO
{
    public class PaymentVerifyResultDto
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public int? BookingId { get; set; }
    }
}