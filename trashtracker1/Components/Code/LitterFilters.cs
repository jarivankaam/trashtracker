using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq;

namespace trashtracker1.Components.Code
{
    public class LitterFilters
    {
        public Random rnd = new Random();
        public string location;
        public int LitterAmount;
        public int filteredLitterAmount;
        public List<int> litterData = new List<int>();
        public List<string> litterDays = new List<string>();

        public string[] Labels = ["Ma", "Di", "Wo", "Do", "Vr", "Za", "Zo"];
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
            for (int i = days; i > 0; i--)
            {
                litterData.Add(rnd.Next(0, 100));
                litterDays.Add(DateTime.Now.AddDays(-i).ToString("dd-MM"));
            }
            Values = litterData.ToArray();
            Labels = litterDays.ToArray();
            await UpdateChartAsync();
        }

    }
}
