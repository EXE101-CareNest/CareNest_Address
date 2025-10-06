using CareNest_Address.API.Middleware;
using CareNest_Address.Application.Common;
using CareNest_Address.Application.Features.Commands.Create;
using CareNest_Address.Application.Features.Commands.Delete;
using CareNest_Address.Application.Features.Commands.Update;
using CareNest_Address.Application.Features.Commands.SetDefault;
using CareNest_Address.Application.Features.Queries.GetAllPaging;
using CareNest_Address.Application.Features.Queries.GetById;
using CareNest_Address.Application.Interfaces.CQRS;
using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Application.Interfaces.CQRS.Queries;
using CareNest_Address.Application.Interfaces.Services;
using CareNest_Address.Application.Interfaces.UOW;
using CareNest_Address.Application.UseCases;
using CareNest_Address.Domain.Entitites;
using CareNest_Address.Domain.Repositories;
using CareNest_Address.Infrastructure.Persistences.Configuration;
using CareNest_Address.Infrastructure.Persistences.Database;
using CareNest_Address.Infrastructure.Persistences.Repository;
using CareNest_Address.Infrastructure.Services;
using CareNest_Address.Infrastructure.UOW;
using CareNest_Addressry.Application.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Lấy DatabaseSettings từ configuration
DatabaseSettings dbSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>()!;
dbSettings.Display();
string connectionString = dbSettings!.GetConnectionString();


// Đăng ký DbContext với PostgreSQL
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null);
    }));

builder.Services.AddTransient<DatabaseSeeder>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Đăng ký service thêm chú thích cho api
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    //ADD JWT BEARER SECURITY DEFINITION
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        //Type = SecuritySchemeType.ApiKey,
        Type = SecuritySchemeType.Http,//ko cần thêm token phía trước
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            },
            new List<string>()
        }
    });
});

// Đăng ký các repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//command
builder.Services.AddScoped<ICommandHandler<CreateCommand, Address>, CreateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCommand, Address>, UpdateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteCommand>, DeleteCommandHandler>();
builder.Services.AddScoped<ICommandHandler<SetDefaultCommand, Address>, SetDefaultCommandHandler>();
//query
builder.Services.AddScoped<IQueryHandler<GetAllPagingQuery, PageResult<AddressResponse>>, GetAllPagingQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetByIdQuery, Address>, GetByIdQueryHandler>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);


//Đăng ký cho FE
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

// Đăng ký cấu hình APIServiceOption
builder.Services.Configure<APIServiceOption>(
    builder.Configuration.GetSection("APIService")
);
//Đăng ký lấy thông tin từ token
builder.Services.AddHttpClient<IAPIService, APIService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    context.Database.Migrate();
}
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();