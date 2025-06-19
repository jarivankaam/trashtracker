namespace trashtracker1.Components.Code.API
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Dto.FavoriteLocationDto>> GetFavoriteLocationAsync()
        {
            var response = await _httpClient.GetAsync("https//Holidays.com");
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
            var response = await _httpClient.GetAsync("https//Holidays.com");
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
            var response = await _httpClient.GetAsync("https//Litter.com");
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
            var response = await _httpClient.GetAsync("https//Litter.com");
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
            var response = await _httpClient.GetAsync("https//Litter.com");
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
