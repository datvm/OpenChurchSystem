using LukeVo.Ocms.Api.Models.Entities;
using ServiceSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LukeVo.Ocms.Api.Models.Services
{

    public abstract class BaseService : IService
    {

        protected OcmsContext DbContext { get; set; }

        public BaseService(OcmsContext dbContext)
        {
            this.DbContext = dbContext;
        }

    }

}
