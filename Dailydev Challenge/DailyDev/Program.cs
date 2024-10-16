using DailyDev.Repositories;
using DailyDev.Repository;
using DailyDev.Service;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add repositories to the DI container
builder.Services.AddScoped<ProviderRepository>(provider => new ProviderRepository(connectionString));
builder.Services.AddScoped<CategoryRepository>(provider => new CategoryRepository(connectionString));
builder.Services.AddScoped<ItemRepository>(provider => new ItemRepository(connectionString));
builder.Services.AddScoped<TagRepository>(provider => new TagRepository(connectionString));
builder.Services.AddScoped<ItemTagRepository>(provider => new ItemTagRepository(connectionString));
builder.Services.AddScoped<UserRepository>(provider => new UserRepository(connectionString));
builder.Services.AddScoped<UserProviderRepository>(provider => new UserProviderRepository(connectionString));
builder.Services.AddScoped<UserCategoryRepository>(provider => new UserCategoryRepository(connectionString));
builder.Services.AddScoped<UserTagRepository>(provider => new UserTagRepository(connectionString));
builder.Services.AddScoped<TableConfigRepository>(provider => new TableConfigRepository(connectionString));
builder.Services.AddScoped<UserItemRepository>(provider => new UserItemRepository(connectionString));


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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
