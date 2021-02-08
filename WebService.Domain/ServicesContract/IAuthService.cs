using System.Threading;
using System.Threading.Tasks;
using WebService.Domain.Dto.Auth;

namespace WebService.Domain.ServicesContract
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Authorize(string login, string password, CancellationToken ct);
    }
}
