﻿using FleetFlow.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetFlow.Api.Controllers
{
    public class OrdersController : RestfulSense
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> PostAsync()
            => Ok(await this.orderService.AddAsync());


    }
}
