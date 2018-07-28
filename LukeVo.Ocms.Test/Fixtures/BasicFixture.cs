using LukeVo.Ocms.Api.Models;
using LukeVo.Ocms.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace LukeVo.Ocms.Test.Fixtures
{
    public class BasicFixture : IDisposable
    {

        public OcmsContext DbContext { get; private set; }
        public IConfiguration Config { get; private set; }
        public AppSettings AppSettings { get; private set; }

        public BasicFixture()
        {
            var options = new DbContextOptionsBuilder<OcmsContext>()
                .UseInMemoryDatabase("Ocms")
                .Options;

            this.DbContext = new OcmsContext(options);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            this.Config = builder.Build();

            this.AppSettings = new AppSettings();
            this.Config.Bind(this.AppSettings);
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }

    }
}
