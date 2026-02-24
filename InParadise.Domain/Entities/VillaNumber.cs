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
        public int NumberOfVilla { get; set; }

        [Display(Name = "توضیحات مختص به این شماره ویلا")]
        public string? SpecialDetails { get; set; }

        [Display(Name = "ویلا")] public int VillaId { get; set; }
        [ForeignKey("VillaId")] public virtual Villa Villa { get; set; }
    }
}