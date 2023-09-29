using SpotifyPlaylistRadio.Models;
using SpotifyPlaylistRadio.Services;
using SpotifyPlaylistRadio.Socket;
using System.Net.WebSockets;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //ConfigurationManager configuration = builder.Configuration;

        // Add services to the container.

        builder.Services.AddHttpClient<ISpotifyAccountService, SpotifyAccountService>(c =>
        {
            c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
        });

        builder.Services.AddHttpClient<ISpotifyService, SpotifyService>(c =>
        {
            c.BaseAddress = new Uri("https://api.spotify.com/v1/");
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.Configure<SpotifyPlaylistsFromRadioDatabaseSettings>(
            builder.Configuration.GetSection("MongoDB"));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ClientPermission", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000");
                //.AllowCredentials();
            });
        });

        builder.Services.AddSingleton<ISocketMessages, SocketMessage>();
        builder.Services.AddSingleton<ISearchHelperService, SearchHelperService>();
        builder.Services.AddScoped<IInitService, InitService>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<RadiosService>();
        builder.Services.AddSingleton<PlaylistService>();
        builder.Services.AddSingleton<MusicSpotifyService>();
        builder.Services.AddSingleton<ArtistService>();

        builder.Services.AddControllers();
        //.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors("ClientPermission");

        //var webSocketOptions = new WebSocketOptions
        //{
        //    KeepAliveInterval = TimeSpan.FromMinutes(2)
        //};

        //app.UseWebSockets(webSocketOptions);

        //app.Run(async (context) =>
        //{
        //    if (context.WebSockets.IsWebSocketRequest)
        //    {
        //        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

        //        using (var serviceScope = app.Services.CreateScope())
        //        {
        //            var services = serviceScope.ServiceProvider;

        //            var init = services.GetRequiredService<IInitService>();

        //            Use the service
        //            await init.something(context/*, webSocket*/);
        //        }
        //    }
        //});

        app.UseRouting();

        app.UseHttpsRedirection();

        app.MapControllers();
        app.Run();

    }
}