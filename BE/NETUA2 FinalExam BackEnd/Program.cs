
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.API_Services;
using System.Reflection;
using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.DB_Repositories;
using Microsoft.Extensions.Configuration;
using FE_BE._DATA;
using Microsoft.EntityFrameworkCore;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._BUSINESS.BL_Services;



namespace NETUA2_FinalExam_BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //JWT
            builder.Services.AddTransient<IJwtService, JwtService>();

            //SERVICES
            builder.Services.AddScoped<IUserService, UserService>();//Update method should be scoped  
            builder.Services.AddScoped<IImageFileService, ImageFileService>();


            //MAPPERS
            builder.Services.AddTransient<IUserMapper, UserMapper>();
            builder.Services.AddTransient<IPersonMapper, PersonMapper>();
            builder.Services.AddTransient<ILocationMapper, LocationMapper>();


            //REPOSITORIES            
            builder.Services.AddScoped<IUserDBRepository, UserDBRepository>();//Repositories - scoped                    
            builder.Services.AddScoped<IImageRepository, ImageRepository>();//Repositories - scoped            
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();//Repositories - scoped
            builder.Services.AddScoped<ILivingLocationRepository, LivingLocationRepository>();//Repositories should be scoped




            // Connecting the database
            builder.Services.AddDbContext<FinalExamDbContext>(options =>
            {
                //sql lite - would work not only on windowsOS
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Authentication using JWTToken
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Auth schema is what jwt says it is.
               .AddJwtBearer(
               options =>
               {
                   var secretKey = builder.Configuration.GetSection("Jwt:Key").Value;
                   var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                       ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                       IssuerSigningKey = key
                   };
               });




            //For getting currently logged-in User ID
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            // Custom swagger comments
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "NETUA2 FinalExam BackEnd",
                    Description = "An ASP.NET Core Web API for logging users and their information",
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);//needs to tick a box in project -> properties to generate comments


                // Authorize button/function in swagger
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please ender valid JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }}, new List<string>()
                    }
                });

            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //Bypass internet browser security against homemade backend?
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            //User is first Authenticated and then given authorization
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
