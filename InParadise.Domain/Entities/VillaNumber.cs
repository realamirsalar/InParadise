using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InParadise.Domain.Entities
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "شماره ویلا")]
        [Required(ErrorMessage = "لطفا مقدار {0} را وارد کنید")]
        public int NumberOfVilla { get; set; }

        [Display(Name = "توضیحات مختص به این شماره ویلا")]
        public string? SpecialDetails { get; set; }

        [Display(Name = "ویلا")]
        [Required(ErrorMessage = "لطفا مقدار {0} را انتخاب کنید")]
        public int VillaId { get; set; }

        [ValidateNever]
        [ForeignKey("VillaId")]
        public virtual Villa Villa { get; set; }
    }
}