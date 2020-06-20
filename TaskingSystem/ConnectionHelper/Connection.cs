using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TaskingSystem.ConnectionHelper
{
    public class Connection
    {
        public static string GetApiConnectionString()
        {
            return ConfigurationManager.AppSettings["MySeverUrl"];
        }
    }
}