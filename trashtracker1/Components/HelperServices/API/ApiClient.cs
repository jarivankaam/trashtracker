using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace trashtracker1.Components.HelperServices.API
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApiClient(HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
        }

        private void AttachJwtHeader()
        {
            var token = _contextAccessor.HttpContext?.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Dto.FavoriteLocationDto>> GetFavoriteLocationAsync()
        {
            AttachJwtHeader();
            var response = await _httpClient.GetAsync("https://avansict2231011.azurewebsites.net");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Dto.FavoriteLocationDto>>();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return new List<Dto.FavoriteLocationDto>();
            }
        }

        public async Task<List<Dto.HolidaysDto>> GetHolidaysAsync()
        {
            AttachJwtHeader();
            var response = await _httpClient.GetAsync("https://avansict2231011.azurewebsites.net/api/External/holidays");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Dto.HolidaysDto>>();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return new List<Dto.HolidaysDto>();
            }
        }

        public async Task<List<Dto.LitterDto>> GetLitterAsync()
        {
            AttachJwtHeader();
            var response = await _httpClient.GetAsync("https://avansict2231011.azurewebsites.net/api/Litter/GetAllLitter");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Dto.LitterDto>>();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return new List<Dto.LitterDto>();
            }
        }

        public async Task<List<Dto.PredictionDto>> GetPredictionAsync()
        {
            AttachJwtHeader();
            DateTime date = DateTime.Now.AddDays(1);
            string currentdate = date.ToString("yyyy-MM-dd");
            DateTime nextMonth = DateTime.Now.AddDays(1).AddMonths(1);
            string nextMonthDate = nextMonth.ToString("yyyy-MM-dd");
            string connectionString = $"https://avansict2231011.azurewebsites.net/api/External/predict?startDate={currentdate}&endDate={nextMonthDate}";
            var response = await _httpClient.GetAsync(connectionString);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Dto.PredictionDto>>();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return new List<Dto.PredictionDto>();
            }
        }

        public async Task<List<Dto.UserDto>> GetUserAsync()
        {
            AttachJwtHeader();
            var response = await _httpClient.GetAsync("https://avansict2231011.azurewebsites.net");
            string statusCode = Convert.ToString(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Dto.UserDto>>();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return new List<Dto.UserDto>();
            }
        }
    }
}
