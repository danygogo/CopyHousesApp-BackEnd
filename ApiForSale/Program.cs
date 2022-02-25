
using ApiForSale.Data;
using Microsoft.EntityFrameworkCore;



string _myCors = "myCors";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<Database>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _myCors, builder =>
    {
        
        builder.WithOrigins("https://localhost:4200", "https://ashy-mud-015264910.1.azurestaticapps.net", "localhost")
        .AllowAnyHeader().AllowAnyMethod();
        

        /*
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
        .AllowAnyHeader().AllowAnyMethod();
        */
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(_myCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
