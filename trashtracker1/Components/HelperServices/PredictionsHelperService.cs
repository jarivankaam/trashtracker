using Microsoft.AspNetCore.Components;

namespace trashtracker1.Components.HelperServices;

public class PredictionsHelperService
{
    public string TitleData { get; private set; } = "Historische Data";

    public event Action? OnTitleChanged;

    public void OnSelectChange(ChangeEventArgs? e)
    {
        var selected = e?.Value?.ToString();
        TitleData = selected == "History" ? "Historische Data" : "Toekomstige Data";
        OnTitleChanged?.Invoke();
    }

    public bool IsFutureSelected => TitleData == "Toekomstige Data";
}