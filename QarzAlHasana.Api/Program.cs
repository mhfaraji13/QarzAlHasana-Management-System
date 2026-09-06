using QarzAlHasana.Api.ExceptionHandlers;
using QarzAlHasana.Infrastructure;
using QarzAlHasana.Application;
using QarzAlHasanaSystem.Application;

var builder = WebApplication.CreateBuilder(args);

// ---------- SERVICE-HA (sabt-e nam) ----------
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BusinessRuleExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------- MIDDLEWARE (masir-e request) ----------
app.UseExceptionHandler();         

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();