using System.Net.Http.Headers;
using trashtracker1.Components.HelperServices.API.Dto;

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
        
        public async Task<string> GetAddressFromCoordinatesAsync(string coordinates)
        {
            var rawParts = coordinates.Trim().Split(',');

            if (rawParts.Length != 4)
                return "Ongeldige coördinaten"; // Verwacht exact: [51, 589, 4, 781]

            // Stap 2: combineer losse getallen tot geldige decimale strings
            var lat = $"{rawParts[0].Trim()}.{rawParts[1].Trim()}"; // "51.589"
            var lon = $"{rawParts[2].Trim()}.{rawParts[3].Trim()}"; // "4.781"

            var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lon}&key=AIzaSyCusLDewLcpJqvFB1PcR47aspiwA8w-kDk";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Google Maps API error: " + response.StatusCode);
                return "Adres niet beschikbaar";
            }

            var data = await response.Content.ReadFromJsonAsync<Dto.GoogleGeocodingDto>();
            return data?.results?.FirstOrDefault()?.formatted_address ?? "Adres niet gevonden";
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

        // USER MANAGEMENT

        // GET-requests
        public async Task<List<Dto.UserDto>> GetAllUsers()
        {
            AttachJwtHeader();
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

        public async Task<Dto.UserDto> GetUserAsync(string identityUserId)
        {
            AttachJwtHeader();
            var response = await _httpClient.GetAsync($"https://avansict2231011.azurewebsites.net/api/user/id/{identityUserId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Dto.UserDto>();
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return new Dto.UserDto();
            }
        }

        public async Task<string> GetIdenityUserIdByEmail(string email)
        {
            AttachJwtHeader();
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
        public async Task RegisterNewUser(UserCreateDto createUser)
        {
            AttachJwtHeader();
            var user = new UserDto
            {
                Id = Guid.NewGuid().ToString(),
                IdentityUserId = Guid.NewGuid().ToString(),
                Email = createUser.Email,
                Password = createUser.Password,
                Username = createUser.Username,
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                Role = createUser.Role
            };
            var response = await _httpClient.PostAsJsonAsync("https://avansict2231011.azurewebsites.net/custom/auth/register", user);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        public async Task VerifyUserByPassword(UserDto user)
        {
            AttachJwtHeader();
            var response = await _httpClient.PostAsJsonAsync("https://avansict2231011.azurewebsites.net/api/user/verifyuser", user);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        // UPDATE-requests
        public async Task UpdateUser(UserCreateDto user)
        {
            AttachJwtHeader();
            var response = await _httpClient.PutAsJsonAsync("https://avansict2231011.azurewebsites.net/api/user/updateuser", user);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        // DELETE-requests
        public async Task DeleteUserByIdentityUserId(string identityUserId)
        {
            AttachJwtHeader();
            var response = await _httpClient.DeleteAsync($"https://avansict2231011.azurewebsites.net/api/user/{identityUserId}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
        
    }
}
