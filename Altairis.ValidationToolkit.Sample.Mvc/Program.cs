var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Uncomment the following line to turn off the bank code validation globally
// builder.Services.AddSingleton<IBankCodeValidator>(new EmptyBankCodeValidator());

// Uncomment the following line to use online bank code validation
// builder.Services.AddSingleton<IBankCodeValidator>(new OnlineBankCodeValidator());

var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();