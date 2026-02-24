using Microsoft.AspNetCore.Identity;
using VirasureYouAI.Context;
using VirasureYouAI.Entities;
using VirasureYouAI.Models;
using VirasureYouAI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VirasureContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<VirasureContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<AIService>();

builder.Services.AddHttpClient();
builder.Services.AddSignalR();

builder.Services.AddHttpClient("openai", c =>
{
    c.BaseAddress = new Uri("https://api.openai.com/");
});

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/ErrorPage/{0}");

app.MapHub<AskAIHubModel>("/chathub");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();