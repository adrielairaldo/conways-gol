using Conways.Service.Domain;
var builder = WebApplication.CreateBuilder(args);

#region // Add services to the container

builder.Services.AddDomainLayerServices();
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


#endregion // Add middlewares to the pipeline

app.Run();