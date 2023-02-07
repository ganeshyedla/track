using System;
using System.Collections.Generic;

namespace TrackNowApi.Model
{
    public partial class CustomerHistory
    {
        public string? Title { get; set; }
        public decimal? CustomerId { get; set; }
        public decimal? CatergoryId { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public decimal? LocationCode { get; set; }
        public string? TaxNumber { get; set; }
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? Juristiction { get; set; }
        public string? Dboperation { get; set; }
        public string? Source { get; set; }
        public string? Notes { get; set; }
        public string? Poc { get; set; }
    }
}
