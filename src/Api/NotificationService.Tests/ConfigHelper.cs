using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Tests
{
    internal class ConfigHelper
    {
        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("testSettings.json")
                .Build();

            return config;
        }

        public static IConfiguration GetMailingConfig()
        {
            return InitConfiguration().GetSection("MailConfig");
        }
    }
}
