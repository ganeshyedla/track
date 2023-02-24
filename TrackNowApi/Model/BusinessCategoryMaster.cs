using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class BusinessCategoryMaster
    {
        public decimal BusinessCategoryId { get; set; }
        public string? BusinessCategoryName { get; set; }
        public string? BusinessCategoryDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
