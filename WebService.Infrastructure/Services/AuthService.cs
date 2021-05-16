using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Text;
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
        private readonly IHttpClientFactory _clientFactory;

        public AuthService(
            IJwtTokenService jwtTokenService, ApplicationContext context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// авторизация по логин/пароль
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> Authorize
            (string login, string password, CancellationToken ct)
        {
            await CheckPasswordAsync(login, password, ct);

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
        private async Task CheckPasswordAsync
            (string login, string password, CancellationToken ct)
        {
            User user = await _context.User
                .FirstOrDefaultAsync(x => x.UserName == login, ct);

            if (user == null)
                throw new Exception("логин не существует");

            if (user.Password != password)
                throw new Exception("пароль неверный");
        }

        /// <summary>
        /// генерирование кода для пользователя
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> SendAccesTokenToSmsAsync(
            string phone, CancellationToken ct)
        {
            var user = await _context.User
               .FirstOrDefaultAsync(x => x.PhoneNumber.Equals(phone), ct);

            if (user == null)
                throw new Exception("пользователей не найден");

            var code = GeneratePhoneNumberTokenAsync();

            user.PhoneCode = code;

            _context.User.Update(user);
            await _context.SaveChangesAsync(ct);

            var message = $"код для доступа: {code}";

            var userName = "prodevkir@mail.ru";
            var password = "bX2KWyrQXCu4ujDnccUL9sKq25e";

            var request = new HttpRequestMessage(HttpMethod.Get,
               $"https://gate.smsaero.ru/v2/sms/send?number={phone}&text={message}&sign=SMS Aero");

            var b = Encoding.ASCII.GetBytes($"{userName}:{password}");
            var g = Convert.ToBase64String(b);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", g);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request, ct);

            return true;
        }

        /// <summary>
        /// генерирование 4-х значного кода
        /// </summary>
        /// <returns></returns>
        private string GeneratePhoneNumberTokenAsync()
        {
            var code = string.Empty;

            var rnd = new Random();
            var start = rnd.Next(9, 99);

            var end = DateTime.Now.Second;

            code = start.ToString() + end.ToString();

            return code;
        }

        /// <summary>
        /// сравнение сгенерированного кода с сохранённым
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<LoginResponseDto> CheckPhoneAccessTokenAsync(
            string phone, string code, CancellationToken ct)
        {
            var user = await _context.User
               .FirstOrDefaultAsync(x => x.PhoneNumber.Equals(phone), ct);

            if (user == null)
                throw new Exception("пользователей не найден");

            if (code == user.PhoneCode)
                return await Authorize(user.UserName, user.Password, ct);
            else
                return null;
        }

    }
}
