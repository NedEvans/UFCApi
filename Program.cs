using Microsoft.EntityFrameworkCore;
using UFCApi.DB;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,                // how many retries
            maxRetryDelay: TimeSpan.FromSeconds(20), // wait between
            errorNumbersToAdd: null          // custom SQL errors, usually leave null
        )
    ));

builder.Services.AddControllers();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy
            .WithOrigins("https://app.nedevans.au")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Enable CORS - add this before other middleware
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.Urls.Add("http://0.0.0.0:8080");
app.Urls.Add("http://0.0.0.0:5227");
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
