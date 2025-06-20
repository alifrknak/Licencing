using Application;
using Application.Hash;
using Business.Services.LicenceGenerators;
using Domain.Ports;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<DbContext, MasterContext>(options =>
          options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRESQL_URI")));

builder.Services.AddScoped<RequestContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IHashService, Argon2HashService>();
builder.Services.AddScoped(typeof(IPersistencePort<>), typeof(PersistentRespository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILicenceGeneratorService, LicenceGeneratorService>();

var app = builder.Build();

// debug modda  veritabaný baðlantýsýný kontrol etmesine gerek yoktur.

#if !DEBUG
app.Services.GetRequiredService<MasterContext>().Database.EnsureCreated();
#endif

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
