//Adding services to the dependency injection container.
//Configuring middleware.
//Setting up logging, configuration, and other app settings.
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//The Build() method finalizes the setup process defined in the builder object.
//It assembles all configurations, services, and middleware added to the builder into a runnable web application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //is middleware in ASP.NET Core that enables HTTP Strict Transport Security (HSTS).
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//automaticall redirect http request to https request
app.UseHttpsRedirection();

//is middleware in ASP.NET Core that enables serving static files (e.g., HTML, CSS, JavaScript, images)
app.UseStaticFiles();

//is middleware in ASP.NET Core that enables routing in the application's HTTP request pipeline.
app.UseRouting();


//is middleware in ASP.NET Core that enables authorization in the HTTP request pipeline.
//Works in conjunction with authentication (verifying the user's identity).
app.UseAuthorization();

//default mapping 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run() starts the web server and makes the application ready to process requests.
//It marks the end of the middleware pipeline. No middleware added after this line will execute.
app.Run();
