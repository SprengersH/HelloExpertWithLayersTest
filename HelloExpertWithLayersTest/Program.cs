using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// add layers?

// db
var connectionString = builder.Configuration.GetConnectionString("HEconnection") ?? throw new InvalidOperationException("Connection string 'HEconnection' not found.");
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(
        connectionString);
});



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
