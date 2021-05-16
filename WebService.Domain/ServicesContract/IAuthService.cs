using System.Threading;
using System.Threading.Tasks;
using WebService.Domain.Dto.Auth;

namespace WebService.Domain.ServicesContract
{
    public interface IAuthService
    {
        /// <summary>
        /// авторизация по логин/пароль
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<LoginResponseDto> Authorize(string login, string password, CancellationToken ct);

        /// <summary>
        /// генерирование кода для пользователя
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> SendAccesTokenToSmsAsync(
             string phone, CancellationToken ct);

        /// <summary>
        /// сравнение сгенерированного кода с сохранённым
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<LoginResponseDto> CheckPhoneAccessTokenAsync(
             string phone, string code, CancellationToken ct);
    }
}
