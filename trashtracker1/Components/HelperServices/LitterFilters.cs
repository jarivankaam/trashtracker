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
        public int PredictedTotal { get; set; }
        public int PredictionConfidence { get; set; }
        public float[] ConfidenceValues { get; private set; } = [];



        
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

        public async Task UpdateChartAsync(bool isInitialRender = false)
        {
            if (_jsRuntime == null || Labels == null || Values == null || Labels.Length == 0 || Values.Length == 0)
            {
                Console.WriteLine("⚠️ Chart not rendered: missing data.");
                return;
            }

            Console.WriteLine("✅ Chart rendering with:");
            Console.WriteLine($"Labels: {string.Join(", ", Labels)}");
            Console.WriteLine($"Values: {string.Join(", ", Values)}");

            try
            {
                ChartTitle = "Afval per dag";
                var functionName = isInitialRender ? "renderLineChart" : "updateLineChart";

                if (isLastChosenFutureSelected && ConfidenceValues != null && ConfidenceValues.Length > 0)
                {
                    // Pass confidence values as tooltip data
                    await _jsRuntime.InvokeVoidAsync(functionName, Labels, Values, ChartTitle, ConfidenceValues);
                }
                else
                {
                    await _jsRuntime.InvokeVoidAsync(functionName, Labels, Values, ChartTitle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chart JS error: {ex.Message}");
            }
        }





       public string MostLitterLocation()
{
    if (litterDto == null || !litterDto.Any())
        return "Geen gegevens beschikbaar";

    List<LitterDto> filteredLitter = new();

    if (lastChosenDays == 1 && !isLastChosenFutureSelected)
    {
        var now = DateTime.Now;
        DateTime beginDay = new DateTime(now.Year, now.Month, now.Day);

        filteredLitter = litterDto
            .Where(l => l.DetectionTime.Date == beginDay.Date &&
                        (lastChosenTypeOfLitter == 8 || l.Classification == lastChosenTypeOfLitter))
            .ToList();
    }
    else if (!isLastChosenFutureSelected)
    {
        for (int i = (lastChosenDays - 1); i >= 0; i--)
        {
            string currentDay = DateTime.Now.AddDays(-i).ToString("dd-MM");

            filteredLitter.AddRange(litterDto.Where(l =>
                l.DetectionTime.ToString("dd-MM").Contains(currentDay) &&
                (lastChosenTypeOfLitter == 8 || l.Classification == lastChosenTypeOfLitter)));
        }
    }
    else // Future
    {
        return "Geen gegevens voor toekomst beschikbaar";
    }

    var mostLitter = filteredLitter
        .GroupBy(l => $"{Math.Round(l.LocationLatitude, 3)}, {Math.Round(l.LocationLongitude, 3)}")
        .Select(g => new { Location = g.Key, Count = g.Count() })
        .OrderByDescending(x => x.Count)
        .FirstOrDefault();

    return mostLitter?.Location ?? "Onbekend";
}

public int MostLitterAmount()
{
    if (litterDto == null || !litterDto.Any())
        return 0;

    List<LitterDto> filteredLitter = new();

    if (lastChosenDays == 1 && !isLastChosenFutureSelected)
    {
        var now = DateTime.Now;
        DateTime beginDay = new DateTime(now.Year, now.Month, now.Day);

        filteredLitter = litterDto
            .Where(l => l.DetectionTime.Date == beginDay.Date &&
                        (lastChosenTypeOfLitter == 8 || l.Classification == lastChosenTypeOfLitter))
            .ToList();
    }
    else if (!isLastChosenFutureSelected)
    {
        for (int i = (lastChosenDays - 1); i >= 0; i--)
        {
            string currentDay = DateTime.Now.AddDays(-i).ToString("dd-MM");

            filteredLitter.AddRange(litterDto.Where(l =>
                l.DetectionTime.ToString("dd-MM").Contains(currentDay) &&
                (lastChosenTypeOfLitter == 8 || l.Classification == lastChosenTypeOfLitter)));
        }
    }
    else // Future
    {
        return 0;
    }

    var mostLitter = filteredLitter
        .GroupBy(l => $"{Math.Round(l.LocationLatitude, 3)}, {Math.Round(l.LocationLongitude, 3)}")
        .Select(g => new { Location = g.Key, Count = g.Count() })
        .OrderByDescending(x => x.Count)
        .FirstOrDefault();

    return mostLitter?.Count ?? 0;
}
        public int FilteredLitterAmount()
        {

            int filteredLitterAmount = rnd.Next(0, 100);

            return filteredLitterAmount;
        }

        public async Task GetLitterData(int days, int typeOfLitter, bool isFutureSelected)
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
            else if (isFutureSelected) // Toekomst
            {
                var confidenceList = new List<float>();

                for (int i = 0; i < days && i < predictionDto.Count; i++)
                {
                    var prediction = predictionDto[i];
                    litterData.Add(prediction.predictedTotal);
                    confidenceList.Add(prediction.confidence); // Assuming your DTO has this field

                    string currentDay = prediction.date.ToString("dd-MM");
                    litterDaysLabels.Add(currentDay == DateTime.Now.ToString("dd-MM") ? "Vandaag" : currentDay);
                }

                ConfidenceValues = confidenceList.ToArray();
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
                ConfidenceValues = Array.Empty<float>();

            }
            if (predictionDto.Count > 0)
            {
                var combinedPrediction = predictionDto.Take(days).ToList();
                PredictedTotal = combinedPrediction.Sum(p => p.predictedTotal);
                PredictionConfidence = (int)(combinedPrediction.Average(p => p.confidence) * 100);
            }


                
            Values = litterData.ToArray();
            Labels = litterDaysLabels.ToArray();
            await UpdateChartAsync(isInitialRender: false);
            await GetHolidayData();
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