using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebShopData.Data;
using WebShopData.Repositories;

namespace WebShopData.Bootstrap
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPostgreSQL(this IServiceCollection services, string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty.");
            }

            // Dodavanje DbContext-a sa Npgsql
            services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

            // Dodavanje repozitorijuma
            services.AddScoped<IClothesRepository, ClothesRepository>();
            services.AddScoped<ITypesRepository, TypesRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}