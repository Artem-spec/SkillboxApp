using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WebService.Domain.Dto;
using WebService.Domain.Dto.Auth;
using WebService.Domain.ServicesContract;

namespace WebService.API.Controllers
{
    /// <summary>
    /// контроллер авторизации пользователей
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        /// <summary>
        /// инициализация контроллера авторизации пользователей
        /// </summary>
        /// <param name="service"></param>
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        /// <summary>
        /// авторизация
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginResponseDto> AuthUser
            ([FromBody] AuthorizeQuery query, CancellationToken ct = default)
        {
            return await _service.Authorize(query.UserName, query.Password, ct);
        }
    }
}
