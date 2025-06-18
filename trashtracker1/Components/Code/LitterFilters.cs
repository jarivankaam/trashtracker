using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq;

namespace trashtracker1.Components.Code
{
    public class Holiday
    {
        public string Date { get; set; }
        public string LocalName { get; set; }
    }
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
        public List<Holiday> displayedHolidays = new();
        private List<Holiday> holidays = new()
        {
        new Holiday { Date = new DateTime(2025, 6, 18).ToString("dd-MM"), LocalName = "Nieuwjaarsdag" },
        new Holiday { Date = new DateTime(2025, 6, 17).ToString("dd-MM"), LocalName = "Goede Vrijdag" },
        new Holiday { Date = new DateTime(2025, 6, 16).ToString("dd-MM"), LocalName = "Eerste Paasdag" }
        };
        public async Task GetHolidayData()
        {
            displayedHolidays.Clear();
            foreach (string day in litterDays)
            {
                foreach (Holiday holiday in holidays)
                {
                    if (holiday.Date.Contains(day))
                    {
                        displayedHolidays.Add(holiday);
                    }
                }
            }
        }
    }
}
