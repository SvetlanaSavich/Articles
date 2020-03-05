using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Articles.Data;
using Articles.Data.Articles;
using Articles.Data.Comments;
using Articles.Data.Users;
using Articles.Services.ArticleManagement;
using Articles.Services.CommentManagement;
using Articles.Services.UserManagement;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Articles.API
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
            services.AddControllers();

            AddMongoDbContext(services);

            //AddLocalDbContext(services);

            services.AddTransient<IUserRepository, MongoUserRepository>();

            services.AddTransient<UserService>();

            services.AddTransient<IArticleRepository, MongoArticleRepository>();

            services.AddTransient<ArticleService>();

            services.AddTransient<IArticleCategoryRepository, MongoArticleCategoryRepository>();

            services.AddTransient<ArticleCategoryService>();

            services.AddTransient<ICommentRepository, MongoCommentRepository>();

            services.AddTransient<CommentService>();

            AddSwagger(services);

            AddMapper(services);

            AddAuthentication(services);
        }

        private void AddLocalDbContext(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("ArticlesConnection");

            services.AddDbContext<ArticlesDbContext>(options =>
                options.UseSqlServer(connection, x => x.MigrationsAssembly("Articles.Data")));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Articles API V1");
                c.RoutePrefix = string.Empty;
                //  c.DocExpansion(DocExpansion.None);
            });


            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
        }

        private static void AddMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CommentMappingProfile());
                mc.AddProfile(new UserMappingProfile());
                mc.AddProfile(new ArticleMappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Articles API",
                    Version = "v1",
                    Description = "A simple web site for users to create/submit articles and post comments."
                });

                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                                        Enter 'Bearer' [space] and then your token in the text input below.
                                        \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Type = SecuritySchemeType.ApiKey, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "Bearer", //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void AddMongoDbContext(IServiceCollection services)
        {
            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            services.AddSingleton<IMongoClient>(s => new MongoClient(Configuration.GetConnectionString("MongoConnection")));

            services.AddScoped(s => new MongoDbContext(s.GetRequiredService<IMongoClient>(), Configuration["DbName"]));
        }
    }
}
