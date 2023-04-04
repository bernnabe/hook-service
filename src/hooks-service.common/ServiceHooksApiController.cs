using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ServiceHooks.Common
{
    [ApiController]
    public class ServiceHooksApiController : ControllerBase
    {
        protected readonly IMapper _Mapper = null;
        static ServiceHooksApiController()
        {

        }

        public ServiceHooksApiController(IMapper mapper)
        {
            _Mapper = mapper;
        }
    }
}
