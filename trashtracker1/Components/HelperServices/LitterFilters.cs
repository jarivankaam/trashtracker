using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using trashtracker1.Components.HelperServices.API.Dto;
using trashtracker1.Components.HelperServices.API;

namespace trashtracker1.Components.HelperServices
{
    public class LitterFilters
    {
        private int amountOfLitter; 
        public Random rnd = new Random();
        public string location;
        public int LitterAmount;
        public int filteredLitterAmount;
        public List<int> litterData = new List<int>();
        public List<string> litterDays = new List<string>();
        public List<string> litterDaysLabels = new List<string>();
        public List<HolidaysDto> displayedHolidays = new();
        private List<HolidaysDto> holidaysDto = new();
        private List<PredictionDto> predictionDto = new();
        private List<LitterDto> litterDto = new();
        public int lastChosenDays = 7;
        public bool isLastChosenFutureSelected = false;
        public int lastChosenTypeOfLitter = 8;
        public string[] Labels = [];
        public int[] Values;
        public string ChartTitle = "Afval per dag";
        
        private readonly ApiClient apiClient;

        private readonly IJSRuntime _jsRuntime;

        public LitterFilters(ApiClient apiClient, IJSRuntime jsRuntime)
        {
            this.apiClient = apiClient;
            _jsRuntime = jsRuntime;
        }
        public async Task MakeApiCall()
        {
                litterDto = await apiClient.GetLitterAsync();
                holidaysDto = await apiClient.GetHolidaysAsync();
                predictionDto = await apiClient.GetPredictionAsync();
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

        public async void GetLitterData(int days, int typeOfLitter, bool isFutureSelected)
        {
            lastChosenDays = days;
            isLastChosenFutureSelected = isFutureSelected;
            lastChosenTypeOfLitter = typeOfLitter;
            litterData.Clear();
            litterDays.Clear();
            litterDaysLabels.Clear();
            if (lastChosenDays == 1) // geschiedenis vandaag
            {
                DateTime now = DateTime.Now;
                DateTime beginDay = new DateTime(now.Year, now.Month, now.Day);
                litterDays.Add(DateTime.Now.ToString("dd-MM"));
                for (int i = 0; i < 24; i++)
                {
                    DateTime targetHour = beginDay.AddHours(i);
                    amountOfLitter = 0;
                    foreach (LitterDto litter in litterDto)
                    {
                        if (litter.DetectionTime.Day == targetHour.Day && litter.DetectionTime.Hour == targetHour.Hour && (lastChosenTypeOfLitter == litter.Classification || lastChosenTypeOfLitter == 8))
                        {
                            amountOfLitter++;
                        }
                    }
                    litterData.Add(amountOfLitter);
                    litterDaysLabels.Add(beginDay.AddHours(i).ToString("HH:mm"));
                }
            }
            else if (!isFutureSelected) // geschiedenis meerdere dagen
            {
                for (int i = (lastChosenDays - 1); i >= 0; i--)
                {
                    amountOfLitter = 0;
                    string currentDay = DateTime.Now.AddDays(-i).ToString("dd-MM");
                    foreach (LitterDto litter in litterDto)
                    {
                        if (litter.DetectionTime.ToString("dd-MM").Contains(currentDay) && (lastChosenTypeOfLitter == litter.Classification || lastChosenTypeOfLitter == 8))
                        {
                            amountOfLitter++;
                        }
                    }
                    litterData.Add(amountOfLitter);
                    litterDays.Add(currentDay);
                    if (currentDay == DateTime.Now.ToString("dd-MM"))
                    {
                        litterDaysLabels.Add("Vandaag");
                    }
                    else
                    {
                        litterDaysLabels.Add(currentDay);
                    }
                }
            }
            else if (isFutureSelected) // Toekomst
            {
                for (int i = 0; i < days; i++)
                {
                    PredictionDto prediction = predictionDto[i];
                    litterData.Add(prediction.predictedTotal);
                    string currentDay = prediction.date.ToString("dd-MM");
                    if (currentDay == DateTime.Now.ToString("dd-MM"))
                    {
                        litterDaysLabels.Add("Vandaag");
                    }
                    else
                    {
                        litterDaysLabels.Add(currentDay);
                    }
                }
            }
                
            Values = litterData.ToArray();
            Labels = litterDaysLabels.ToArray();
            
            await GetHolidayData();
            await UpdateChartAsync();
        }
        
        public async Task GetHolidayData()
        {
            displayedHolidays.Clear();
            foreach (string day in litterDays)
            {
                foreach (HolidaysDto holiday in holidaysDto)
                {
                    if (holiday.Date.ToString("dd-MM").Contains(day))
                    {
                        displayedHolidays.Add(new HolidaysDto
                        {
                            LocalName = holiday.LocalName,
                            Date = holiday.Date 
                        });
                    }
                }
            }
        }
    }
}