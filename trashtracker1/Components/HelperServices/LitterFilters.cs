using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using trashtracker1.Components.HelperServices.API.Dto;

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
        public int lastChosenDays = 7;
        public bool isLastChosenFutureSelected = false;
        public int lastChosenTypeOfLitter = 8;
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

        private List<HolidaysDto> holidaysDto = new() //test data Holiday api
        {
        new HolidaysDto { Date = new DateTime(2025, 6, 19), LocalName = "Nieuwjaarsdag" },
        new HolidaysDto { Date = new DateTime(2025, 6, 20), LocalName = "Goede Vrijdag" },
        new HolidaysDto { Date = new DateTime(2025, 6, 21), LocalName = "Eerste Paasdag" }
        };

        private List<PredictionDto> predictionDto = new() //test data Prediction api
        {
            new PredictionDto
            {
                predictedTotal = 21,
                confidence = 95.0f,
                date = new DateOnly(2025, 6, 22)
            },
            new PredictionDto
            {
                predictedTotal = 51,
                confidence = 95.0f,
                date = new DateOnly(2025, 6, 23)
            },
            new PredictionDto
            {
                predictedTotal = 61,
                confidence = 95.0f,
                date = new DateOnly(2025, 6, 24)
            },
            new PredictionDto
            {
                predictedTotal = 71,
                confidence = 95.0f,
                date = new DateOnly(2025, 6, 25)
            },
            new PredictionDto
            {
                predictedTotal = 81,
                confidence = 95.0f,
                date = new DateOnly(2025, 6, 26)
            },
            new PredictionDto
            {
                predictedTotal = 0,
                confidence = 95.0f,
                date = new DateOnly(2025, 6, 27)
            },
            new PredictionDto
            {
                predictedTotal = 101,
                confidence = 95.0f,
                date = new DateOnly(2025, 6, 28)
            }
        };

        private List<LitterDto> litterDto = new() //test data Litter api
        {
            new LitterDto
            {
                Id = "L001",
                Classification = 1,
                Confidence = 95.3f,
                LocationLongitude = 4.899431f,
                LocationLatitude = 52.379189f,
                DetectionTime = new DateTime(2025, 6, 20, 14, 30, 0)
            },
            new LitterDto
            {
                Id = "L002",
                Classification = 2,
                Confidence = 87.5f,
                LocationLongitude = 4.895168f,
                LocationLatitude = 52.370216f,
                DetectionTime = new DateTime(2025, 6, 20, 9, 15, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 21, 11, 45, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 20, 12, 45, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 19, 11, 45, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 20, 10, 45, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 20, 0, 45, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 20, 0, 0, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 20, 23, 59, 0)
            },
            new LitterDto
            {
                Id = "L003",
                Classification = 3,
                Confidence = 92.1f,
                LocationLongitude = 4.920000f,
                LocationLatitude = 52.360000f,
                DetectionTime = new DateTime(2025, 6, 20, 15, 00, 0)
            }
        };

        public async void GetLitterData(int days, int typeOfLitter, bool isFutureSelected)
        {
            lastChosenDays = days;
            isLastChosenFutureSelected = isFutureSelected;
            lastChosenTypeOfLitter = typeOfLitter;
            litterData.Clear();
            litterDays.Clear();
            litterDaysLabels.Clear();
            if (!isFutureSelected) // geschiedenis
            {
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
                else // geschiedenis meerdere dagen
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
            }
            if (isFutureSelected) // Toekomst
            {
                foreach (PredictionDto litterPredictedAmount in predictionDto)
                {
                    litterData.Add(litterPredictedAmount.predictedTotal);
                    string currentDay = litterPredictedAmount.date.ToString("dd-MM");
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