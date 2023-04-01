using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackNowApi.Model
{
    public partial class CustomerBusinessCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }
        public decimal? CustomerId { get; set; }
        public string? State { get; set; }
        public decimal? BusinessCategoryId { get; set; }
     
    }
}
