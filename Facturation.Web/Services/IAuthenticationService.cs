using System;
using System.Threading.Tasks;

namespace Facturation.Web.Services
{
    public class UserSession
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string AvatarColor { get; set; } = "#1565C0";
    }

    public interface IAuthenticationService
    {
        bool IsAuthenticated { get; }
        UserSession? CurrentUser { get; }
        event Action? OnChange;
        
        Task<bool> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
