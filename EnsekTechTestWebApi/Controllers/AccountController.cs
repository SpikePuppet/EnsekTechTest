using EnsekTechTestModels.Models;
using EnsekTechTestServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnsekTechTestWebApi.Controllers
{
    [Route("account")]
    public class AccountController : EntityController<Account>
    {
        public AccountController(IEntityService<Account> entityService) : base(entityService)
        {
        }
    }
}