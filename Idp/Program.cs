using Idp.Models;
using Idp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var context = new Context
{
    Users = new List<User>
    {
        new(1, "tiny-idp@example.com", "aaaa", "tiny-client")
    },
    AuthCodes = new List<AuthCode>(),
    AccessTokens = new List<AccessToken>(),
    Clients = new List<Client>
    {
        new("tiny-client", "c1!3n753cr37")
    }
};

builder.Services.AddSingleton(context);

builder.Services.AddSingleton<JwtService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
