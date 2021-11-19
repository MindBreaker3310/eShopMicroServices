using System;
using System.Net;
using System.Threading.Tasks;
using Basket.API.Models;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
        //[HttpGet("{userName}", Name = "GetBasket")]
        //[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        //{
        //    var basket = await _repository.GetBasket(userName);
        //    return Ok(basket ?? new ShoppingCart(userName));
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            var updatedCart = await _repository.UpdateBasket(basket);

            return Ok(updatedCart);
        }


        [HttpDelete("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
