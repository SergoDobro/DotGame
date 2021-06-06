using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotGameOnline.Game;

namespace DotGameOnline
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<FinalGrid>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            for (int i = 0; i < 10; i++)
            {
                app.ApplicationServices.GetService<FinalGrid>().SetPixel(5 + i, 5, "SerDob");
                app.ApplicationServices.GetService<FinalGrid>().SetPixel(4, 6 + i, "SerDob");
                app.ApplicationServices.GetService<FinalGrid>().SetPixel(5 + i, 16, "SerDob");
                app.ApplicationServices.GetService<FinalGrid>().SetPixel(15, 6 + i, "SerDob");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("", async context =>
                {
                    string page = File.ReadAllText(@"Pages\dotgamepage.html");
                    await context.Response.WriteAsync(page);
                });
                endpoints.MapPost("/GetPoints", async context =>
                {
                    Get_WhatPartOfMap dataWeGet = await context.Request.ReadFromJsonAsync<Get_WhatPartOfMap>();
                    var dotsAndPlayers = new
                    {
                        dots = app.ApplicationServices.GetService<FinalGrid>().GetPointsInZone(dataWeGet.X, dataWeGet.Y, dataWeGet.Width, dataWeGet.Height).Keys.Select(x => new object[3] { x.x, x.y, x.playerID }),
                        players = app.ApplicationServices.GetService<FinalGrid>().GetPlayers.Select(x => new KeyValuePair<object, string>(x.Key as object, x.Value.GetColor().ToString()))
                    };
                    await context.Response.WriteAsJsonAsync(dotsAndPlayers);
                });
                endpoints.MapPost("/SetPoint", async context =>
                {
                    SendPoint point = await context.Request.ReadFromJsonAsync<SendPoint>();
                    app.ApplicationServices.GetService<FinalGrid>().SetPixel(point.x, point.y, point.playerID);
                    await context.Response.WriteAsJsonAsync("");
                });
            });
        }
    }
}
