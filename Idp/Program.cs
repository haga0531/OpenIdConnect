using Idp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var context = new Context
{
    Users = new List<User>
    {
        new(1, "tiny-idp@example.com", "password", "tiny-client")
    },
    AuthCodes = new List<AuthCode>()
};

builder.Services.AddSingleton(context);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
