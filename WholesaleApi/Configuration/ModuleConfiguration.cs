using System.Text;
using System.Threading.Tasks;
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
            _services.AddScoped<IUserService, UserService>();
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
                .UseLoggerFactory(Startup.MyLoggerFactory),
                ServiceLifetime.Transient)
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
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var userId = int.Parse(context.Principal.Identity.Name);
                            var user = userService.GetById(userId);
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
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
