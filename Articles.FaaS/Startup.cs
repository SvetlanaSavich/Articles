using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using Articles.Data;
using Articles.Data.Articles;
using Articles.Data.Comments;
using Articles.Data.Users;
using Articles.FaaS;
using Articles.Services.ArticleManagement;
using Articles.Services.CommentManagement;
using Articles.Services.UserManagement;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Articles.FaaS
{
    public class Startup : IWebJobsStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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

            AddMapper(services);
        }

        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly());

            ConfigureServices(builder.Services);
        }

        //private static void AddAuthentication(IServiceCollection services)
        //{
        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(options =>
        //        {
        //            options.RequireHttpsMetadata = false;
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuer = true,
        //                ValidIssuer = AuthOptions.Issuer,
        //                ValidateAudience = true,
        //                ValidAudience = AuthOptions.Audience,
        //                ValidateLifetime = true,
        //                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        //                ValidateIssuerSigningKey = true,
        //            };
        //        });
        //}

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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        private void AddMongoDbContext(IServiceCollection services)
        {
            var connectionsString =
                "mongodb://testdb0301:UJhpltNVlMsCz93PIWJGoUhqIFQSfBGv7juzRbA20j2BIZhYpWUUMoUhLKocpnFUGqMHKTNw9SStUNcZfY4QIg==@testdb0301.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@testdb0301@";


            ConventionRegistry.Register("Camel Case", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);

            services.AddSingleton<IMongoClient>(s => new MongoClient(connectionsString));

            var dbName = "articlesdb";

            services.AddScoped(s => new MongoDbContext(s.GetRequiredService<IMongoClient>(), dbName));
        }
    }
}