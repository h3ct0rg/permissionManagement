using Microsoft.EntityFrameworkCore;
using PermissionManagement.AutoMapper;
using PermissionManagement.Model;
using PermissionManagement.Repositories;
using PermissionManagement.Repositories.UnitOfWork;
using PermissionManagement.Services;
using Serilog;
using userPermissionManagement.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<permissionModelContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("interviewConnection"));
});

Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logfile.txt") // Especifica el nombre del archivo de registro
            .CreateLogger();

builder.Logging.AddSerilog();
builder.Services.AddSingleton<Serilog.ILogger>(dd => Log.Logger);


builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPermissionTypeService, PermissionTypeService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    permissionModelContext context = scope.ServiceProvider.GetRequiredService<permissionModelContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
