using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookReviews.Application.Common.Interfaces;
using BookReviews.Domain.Settings;
using BookReviews.Infrastructure.Identity;
using BookReviews.Infrastructure.Persistence;


namespace BookReviews.Infrastructure.Registration
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration 
        configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
                // .AddDefaultTokenProviders(); 
                
            services.AddTransient<IAuthService, AuthService>();

            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);
            return services;
        }
    }
}