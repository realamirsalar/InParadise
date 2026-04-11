using InParadise.Application.Common.DTO;
using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Application.Services.Interface;
using InParadise.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace InParadise.Application.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IBookingService _bookingService;

        public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration, HttpClient httpClient,
            IBookingService bookingService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpClient = httpClient;
            _bookingService = bookingService;
        }

        public ZarinPalRequestDto ZarinPalRequestData(int amount, string callBackUrl)
        {
            ZarinPalRequestDto zarinPalRequest = new ZarinPalRequestDto()
            {
                merchant_id = _configuration.GetValue<string>(SD.ZarinPalMerchantId),
                amount = amount,
                description = "این یک تست است ومن درحال آموزش هستم.",
                callback_url = callBackUrl
            };
            return zarinPalRequest;
        }

        public async Task<PaymentRequestResultDto> RequestZarinPalPayment(int bookingId, int totalCost,
            string callBackUrl)
        {
            try
            {
                var requestData = ZarinPalRequestData(totalCost, callBackUrl);
                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestUrl = _configuration.GetValue<string>(SD.ZarinPalPaymentRequestUrl);
                var response =
                    await _httpClient.PostAsync(requestUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseString);

                if (jsonDoc.RootElement.TryGetProperty("data", out JsonElement dataNode) &&
                    dataNode.ValueKind == JsonValueKind.Object)
                {
                    if (dataNode.TryGetProperty("code", out JsonElement codeNode) && codeNode.GetInt32() == 100)
                    {
                        // دریافت Authority (کد شناسه پرداخت)
                        string authority = dataNode.GetProperty("authority").GetString();
                        _bookingService.UpdatePayment(bookingId, authority, SD.ZarinPalGateway, null);

                        // هدایت کاربر به درگاه پرداخت تستی
                        var paymentGetWay = _configuration.GetValue<string>(SD.ZarinPalPaymentGatewayUrl);
                        string paymentUrl = paymentGetWay + authority;
                        return new PaymentRequestResultDto()
                        {
                            IsSuccess = true,
                            PaymentUrl = paymentUrl,
                            ErrorMessage = "درحال انتقال به درگاه پرداخت"
                        };
                    }
                }

                return new PaymentRequestResultDto()
                {
                    IsSuccess = false,
                    PaymentUrl = null,
                    ErrorMessage = "خطا در اتصال به درگاه پرداخت"
                };
            }
            catch (HttpRequestException ex)
            {
                return new PaymentRequestResultDto()
                {
                    IsSuccess = false,
                    ErrorMessage = "ارتباط با درگاه پرداخت برقرار نشد. لطفاً دقایقی دیگر تلاش کنید."
                };
            }
            catch (Exception ex)
            {
                return new PaymentRequestResultDto()
                {
                    IsSuccess = false,
                    ErrorMessage = "یک خطای سیستمی رخ داده است. با پشتیبانی تماس بگیرید."
                };
            }
        }

        public async Task<PaymentVerifyResultDto> ZarinPalVerifyData(string authority)
        {
            try
            {
                var booking = _bookingService.GetBookingByAuthority(authority);
                if (booking == null)
                {
                    return new PaymentVerifyResultDto()
                    {
                        IsSuccess = false,
                        ErrorMessage = "رزرو یافت نشد!"
                    };
                }

                var verifyData = new ZarinPalVerifyDto()
                {
                    merchant_id = _configuration.GetValue<string>(SD.ZarinPalMerchantId),
                    amount = (int)booking.TotalCost,
                    authority = authority
                };

                var json = JsonSerializer.Serialize(verifyData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                // آدرس سندباکس برای تایید پرداخت
                var verifyUrl = _configuration.GetValue<string>(SD.ZarinPalPaymentVerificationUrl);
                var response =
                    await _httpClient.PostAsync(verifyUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();


                using var jsonDoc = JsonDocument.Parse(responseString);

                if (jsonDoc.RootElement.TryGetProperty("data", out JsonElement dataNode) &&
                    dataNode.ValueKind == JsonValueKind.Object)
                {
                    if (dataNode.TryGetProperty("code", out JsonElement codeNode))
                    {
                        int code = codeNode.GetInt32();

                        if (code == 100 || code == 101)
                        {
                            // (اصلاح شده): جلوگیری از خطای سرریز. ref_id باید string یا long باشد.
                            string refId = dataNode.GetProperty("ref_id").GetInt64().ToString();

                            // (نکته): فرض کردم پارامتر آخر UpdatePayment از نوع string است (که باید باشد)
                            _bookingService.UpdatePayment(booking.Id, booking.Authority, SD.ZarinPalGateway, refId);
                            _bookingService.UpdateStatus(booking.Id, SD.StatusApproved, true, 0);

                            return new PaymentVerifyResultDto()
                            {
                                IsSuccess = true,
                                BookingId = booking.Id,
                                ErrorMessage = "پرداخت موفق بود!"
                            };
                        }
                    }
                }

                return new PaymentVerifyResultDto()
                {
                    IsSuccess = false,
                    ErrorMessage = "پرداخت ناموفق بود!"
                };
            }
            catch (HttpRequestException ex)
            {
                return new PaymentVerifyResultDto()
                {
                    IsSuccess = false,
                    ErrorMessage = "خطا در ارتباط با درگاه پرداخت!"
                };
            }
            catch (Exception e)
            {
                return new PaymentVerifyResultDto()
                {
                    IsSuccess = false,
                    ErrorMessage = "یک خطای ناشناخته در زمان تایید پرداخت رخ داد.!"
                };
            }
        }
    }
}