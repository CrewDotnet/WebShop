using Microsoft.EntityFrameworkCore;
using WebShopApp.Services;
using WebShopData.Bootstrap;
using FluentValidation.AspNetCore;
using FluentValidation;
using WebShopApp.Validators;
using WebShopApp.Services.ServiceInterface;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddPostgreSQL(builder.Configuration.GetConnectionString("WebApiDatabase"));

builder.Services.AddScoped<IClothesService, ClothesService>();
builder.Services.AddScoped<ITypesService, TypesService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddRefitClient<ICustomerClient>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<ClothesItemRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
    await customerService.FetchAndSaveCustomersAsync(); // Povuci i popuni podatke
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

