using AuthService.API.Config;
using AuthService.API.Utils.Middlewares;
using AuthService.Application.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GloblalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
