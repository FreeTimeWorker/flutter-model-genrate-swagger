using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flutter_model_genrate_swagger
{
    public class ServiceAgent
    {
        public static readonly List<string> BaseTypes = new List<string>() { "integer", "string", "boolean", "number" };
        private static readonly object objlock = new object();
        private static IServiceProvider _ServiceProvider;
        public static IServiceProvider Provider
        {
            get
            {
                if (_ServiceProvider == null)
                {
                    lock (objlock)
                    {
                        if (_ServiceProvider == null)
                        {
                            _ServiceProvider = ConfigureServices();
                        }
                    }
                }
                return _ServiceProvider;
            }
        }
        private static ServiceProvider ConfigureServices()
        {
            //var config = new ConfigurationBuilder()
            //       .SetBasePath(Directory.GetCurrentDirectory())
            //       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //       .Build();
            var serviceCollection = new ServiceCollection()
            //.AddSingleton<IConfiguration>(config)
            .AddHttpClient()
            .AddLogging()
            .BuildServiceProvider();
            return serviceCollection;
        }
    }
}
