using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.GrpcServices;
using Basket.API.Models;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _repository = repository;
            _discountGrpcService = discountGrpcService;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        //api/v1/BasketController/{userName}
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            if (basket == null)
            {
                basket = new ShoppingCart(userName);
            }

            return Ok(basket);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //呼叫Discount.Grpc來找折扣碼
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }


        [HttpDelete("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)//雖然說可以直接用BasketCheckoutEvent類型，但是分開寫日後維護上比較輕鬆。
        {
            //取得購物車內總金額
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if(basket == null)
            {
                return BadRequest();
            }

            //建立BasketCheckout Event
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            //設定Event裡面的總金額
            eventMessage.TotalPrice = basket.TotalPrice;
            //傳送Event給RabbitMQ
            await _publishEndpoint.Publish(eventMessage);
            //清空購物車
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}
