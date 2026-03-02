using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InParadise.Domain.Entities
{
    public class Amenity
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        public int VillaId { get; set; }

        [ForeignKey("VillaId")]
        [ValidateNever]
        public virtual Villa Villa { get; set; }
    }
}