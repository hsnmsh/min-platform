using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinPlatform.Caching.Service;
using MinPlatform.ConfigStore.Service;
using MinPlatform.Data.Service;
using MinPlatform.Data.Service.Lookup;
using MinPlatform.Data.Service.Models;
using MinPlatform.DI.Service;
using MinPlatform.Logging.Abstractions.Models;
using MinPlatform.Logging.Service;
using MinPlatform.Notifications.Service;
using MinPlatform.Tenant.Service.Models;
using MinPlatform.Validators.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinPlatform.NetService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly NotificationManager service;


        public TestController(NotificationManager service)
        {
            this.service = service;

        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetBookByIdAsync()
        {
            var result = await service.GetNotificationsAsync("Notification 2", "ar");
            var result1 = await service.SendNotificationAsync(new Notifications.Abstractions.NotificationInputs
            {
                Inputs = new Dictionary<string, object>()
                {
                    {"Recipients", new List<string>{"to@test.com" } }
                },
                PlaceHolders = new Dictionary<string, object>()
                {
                    {"Title","زيادة رصيد" },
                    {"CustomerName","Test" },
                    {"NumberOfCredits",20 },


                },
                Notification = result.First()
            });

            return Ok(result1);

        }
    }
}
