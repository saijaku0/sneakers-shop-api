using FluentValidation;
using MediatR;
using Scalar.AspNetCore;
using Sneakers.Shop.Backend.Api.Middleware;
using Sneakers.Shop.Backend.Application.Auth.Validations;
using Sneakers.Shop.Backend.Application.Injection;
using Sneakers.Shop.Backend.Infrastructure;
using Sneakers.Shop.Backend.Infrastructure.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

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

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<UserSeeder>();
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
app.Run();
