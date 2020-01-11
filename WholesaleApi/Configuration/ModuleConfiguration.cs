using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Wholesale.BL.Enums;
using Wholesale.BL.RepositoryInterfaces;
using Wholesale.BL.Services;
using Wholesale.DAL;
using Wholesale.DAL.Repositories;

namespace WholesaleApi.Configuration
{
    public class ModuleConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;

        public ModuleConfiguration(IConfiguration configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _services = services;
        }

        public void ConfigureServices()
        {
            _services.AddScoped<IUserRepository, UserRepository>();
            _services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            _services.AddScoped<IProductRepository, ProductRepository>();
            _services.AddScoped<IOrderRepository, OrderRepository>();
            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<IProductCategoryService, ProductCategoryService>();
            _services.AddScoped<IProductService, ProductService>();
            _services.AddScoped<IOrderService, OrderService>();
        }

        public void CreateNpsqlEnumMappings()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrderStatus>("order_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<UserRole>("user_role");
        }

        public void AddDatabaseContext()
        {
            var connectionString = _configuration.GetConnectionString("DeliveriesDatabase");

            //_services.AddDbContext<DeliveriesContext>(options => options
            //    .UseNpgsql(connectionString)
            //    .UseLoggerFactory(Startup.MyLoggerFactory));

            _services.AddEntityFrameworkNpgsql().AddDbContext<DeliveriesContext>(options => options
                .UseNpgsql(connectionString)
                .UseLoggerFactory(Startup.MyLoggerFactory))
                .BuildServiceProvider();
        }

        public void AddJwtAuthentication()
        {
            var appSettingsSection = _configuration.GetSection("AppSettings");
            _services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            _services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var userId = int.Parse(context.Principal.Identity.Name);
                            await using var con = new DeliveriesContext(new DbContextOptionsBuilder<DeliveriesContext>()
                                .UseNpgsql(_configuration.GetConnectionString("DeliveriesDatabase")).Options);
                            var user = await con.Users.FindAsync(userId);
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                        }
                    };
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public void AddSwagger()
        {
            _services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wholesale API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into the field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
    }
}
