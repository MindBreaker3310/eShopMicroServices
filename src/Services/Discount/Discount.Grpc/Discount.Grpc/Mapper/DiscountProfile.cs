using System;
using AutoMapper;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class DiscountProfile :Profile
    {
        public DiscountProfile()
        {
            //讓Coupon與CouponModel可以方便地互相轉換
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
