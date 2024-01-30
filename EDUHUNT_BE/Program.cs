//using EDUHUNT_BE.Data;
//using EDUHUNT_BE.Hubs;
//using EDUHUNT_BE.Repositories;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using SharedClassLibrary.Contracts;
//using Swashbuckle.AspNetCore.Filters;
//using System.Text;






//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

//// builder 
//builder.Services.AddDbContext<AppDbContext>(options =>
//   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
//       throw new InvalidOperationException("Connection String is not found")

//   ));
//// ADD Identity & JWT AUTHENTICATION
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddSignInManager()
//    .AddRoles<IdentityRole>();
//builder.Services.AddScoped<IScholarship, ScholarshipRepository>();


//// Enable CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//    builder =>
//    {
//        builder.WithOrigins("http://localhost:3000")
//               .AllowAnyMethod()
//               .AllowAnyHeader()
//               .AllowCredentials();
//    });

//});

//builder.Services.AddSignalR();
//// ADD JWT AUTHENTICATION

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
//    };
//});


////add authentication to swagger UI
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//    });
//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

//builder.Services.AddScoped<IUserAccount, AccountRepository>();


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseCors("AllowSpecificOrigin");

//app.MapHub<ChatHub>("/chatHub");
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();



using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
