using MiniLobby.Data;
using Microsoft.EntityFrameworkCore;
using MiniLobby.Interfaces;
using MiniLobby.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ILobbyRepository, LobbyRepository>();
builder.Services.AddScoped<IMembersRepository, MembersRepository>();
builder.Services.AddScoped<ILobbyDataRepository, LobbyDataRepository>();
builder.Services.AddScoped<IMemberDataRepository, MemberDataRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
