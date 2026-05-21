using System;
using System.Threading.Tasks;

namespace Facturation.Web.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private UserSession? _currentUser;

        public bool IsAuthenticated => _currentUser != null;

        public UserSession? CurrentUser => _currentUser;

        public event Action? OnChange;

        public async Task<bool> LoginAsync(string email, string password)
        {
            // Simulate network delay for a real feeling login experience
            await Task.Delay(800);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Normalise the email
            var normalizedEmail = email.Trim().ToLowerInvariant();

            // Demo accounts
            if (normalizedEmail == "admin@delta.com" && password == "admin123")
            {
                _currentUser = new UserSession
                {
                    Username = "admin",
                    Email = "admin@delta.com",
                    FullName = "Directeur Général",
                    Role = "Administrateur",
                    AvatarColor = "#1565C0"
                };
                NotifyStateChanged();
                return true;
            }
            else if (normalizedEmail == "commercial@delta.com" && password == "commercial123")
            {
                _currentUser = new UserSession
                {
                    Username = "commercial",
                    Email = "commercial@delta.com",
                    FullName = "Sami Ben Ali",
                    Role = "Commercial",
                    AvatarColor = "#00BFA5"
                };
                NotifyStateChanged();
                return true;
            }

            return false;
        }

        public Task LogoutAsync()
        {
            _currentUser = null;
            NotifyStateChanged();
            return Task.CompletedTask;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
