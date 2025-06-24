using System.ComponentModel.DataAnnotations;

namespace trashtracker1.Components.HelperServices.API.Dto
{
    public class WeatherInfoDto
    {
        public required string Id { get; set; }
        public float TemperatureCelsius { get; set; }
        public float Humidity { get; set; }
        public string? Conditions { get; set; }
        public LitterDto? Litter { get; set; }
    }
}
