using HcmMemberSearch.Provider;
using HcmMemberSearch.Provider.IProvider;
using HcmMemberSearch.Helper;
using HcmMemberSearch.Helper.IHelper;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/HcmMemberSearchLog.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMemberHelper, MemberHelper>();
builder.Services.AddScoped<IClaimHelper, ClaimHelper>();
builder.Services.AddScoped<IMembers, Members>();
builder.Services.AddScoped<IClaims, Claims>();
builder.Services.AddScoped<IPhysicians, Physicians>();
builder.Services.AddCors((setup) =>
{
    setup.AddPolicy("default", (options) =>
    {
        options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("default");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
