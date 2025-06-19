using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Linq;
using trashtracker1.Components.Code.API.Dto;

namespace trashtracker1.Components.Code
{
    public class LitterFilters
    {
        public Random rnd = new Random();
        public string location;
        public int LitterAmount;
        public int filteredLitterAmount;
        public List<int> litterData = new List<int>();
        public List<string> litterDays = new List<string> { "18-6", "17-6", "16-6", "15-6", "14-6", "13-6", "12-6" };

        public string[] Labels = [];
        public int[] Values;
        public string ChartTitle = "Afval per dag";

        private readonly IJSRuntime _jsRuntime;

        public LitterFilters(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task UpdateChartAsync()
        {
            ChartTitle = "Afval per dag";
            await _jsRuntime.InvokeVoidAsync("updateLineChart", Labels, Values, ChartTitle);
        }

        public string MostLitterLocation()
        {

            string location = "Location";

            return location;
        }
        public int MostLitterAmount()
        {

            int LitterAmount = rnd.Next(0, 100);

            return LitterAmount;
        }
        public int FilteredLitterAmount()
        {

            int filteredLitterAmount = rnd.Next(0, 100);

            return filteredLitterAmount;
        }
      
        public async void GetLitterData(int days, int typeOfLitter)
        {
            litterData.Clear();
            litterDays.Clear();
            for (int i = (days - 1); i >= 0; i--)
            {
                litterData.Add(rnd.Next(0, 100));
                litterDays.Add(DateTime.Now.AddDays(-i).ToString("dd-MM"));
            }
            Values = litterData.ToArray();
            Labels = litterDays.ToArray();
            await GetHolidayData();
            await UpdateChartAsync();
        }
        public List<HolidaysDto> displayedHolidays = new();
        private List<HolidaysDto> holidaysDto = new()
        {
        new HolidaysDto { Date = "2025-06-19", LocalName = "Nieuwjaarsdag" },
        new HolidaysDto { Date = "2025-06-15", LocalName = "Goede Vrijdag" },
        new HolidaysDto { Date = "2025-06-17", LocalName = "Eerste Paasdag" }
        };
        public async Task GetHolidayData()
        {
            displayedHolidays.Clear();
            foreach (string day in litterDays)
            {
                foreach (HolidaysDto holiday in holidaysDto)
                {
                    if (FormatToDayMonth(holiday.Date).Contains(day))
                    {
                        displayedHolidays.Add(new HolidaysDto
                        {
                            LocalName = holiday.LocalName,
                            Date = (day == DateTime.Now.ToString("dd-MM")) ? "Vandaag" : day
                        });
                    }
                }
            }
        }
        public string FormatToDayMonth(string dateString)
        {
            var date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return date.ToString("dd-MM");
        }
    }
}
