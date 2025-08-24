using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// isco => add aws lambda hosting for minimal apis
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

app.MapPost("api/short-url", async (CreateShortUrlRequest request, ApplicationDbContext context) =>
{
    ShortUrl shortUrl = ShortUrl.Create(request.Url);

    await context.AddAsync(shortUrl);
    await context.SaveChangesAsync();

    return Results.Created($"api/short-url/{shortUrl.Code}", shortUrl);
});

app.MapGet("api/short-url/{code}", async (string code, ApplicationDbContext context) =>
{
    ShortUrl shortUrl = await context.ShortUrls.AsNoTracking()
                                     .Where(x => x.Code == code)
                                     .FirstOrDefaultAsync()
                                     .ConfigureAwait(false);
    
    if (shortUrl is null)
    {
        return Results.NotFound();
    }

    return Results.Redirect(shortUrl.Url);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();