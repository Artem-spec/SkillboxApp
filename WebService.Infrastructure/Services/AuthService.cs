using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebService.Domain.Dto.Auth;
using WebService.Domain.ServicesContract;
using WebService.Infrastructure.Context;
using WebService.Infrastructure.Entity;

namespace WebService.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ApplicationContext _context;

        public AuthService(IJwtTokenService jwtTokenService, ApplicationContext context)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponseDto> Authorize
            (string login, string password, CancellationToken ct)
        {
            bool checkUser = await CheckPasswordAsync(login, password, ct);
            if (!checkUser)
                return null;

            string token = await _jwtTokenService.CreateTokenAsync(login, ct);
            int id = await _jwtTokenService.GetIdUserAsync(login, ct);

            var result = new LoginResponseDto()
            {
                Id = id,
                Token = token
            };

            return result;
        }


        /// <summary>
        /// проверка на наличие пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<bool> CheckPasswordAsync
            (string login, string password, CancellationToken ct)
        {
            User user = await _context.User
                .FirstOrDefaultAsync(x => x.UserName == login, ct);

            if (user == null)
                return false;

            return user.Password == password;
        }

    }
}
