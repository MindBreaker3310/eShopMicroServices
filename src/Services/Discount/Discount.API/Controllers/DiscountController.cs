using System;
using System.Threading.Tasks;
using Discount.API.Models;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ICouponRepository _repository;

        public DiscountController(ICouponRepository couponRepository)
        {
            _repository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository)); ;
        }


        [HttpGet("{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            Coupon coupon = await _repository.GetDiscount(productName);

            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CreateDiscount([FromBody] Coupon coupon)
        {
            bool created = await _repository.CreateDiscount(coupon);

            return Ok(created);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
        {
            bool updated = await _repository.UpdateDiscount(coupon);

            return Ok(updated);
        }

        [HttpDelete("{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            bool deleted = await _repository.DeleteDiscount(productName);

            return Ok(deleted);
        }

    }
}
