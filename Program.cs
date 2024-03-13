using Microsoft.EntityFrameworkCore;
using ManagerIO.Model.Context;
using ManagerIO.Repository;
using ManagerIO.Repository.Generic;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using ManagerIO.Business.Interfaces;
using Microsoft.IdentityModel.Tokens;
using ManagerIO.Business;
using ManagerIO.Services;
using ManagerIO.Configurations;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ManagerApi.Repository;
using ManagerApi.Business.Interfaces;
using ManagerApi.Business;
using ManagerApi.Configurations;


internal class Program
{
    private static void Main(string[] args)
    {
        Settings StartConfig = new Settings();
        API api = new API();


        Console.WriteLine("Iniciando App.");

        // Inicializa o Projeto = Configurations/InicializationSettings.cs
        InicializationSettings inicialization = StartConfig.Initialization();

        // Verifica o Resultado
        if (inicialization.SoftwareStatus)
        {
            api.StartAPI(args, inicialization.GlobalConnetionString);
        }

    }
}
public class API
{
    public void StartAPI(string[] args, string ConnectionString)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("API Online");
        Console.ForegroundColor = ConsoleColor.White;

        var builder = WebApplication.CreateBuilder(args);
        var appName = "Rest API's from SuporteIO";
        var appVersion = "v1";

        builder.Services.AddRouting(option => option.LowercaseUrls = true);

        var tokenConfigurations = new TokenConfiguration();

        new ConfigureFromConfigurationOptions<TokenConfiguration>(
                builder.Configuration.GetSection("TokenConfiguration")
            )
            .Configure(tokenConfigurations);

        builder.Services.AddSingleton(tokenConfigurations);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = appName,
                Version = "v1",
                Description = $"API desenvolvida para o software SuporteIO",
                Contact = new OpenApiContact
                {
                    Name = "IoEletronica"
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
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
                new string[] {}
            }
        });
        });


        builder.Services.AddCors(options =>

            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

        builder.Services.AddControllers();

        // conexao com o banco - esse globalConnectionString vem do Settings
        builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(ConnectionString, new MySqlServerVersion(new Version(8, 0, 29))));

        // ------------ secao scopo


        // scope generico
        builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        // scopo do Login
        builder.Services.AddScoped<ILoginBusiness, LoginBusiness>();

        // scopo do Usuario
        builder.Services.AddScoped<IUsersBusiness, UsersBusiness>();
        builder.Services.AddScoped<UsersRepository>();

        // scopo do Cartoes
        builder.Services.AddScoped<ICardBusiness, CardBusiness>();
        builder.Services.AddScoped<CardRepository>();

        // scopo do Cliente
        builder.Services.AddScoped<IClientBusiness, ClientBusiness>();
        builder.Services.AddScoped<ClientRepository>();

        // scopo das Configurações
        builder.Services.AddScoped<IConfigBusiness, ConfigBusiness>();
        builder.Services.AddScoped<ConfigRepository>();

        //scopo das Leitoras
        builder.Services.AddScoped<IReadersSettingsBusiness, ReadersSettingsBusiness>();
        builder.Services.AddScoped<ReadersSettingsRepository>();

        //scopo das Leitoras
        builder.Services.AddScoped<IToyBusiness, ToyBusiness>();
        builder.Services.AddScoped<ToyRepository>();

        // ------------- secao de scopo
        builder.Services.AddApiVersioning();

        builder.Services.AddTransient<ITokenServices, TokenServices>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appName} - {appVersion}"); });
        }


        var option = new RewriteOptions();
        option.AddRedirect("^$", "swagger");
        app.UseRewriter(option);

        app.UseAuthorization();

        app.MapControllers();
        app.MapControllerRoute("DefaultApi", "{controller=values}/v{version=apiVersion}/{id?}") ;

        app.Run();
    }
}