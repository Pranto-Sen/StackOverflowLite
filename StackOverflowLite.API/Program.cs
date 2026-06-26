using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackOverflowLite.API.Middleware;
using StackOverflowLite.Application.DependencyInjection;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Infrastructure.DependencyInjection;
using StackOverflowLite.Persistence.Context;
using StackOverflowLite.Persistence.DependencyInjection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
          JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "StackOverflowLite API",
            Version = "v1"
        });


    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
            "Enter JWT Token"
        });


    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                    new OpenApiReference
                    {
                        Type =
                        ReferenceType.SecurityScheme,

                        Id = "Bearer"
                    }
                },

                Array.Empty<string>()
            }
        });
});




builder.Services.AddApplication();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();
}


app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();


app.Run();