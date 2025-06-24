namespace trashtracker1.Components.HelperServices.API.Dto
{
    public class GoogleGeocodingDto
    {
        public List<Result> results { get; set; }

        public class Result
        {
            public string formatted_address { get; set; }
        }
    }
}