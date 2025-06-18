

using Microsoft.AspNetCore.Cors.Infrastructure;
using ScopeIndia.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<StudentSql>();
builder.Services.AddSingleton<CourseSql>();
builder.Services.AddControllersWithViews();
builder.Configuration.GetConnectionString("Scopestring");
builder.Services.AddScoped<IStudent,StudentSql>();
builder.Services.AddScoped<ICourse, CourseSql>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);

});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=WebHome}/{action=Home}/{id?}");

app.Run();
