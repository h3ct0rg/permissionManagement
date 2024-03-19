using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using PermissionManagement.AutoMapper;
using PermissionManagement.CQRS.Query;
using PermissionManagement.CQRS;
using PermissionManagement.Model;
using PermissionManagement.Repositories.UnitOfWork;
using PermissionManagement.Repository.BaseRepository;
using PermissionManagement.Repository.ElasticSearch;
using PermissionManagement.Repository.PermissionRepository;
using PermissionManagement.Service.ElasticSearch;
using PermissionManagement.Service.Kafka;
using PermissionManagement.Services;
using Serilog;
using PermissionManagement.CQRS.Command;
using PermissionManagement.ViewModels;

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
builder.Services.AddScoped(typeof(IPermissionRepository), typeof(PermissionRepository));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPermissionTypeService, PermissionTypeService>();
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<IElasticSearchProvider>(provider =>
{
    var elasticSearchUri = new Uri(builder.Configuration.GetConnectionString("elasticSearchConnection"));
    return new ElasticSearchProvider(elasticSearchUri);
});

builder.Services.AddSingleton<IProducer<string, string>>(provider =>
{
    var kafkaConfig = new ProducerConfig
    {
        BootstrapServers = builder.Configuration.GetConnectionString("kafkaConnection"), // Cambia esto según tu configuración de Kafka
        ClientId = "test-producer"
    };
    return new ProducerBuilder<string, string>(kafkaConfig).Build();
});

#region CQRS Permission Service
builder.Services.AddScoped<IQueryHandler<GetPermissionListQuery, List<permissionModel>>, GetPermissionListQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPermissionByEmployeeQuery, List<permissionModel>>, GetPermissionByEmployeeQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetPermissionByIdQuery, permissionModel>, GetPermissionByIdQueryHandler>();

//builder.Services.AddScoped<ICommandHandler<AddPermissionCommand, PermissionViewModel>, AddPermissionCommandHandler>();
//builder.Services.AddScoped<ICommandHandler<UpdatePermissionCommand, bool>, UpdatePermissionCommandHandler>();
//builder.Services.AddScoped<ICommandHandler<DeletePermissionCommand, bool>, DeletePermissionCommandHandler>();
#endregion

builder.Services.AddScoped<IKafkaService, KafkaService>();

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
