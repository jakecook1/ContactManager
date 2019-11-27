using System;
using System.IO;
using ContactManagerWeb.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ContactManagerWeb.Helpers
{
    public static class DbValues
    {
        public static string GetDefaultConnection(IWebHostEnvironment env)
        {
            var values = GetValues(env);
            var connectionString = string.Empty;

            if (env.IsDevelopment())
            {
                if (values.Host == StringConstants.LocalHostName)
                    connectionString = "Host={0};Port={1};Username={2};Password={3};Database={4};";
                else
                    connectionString = "Host={0};Port={1};User ID={2};Password={3};Database={4};Pooling=true;SSL Mode=Require;TrustServerCertificate=True;";
            }
            else
            {
                connectionString = "Host={0};Port={1};User ID={2};Password={3};Database={4};Pooling=true;SSL Mode=Require;TrustServerCertificate=True;";
            }

            return string.Format(connectionString, values.Host, values.Port, values.User, values.Password, values.Name);
        }

        public static DbValue GetValues(IWebHostEnvironment env)
        {
            string databaseUrl = string.Empty;

            if (env.IsDevelopment())
            {
                var settings = GetSettings(useLocal: true);

                var host = settings[0];
                var database = settings[1];
                var userName = settings[2];
                var password = settings[3];
                var port = settings[4];

                databaseUrl = string.Format("postgres://{0}:{1}@{2}:{3}/{4}",
                    userName, password, host, port, database);
            }
            else
            {
                databaseUrl = Environment.GetEnvironmentVariable(StringConstants.DatabaseUrl);
            }

            if (string.IsNullOrEmpty(databaseUrl))
                return new DbValue();

            var dbBits = databaseUrl.Split('/');

            var dbBits1 = dbBits[2].Split('@');

            return new DbValue
            {
                Host = dbBits1[1].Split(':')[0],
                Name = dbBits[3],
                User = dbBits1[0].Split(':')[0],
                Password = dbBits1[0].Split(':')[1],
                Port = dbBits1[1].Split(':')[1]
            };
        }

        public static string[] GetSettings(bool useLocal)
        {
            var lines = File.ReadAllLines(@"./dbconnection.settings");

            var host = useLocal ? lines[0].Split('|')[0] : lines[0].Split('|')[1];
            var database = useLocal ? lines[1].Split('|')[0] : lines[1].Split('|')[1];
            var userName = useLocal ? lines[2].Split('|')[0] : lines[2].Split('|')[1];
            var password = useLocal ? lines[3].Split('|')[0] : lines[3].Split('|')[1];
            var port = useLocal ? lines[4].Split('|')[0] : lines[4].Split('|')[1];

            return new string[] { host, database, userName, password, port };
        }
    }

    public class DbValue
    {
        public string Name { get; set; }

        public string Host { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Port { get; set; }
    }
}