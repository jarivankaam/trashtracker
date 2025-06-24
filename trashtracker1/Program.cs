using trashtracker1.Components;
using Blazorise;
using Blazorise.Tailwind;
using Blazorise.Icons.FontAwesome;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services
    .AddBlazorise()
    .AddTailwindProviders()
    .AddFontAwesomeIcons();
builder.Services.AddScoped<trashtracker1.Components.HelperServices.LitterFilters>();
builder.Services.AddScoped<trashtracker1.Components.HelperServices.PredictionsHelperService>();
builder.Services.AddHttpClient<trashtracker1.Components.HelperServices.API.ApiClient>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(); // Add this





var app = builder.Build();
app.MapControllers(); // Add this

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();