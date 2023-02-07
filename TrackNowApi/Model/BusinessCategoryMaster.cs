using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class BusinessCategoryMaster
    {
        public decimal CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
    }
}
