using MeterReadingModels.Models;
using MeterReadingServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeterReadingWebApi.Controllers
{
    [Route("account")]
    public class AccountController : EntityController<Account>
    {
        public AccountController(IEntityService<Account> entityService) : base(entityService)
        {
        }
    }
}