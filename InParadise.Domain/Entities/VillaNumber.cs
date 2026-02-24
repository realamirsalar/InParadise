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
        public int NumberOfVilla { get; set; }

        public string? SpecialDetails { get; set; }
        public int VillaId { get; set; }
        public virtual Villa Villa { get; set; }
    }
}