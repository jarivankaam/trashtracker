namespace trashtracker1.Components.Code.API.Dto
{
    public class PredictionDto
    {
          public DateOnly date { get; set; }
          public int predictedTotal { get; set; }
          public float confidence { get; set; }
       
    }
}
