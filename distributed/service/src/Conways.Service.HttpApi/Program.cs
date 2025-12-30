using Conways.Service.Application;
using Conways.Service.Domain;
using Conways.Service.HttpApi.Extensions;
using Conways.Service.Infrastructure.MongoDb;

var builder = WebApplication.CreateBuilder(args);

#region // Add services to the container

builder.Services.AddHealthChecks();

builder.Services.AddDomainLayerServices();
builder.Services.AddApplicationLayerServices(builder.Configuration);
builder.Services.AddInfrastructureMongoDbServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion // Add services to the container

var app = builder.Build();

#region // Add middlewares to the pipeline

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsBuilder =>
{
    corsBuilder.WithOrigins("http://localhost:3000") // React client
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // Just for SignalR
});

app.UseReadynessHealthChecks();
app.UseLivenessHealthCheck();

app.MapControllers();

#endregion // Add middlewares to the pipeline

app.Run();