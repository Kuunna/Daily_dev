using DailyDev.Models;
using DailyDev.Repositories;
using DailyDev.Repository;
using DailyDev.Service;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add repositories to the DI container
builder.Services.AddScoped<ProviderRepo>(provider => new ProviderRepo(connectionString));
builder.Services.AddScoped<CategoryRepo>(provider => new CategoryRepo(connectionString));
builder.Services.AddScoped<ItemRepo>(provider => new ItemRepo(connectionString));
builder.Services.AddScoped<TagRepo>(provider => new TagRepo(connectionString));
builder.Services.AddScoped<ItemTagRepo>(provider => new ItemTagRepo(connectionString));
builder.Services.AddScoped<UserRepo>(provider => new UserRepo(connectionString));
builder.Services.AddScoped<UserProviderRepo>(provider => new UserProviderRepo(connectionString));
builder.Services.AddScoped<UserCategoryRepo>(provider => new UserCategoryRepo(connectionString));
builder.Services.AddScoped<UserTagRepo>(provider => new UserTagRepo(connectionString));
builder.Services.AddScoped<TableConfigRepo>(provider => new TableConfigRepo(connectionString));
builder.Services.AddScoped<UserItemRepo>(provider => new UserItemRepo(connectionString));
builder.Services.AddScoped<ItemCommentRepo>(provider => new ItemCommentRepo(connectionString));

// Add OData services
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())
    .Filter().Select().Expand().OrderBy().SetMaxTop(100).Count());
// Define the EDM (Entity Data Model) for OData
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Category>("Category");
    return builder.GetEdmModel();
}

// Đăng ký HttpClient, Repositories và BackgroundService
builder.Services.AddHttpClient();

// Đăng ký UpdateService vào DI container
builder.Services.AddHostedService<UpdateService>(); // <-- Đăng ký Background Service

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
