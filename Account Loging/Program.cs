using Account_Loging.BL.AcountSecvices;
using Account_Loging.BL.Mapper;
using Account_Loging.DAL.Repositories;
using AccountLog.DAL.DbContainer;
using AutoMapper;
using MatrixTask.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//---Serilog congigration---//
//builder.Logging.AddSerilog();
//builder.Host.UseSerilog((hostContext, services, configuration) => {
//    ReadFrom.Configuration(context.Configuration).
//    ReadFrom.Services(Services).
//    configuration.WriteTo.Console();
//});

var congig=builder.Configuration.AddJsonFile("appsettings.json").Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(congig).CreateLogger();

 
try
{
    Log.Information("Starting the Service");

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "stander authorazation header using the bearer scheme (\"bearer {token})",
            In = ParameterLocation.Header,
            Name = "Authorazation",
            Type = SecuritySchemeType.ApiKey
        });
        option.OperationFilter<SecurityRequirementsOperationFilter>();
    });

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();

    //.AddJwtBearer(option =>
    // {
    //     option.TokenValidationParameters = new TokenValidationParameters
    //     {
    //         ValidateIssuerSigningKey = true,
    //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Appsetings:Token").Value)),
    //         ValidateIssuer = false,
    //         ValidateAudience = false,
    //     };
    // });
    var MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
    builder.Services.AddCors(option =>
    {
        option.AddPolicy(name: MyAllowSpecificOrigins,
                      option =>
                      {
                          option
                          .WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
       // option.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
    });
    // Auto Mapper Configurations
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new DomainProfile());

    });

    IMapper mapper = mapperConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);

    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IAcountServices, AcountServices>();
    builder.Services.AddDbContext<AccountLogContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AccountDb")));



    builder.Host.UseSerilog(Log.Logger);
    builder.Logging.AddSerilog();
     var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors(MyAllowSpecificOrigins);
    //app.UseCors(options => options.AllowAnyOrigin());
    app.MapControllers();

    app.Run();



}
catch (Exception ex)
{
    Log.Fatal(ex, "There was a problem starting the service");
    return;
}
finally
{
    Log.Information("Service successfully stopped");
    Log.CloseAndFlush();
}


