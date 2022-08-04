using CqrsTest.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("CqrsTest"));
//builder.Services.AddScoped<DbInitializer>(); //nie dziala

builder.Services.Scan(s => s.FromAssemblyOf<IRepository>()
    .AddClasses(c => c.AssignableTo<IRepository>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

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
