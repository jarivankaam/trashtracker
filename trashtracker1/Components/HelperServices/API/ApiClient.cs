namespace trashtracker1.Components.HelperServices.API
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

        // USER MANAGEMENT

        // GET-requests
        public async Task<List<Dto.UserDto>> GetAllUsers()
        {
            var response = await _httpClient.GetAsync($"https://avansict2231011.azurewebsites.net/api/user");
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

        public async Task<List<Dto.UserDto>> GetUserAsync(string identityUserId)
        {
            var response = await _httpClient.GetAsync($"https://avansict2231011.azurewebsites.net/api/user/id/{identityUserId}");
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

        public async Task<string> GetIdenityUserIdByEmail(string email)
        {
            var response = await _httpClient.GetAsync($"https://avansict2231011.azurewebsites.net/api/user/authentication/id/{email}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return string.Empty;
            }
        }

        // POST-requests
        public async Task RegisterNewUser(Dto.UserDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("https://avansict2231011.azurewebsites.net/custom/auth/register", user);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        public async Task VerifyUserByPassword(Dto.UserDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("https://avansict2231011.azurewebsites.net/api/user/verifyuser", user);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        // UPDATE-requests
        public async Task UpdateUser(Dto.UserDto user)
        {
            var response = await _httpClient.PutAsJsonAsync("https://avansict2231011.azurewebsites.net/api/user/updateuser", user);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        // DELETE-requests
        public async Task DeleteUserByIdentiyUserId(string identityUserId)
        {
            var response = await _httpClient.DeleteAsync($"https://avansict2231011.azurewebsites.net/api/user/{identityUserId}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
    }
}
