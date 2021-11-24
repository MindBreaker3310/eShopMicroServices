using System;
using System.Threading.Tasks;
using Discount.API.Models;

namespace Discount.API.Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string idproductName);
    }
}
