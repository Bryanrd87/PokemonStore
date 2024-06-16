using PokemonStore.Web.Service.IService;
using PokemonStore.Web.Service;
using PokemonStore.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IPokemonService, PokemonService>();
StaticDetails.PokemonAPI = builder.Configuration["PokemonApiUrl"];
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IPokemonService, PokemonService>();

StaticDetails.ApiKey = builder.Configuration["ApiKey"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
