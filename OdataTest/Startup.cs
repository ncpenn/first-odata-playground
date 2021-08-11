using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OdataTest.Models.ProductService.Models;
using OdataTest.Models;
using OdataTest.Database;
using OdataTest.ODataSecurity;

namespace OdataTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(o => {
                o.DefaultScheme = SchemesNamesConst.TokenAuthenticationDefaultScheme;
            })
            .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(SchemesNamesConst.TokenAuthenticationDefaultScheme, o => { });

            services.AddDbContext<Database.NathanContext>();
            services.AddTransient<MyRepository>();
            services
                .AddControllers()
                .AddOData(opt =>
                {
                    SecuredMetadataRoutingConvention.ReplaceMetadataRoutingConvention(opt);
                    opt.AddRouteComponents("od", GetEdmModel()).Select().Filter().Count().Expand().SetMaxTop(100);
                });



            services.AddRouting();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OdataTest", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, NathanContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OdataTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            

            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.MapControllers();
            //});

            InitializeDatabase(context);
        }

        private void InitializeDatabase(NathanContext context)
        {
            var accessory = new Accessory { AccessoryId = 4 };
            var product = new Product { Id = 1, Name = "hello world" };
            product.Accessories.Add(accessory);
            context.Add(product);
            context.SaveChanges();
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Product>("Accessories");
            return builder.GetEdmModel();
        }
    }
}
