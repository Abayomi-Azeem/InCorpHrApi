using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using InCorpApp.Application.Abstractions.Authentication;
using InCorpApp.Application.Abstractions.AWS;
using InCorpApp.Application.Abstractions.Notification;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Contracts.Authentication;
using InCorpApp.Contracts.Notification;
using InCorpApp.Infrastructure.Authentication;
using InCorpApp.Infrastructure.AWS;
using InCorpApp.Infrastructure.Notification;
using InCorpApp.Infrastructure.Pesistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IS3Manager, S3Manager>();
            services.AddScoped<IEmailService, EmailService>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            //services.AddScoped<IAmazonDynamoDB, AmazonDynamoDBClient>();
            //services.AddScoped<IAmazonS3, AmazonS3Client>();

            var accessKey = configuration.GetSection("AwsSettings:AccessKey").Value;
            var secretkey = configuration.GetSection("AwsSettings:SecretKey").Value;

            var awsCredentials = new BasicAWSCredentials(accessKey, secretkey);
            var awsConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2 // Set your desired region
            };

            services.AddAWSService<IAmazonDynamoDB>(new AWSOptions
            {
                Credentials = awsCredentials,
                Region = awsConfig.RegionEndpoint
            }, ServiceLifetime.Scoped);
            services.AddAWSService<IAmazonS3>(new AWSOptions
            {
                Credentials = awsCredentials,
                Region = awsConfig.RegionEndpoint
            }, ServiceLifetime.Scoped);

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("JwtSettings:Issuer").Value,
                    ValidAudience = configuration.GetSection("JwtSettings:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(
                                            Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:SecretKey").Value)
                    )
                });

            return services;
        }

    }
}
