using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Common.DTO;

namespace InParadise.Application.Services.Interface
{
    public interface IPaymentService
    {
        ZarinPalRequestDto ZarinPalRequestData(int amount, string callBackUrl);
        Task<PaymentRequestResultDto> RequestZarinPalPayment(int bookingId, int totalCost, string callBackUrl);
        Task<PaymentVerifyResultDto> ZarinPalVerifyData(string authority);
    }
}