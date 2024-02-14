using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OPM.SFS.Web.SharedCode;

namespace OPM.SFS.Tests
{
    [TestClass]
    public class AccountInactiveTests
    {
        private readonly IAccountInactiveService service;
        //protected IMatchService MatchService { get; set; }

        //protected Mock<IMatchService> MockedMatchService => Mock.Get(MatchService);

        //private IServiceProvider ServicesProvider { get; set; }
        //public void TestMe()
        //{
        //    // Configure DI container
        //    ServiceCollection services = new ServiceCollection();
        //    ConfigureServices(services);
        //    ServicesProvider = services.BuildServiceProvider();

        //    // Use DI to get instances of IMatchService
        //    MatchService = ServicesProvider.GetService<IMatchService>();


        //}

        //private static void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddScoped<IMatchService>();
        //    services.AddScoped<MatchController>();
        //}
    }
}
