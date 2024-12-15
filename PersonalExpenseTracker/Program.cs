using Microsoft.AspNetCore.Hosting;
using PersonalExpenseTracker.Services;
using PersonalExpenseTracker.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ExpenseManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add the error handling middleware **before** routing and authorization
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

// Add other middleware like routing, etc.
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
