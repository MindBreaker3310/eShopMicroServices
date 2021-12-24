using System;
using System.Threading.Tasks;
using Ordering.Application.Models;
namespace Ordering.Application.Contracts.Infrastucture
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
