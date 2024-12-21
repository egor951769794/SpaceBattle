using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


namespace SpaceBattle.Lib;


public class Start
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        // app.MapGet("/", () => "сырники");
        app.UseWelcomePage();

        app.Run(async (context) =>
        {
            // var response = context.Response;
            // var request = context.Request;
            // var path = request.Path;

            // if (path == "/game/migrate")
            // {

            // }
        });
    }
}