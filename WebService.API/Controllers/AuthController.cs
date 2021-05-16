using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebService.Domain.Query.Auth;
using WebService.Domain.ServicesContract;

namespace WebService.API.Controllers
{
    /// <summary>
    /// контроллер авторизации пользователей
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthUser
            ([FromBody] AuthorizeQuery query, CancellationToken ct = default)
        {
            try
            {
                var token = await _service.Authorize(query.UserName, query.Password, ct);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("send-sms")]
        public async Task<IActionResult> SendAccesTokenToSms
            ([FromBody] PhoneAuthorizeQuery query, CancellationToken ct = default)
        {
            try
            {
                await _service.SendAccesTokenToSmsAsync(query.Phone, ct);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("check-sms")]
        public async Task<IActionResult> CheckPhoneAccessToken
            ([FromBody] CheckPhoneAuthorizeQuery query, CancellationToken ct = default)
        {
            try
            {
                var token = await _service.CheckPhoneAccessTokenAsync(query.Phone, query.Code, ct);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
    }
}
