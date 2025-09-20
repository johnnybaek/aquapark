using AquaparkApp.DAL;
using AquaparkApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace AquaparkApp.BLL
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepository;

        public AuthenticationService(string connectionString)
        {
            _userRepository = new UserRepository();
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            if (VerifyPassword(password, user.PasswordHash))
            {
                await _userRepository.UpdateLastLoginAsync(user.Id);
                return user;
            }

            return null;
        }

        public async Task<User?> RegisterAsync(User user, string password)
        {
            // Проверяем, не существует ли уже пользователь с таким именем или email
            var existingUser = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUser != null) return null;

            existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null) return null;

            // Хешируем пароль
            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            try
            {
                var userId = await _userRepository.CreateAsync(user);
                user.Id = userId;
                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || !VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> ValidatePasswordAsync(int userId, string password)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user != null && VerifyPassword(password, user.PasswordHash);
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetTopUsersBySpendingAsync(int limit = 10)
        {
            return await _userRepository.GetTopUsersBySpendingAsync(limit);
        }

        public async Task<IEnumerable<User>> GetUsersByRegistrationDateAsync(DateTime startDate, DateTime endDate)
        {
            return await _userRepository.GetUsersByRegistrationDateAsync(startDate, endDate);
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _userRepository.GetActiveUsersCountAsync();
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            return await _userRepository.DeactivateUserAsync(userId);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }
}