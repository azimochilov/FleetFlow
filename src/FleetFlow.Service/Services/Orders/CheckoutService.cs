﻿using AutoMapper;
using FleetFlow.DAL.IRepositories;
using FleetFlow.Domain.Entities.Addresses;
using FleetFlow.Domain.Entities.Orders;
using FleetFlow.Domain.Enums;
using FleetFlow.Service.DTOs.Address;
using FleetFlow.Service.Exceptions;
using FleetFlow.Service.Interfaces.Addresses;
using FleetFlow.Service.Interfaces.Orders;
using FleetFlow.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FleetFlow.Service.Services.Orders
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IAddressService addressService;
        private readonly IRepository<Order> orderRepository;
        private readonly IMapper mapper;
        public CheckoutService(IRepository<Order> orderRepository,
            IAddressService addressService,
            IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.addressService = addressService;
            this.mapper = mapper;
        }

        public async ValueTask<AddressForResultDto> AssignAddressAsync(AddressForCreationDto addressDto)
        {
            var order = await orderRepository.SelectAll(o => o.UserId == HttpContextHelper.UserId
                && o.Status == OrderStatus.Checkout)
                .OrderBy(o => o.Id)
                .LastOrDefaultAsync();
            if (order == null)
                throw new FleetFlowException(404, "Order not found in 'checkout' status");

            order.Address = mapper.Map<Address>(addressDto);

            await orderRepository.SaveAsync();

            return mapper.Map<AddressForResultDto>(order.Address);
        }

        public async ValueTask<AddressForResultDto> RetrieveLastAddressAsync()
        {
            var order = await orderRepository.SelectAll(o => o.UserId == HttpContextHelper.UserId)
                .OrderBy(o => o.Id)
                .LastOrDefaultAsync();

            return await addressService.GetByIdAsync(order?.AddressId ?? 0);
        }
    }
}
