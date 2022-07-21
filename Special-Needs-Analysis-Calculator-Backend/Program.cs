using Microsoft.EntityFrameworkCore;
using Special_Needs_Analysis_Calculator.Data;
using Special_Needs_Analysis_Calculator.Data.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<SpecialNeedsAnalysisDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("SpecialNeedsAnalysisDb"));
});

builder.Services.AddTransient<IDatabaseCrud, DatabaseCrud>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//app.Seed();

app.Run();
