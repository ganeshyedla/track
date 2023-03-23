namespace TrackNowApi.Model
{
    
    public partial class FilingBusinessCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }
        public decimal? FilingId { get; set; }
        public decimal BusinessCategoryId { get; set; }
        public string? State { get; set; }
    }
}
