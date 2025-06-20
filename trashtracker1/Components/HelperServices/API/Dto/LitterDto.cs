namespace trashtracker1.Components.HelperServices.API.Dto
{
    public class LitterDto
    {
        public string Id { get; set; }
        public int Classification { get; set; }
        public float Confidence { get; set; }
        public float LocationLongitude { get; set; }
        public float LocationLatitude { get; set; }
        public DateTime DetectionTime { get; set; }
    }
}
