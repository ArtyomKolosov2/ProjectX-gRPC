using System;
using AspNetCore.Identity.Mongo;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using ProjectX.DataAccess.Models.Base;
using ProjectX.DataAccess.Models.Identity;

namespace ProjectX.DataAccess.Models.DI
{
    public static class IdentityConfiguration
    {
        private const string AllowedNameChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        
        public static void AddMongoDbIdentity(this IServiceCollection services, IDatabaseSettings databaseSettings)
        {
            services.AddIdentityMongoDbProvider<User, Role, ObjectId>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = true;
                
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.AllowedForNewUsers = true;
                
                identityOptions.User.AllowedUserNameCharacters = AllowedNameChars;
                identityOptions.User.RequireUniqueEmail = true;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString =
                    $"{databaseSettings.ConnectionString}/{databaseSettings.DatabaseName}";
            });
        }
    }
}