using PlanetGame.Client.Blazor;

var app = WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .ConfigureApplication();

app.Run();
