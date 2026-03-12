using System.ComponentModel.DataAnnotations;

namespace InParadise.Web.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید.")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید.")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار رمز عبور خود را وارد کنید.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "رمز عبور وارد شده مطابقت ندارد!")]
        [Display(Name = "تایید کلمه عبور")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "لطفا رمز عبور خود را وارد کنید.")]
        public string Name { get; set; }

        [Display(Name = "شماره موبایل")] public string? PhoneNumber { get; set; }
        public string? RedirectUrl { get; set; }
    }
}