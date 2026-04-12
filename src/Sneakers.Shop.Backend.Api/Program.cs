using FluentValidation;
using Scalar.AspNetCore;
using Sneakers.Shop.Backend.Api.Middleware;
using Sneakers.Shop.Backend.Application.Injection;
using Sneakers.Shop.Backend.Infrastructure;
using Sneakers.Shop.Backend.Infrastructure.Auth.Requirments;
using Sneakers.Shop.Backend.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title = "Sneakers Shop API";
        document.Info.Version = "v1";
        return Task.CompletedTask;
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActiveDropper", policy =>
        policy.AddRequirements(new ActiveDropperRequirement()));

    options.AddPolicy("ActiveModerator", policy =>
        policy.AddRequirements(new ActiveModeratorRequirement()));
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<UserSeeder>();
    builder.Services.AddScoped<BrandSeeder>();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
    await seeder.SeedAsync();

    if (app.Environment.IsDevelopment())
    {
        var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
        await userSeeder.SeedAsync();
        var brandSeeder = scope.ServiceProvider.GetRequiredService<BrandSeeder>();
        await brandSeeder.SeedAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.Run();
