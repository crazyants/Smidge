﻿using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Routing;
using Smidge;
using Smidge.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;
using Smidge.Controllers;
using Smidge.Options;
using Smidge.FileProcessors;

namespace Smidge.Web
{
   

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddMvc();

            services.AddSmidge()
                .Configure<SmidgeOptions>(options =>
                {                    
                })
                .Configure<Bundles>(bundles =>
                {
                    bundles.Create("test-bundle-1",
                        new JavaScriptFile("~/Js/Bundle1/a1.js"),
                        new JavaScriptFile("~/Js/Bundle1/a2.js"));

                    bundles.Create("test-bundle-2", WebFileType.Js, "~/Js/Bundle2");

                    bundles.Create("test-bundle-3", bundles.PipelineFactory.GetPipeline(typeof(JsMin)), WebFileType.Js, "~/Js/Bundle2");
                    
                    bundles.Create("test-bundle-4",
                        new CssFile("~/Css/Bundle1/a1.css"),
                        new CssFile("~/Css/Bundle1/a2.css"));
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseErrorPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSmidge();
        }
    }
}
