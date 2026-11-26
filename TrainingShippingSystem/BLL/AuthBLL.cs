////using System;
////using System.Collections.Concurrent;
////using System.Linq;
////using System.Security.Cryptography;
////using System.Text.RegularExpressions;
////using TrainingShippingSystem.DAL;
////using TrainingShippingSystem.Models;

////namespace TrainingShippingSystem.BLL
////{
////    public class AuthBLL
////    {
////        private class SessionInfo
////        {
////            public string Username;
////            public DateTime ExpiresAtUtc;
////        }

////        // token -> session (in-memory; cleared when the app restarts)
////        private static readonly ConcurrentDictionary<string, SessionInfo> Sessions =
////            new ConcurrentDictionary<string, SessionInfo>();

////        private static readonly TimeSpan SessionLifetime = TimeSpan.FromHours(8);

////        private static readonly Regex UsernameRegex =
////            new Regex(@"^[A-Za-z0-9_.]{3,30}$", RegexOptions.Compiled);

////        private readonly UserDAL userDAL = new UserDAL();

////        // ===== SIGN UP =====
////        public User Register(RegisterRequest request)
////        {
////            if (request == null)
////                throw new ArgumentException("Registration data is required.");

////            string username = (request.Username ?? "").Trim();
////            string password = request.Password ?? "";

////            if (username.Length == 0)
////                throw new ArgumentException("Username is required.");

////            if (!UsernameRegex.IsMatch(username))
////                throw new ArgumentException(
////                    "Username must be 3-30 characters: letters, numbers, dot or underscore.");

////            if (password.Length == 0)
////                throw new ArgumentException("Password is required.");

////            if (password.Length < 8)
////                throw new ArgumentException("Password must be at least 8 characters.");

////            if (password.Length > 100)
////                throw new ArgumentException("Password is too long.");

////            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
////                throw new ArgumentException(
////                    "Password must contain at least one letter and one number.");

////            if (password != (request.ConfirmPassword ?? ""))
////                throw new ArgumentException("Passwords do not match.");

////            byte[] hash;
////            byte[] salt;
////            PasswordHasher.CreateHash(password, out hash, out salt);

////            // The display name is the username.
////            int newID = userDAL.Register(username, username, hash, salt);

////            return new User
////            {
////                ID = newID,
////                Username = username,
////                FullName = username,
////                Role = "User"
////            };
////        }

////        // ===== SIGN IN =====
////        public User Login(string username, string password, out string token)
////        {
////            token = null;

////            if (string.IsNullOrWhiteSpace(username))
////                throw new ArgumentException("Username is required.");

////            if (string.IsNullOrEmpty(password))
////                throw new ArgumentException("Password is required.");

////            username = username.Trim();

////            if (username.Length > 100)
////                return null;

////            UserCredentials creds = userDAL.GetByUsername(username);

////            // Same answer for "no such user" and "wrong password".
////            if (creds == null || !creds.IsActive)
////                return null;

////            bool valid;

////            if (creds.PasswordSalt == null)
////            {
////                // Old account (plain SHA-256). Verify, then upgrade to salted PBKDF2.
////                valid = PasswordHasher.VerifyLegacy(password, creds.PasswordHash);

////                if (valid)
////                {
////                    try
////                    {
////                        byte[] newHash;
////                        byte[] newSalt;
////                        PasswordHasher.CreateHash(password, out newHash, out newSalt);
////                        userDAL.UpdatePassword(creds.ID, newHash, newSalt);
////                    }
////                    catch (Exception)
////                    {
////                        // Upgrade failure must not block the sign in.
////                    }
////                }
////            }
////            else
////            {
////                valid = PasswordHasher.Verify(password, creds.PasswordSalt, creds.PasswordHash);
////            }

////            if (!valid)
////                return null;

////            token = CreateToken();

////            Sessions[token] = new SessionInfo
////            {
////                Username = creds.Username,
////                ExpiresAtUtc = DateTime.UtcNow.Add(SessionLifetime)
////            };

////            return new User
////            {
////                ID = creds.ID,
////                Username = creds.Username,
////                FullName = creds.FullName,
////                Role = creds.Role
////            };
////        }

////        // ===== TOKEN HELPERS =====
////        public static string GetUserNameByToken(string token)
////        {
////            SessionInfo session;

////            if (!string.IsNullOrEmpty(token) && Sessions.TryGetValue(token, out session))
////            {
////                if (session.ExpiresAtUtc > DateTime.UtcNow)
////                    return session.Username;

////                SessionInfo expired;
////                Sessions.TryRemove(token, out expired);
////            }

////            return null;
////        }

////        public static void Logout(string token)
////        {
////            if (string.IsNullOrEmpty(token))
////                return;

////            SessionInfo removed;
////            Sessions.TryRemove(token, out removed);
////        }

////        private static string CreateToken()
////        {
////            byte[] bytes = new byte[32];

////            using (var rng = RandomNumberGenerator.Create())
////            {
////                rng.GetBytes(bytes);
////            }

////            return BitConverter.ToString(bytes).Replace("-", "");
////        }
////    }
////}


//using System;
//using System.Collections.Concurrent;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text.RegularExpressions;
//using TrainingShippingSystem.DAL;
//using TrainingShippingSystem.Models;

//namespace TrainingShippingSystem.BLL
//{
//    public class AuthBLL
//    {
//        private class SessionInfo
//        {
//            public string Username;
//            public string Role;
//            public DateTime ExpiresAtUtc;
//        }

//        // token -> session (in-memory; cleared when the app restarts)
//        private static readonly ConcurrentDictionary<string, SessionInfo> Sessions =
//            new ConcurrentDictionary<string, SessionInfo>();

//        private static readonly TimeSpan SessionLifetime = TimeSpan.FromHours(8);

//        private static readonly Regex UsernameRegex =
//            new Regex(@"^[A-Za-z0-9_.]{3,30}$", RegexOptions.Compiled);

//        private readonly UserDAL userDAL = new UserDAL();

//        // ===== SIGN UP =====
//        public User Register(RegisterRequest request)
//        {
//            if (request == null)
//                throw new ArgumentException("Registration data is required.");

//            string username = (request.Username ?? "").Trim();
//            string password = request.Password ?? "";

//            if (username.Length == 0)
//                throw new ArgumentException("Username is required.");

//            if (!UsernameRegex.IsMatch(username))
//                throw new ArgumentException(
//                    "Username must be 3-30 characters: letters, numbers, dot or underscore.");

//            if (password.Length == 0)
//                throw new ArgumentException("Password is required.");

//            if (password.Length < 8)
//                throw new ArgumentException("Password must be at least 8 characters.");

//            if (password.Length > 100)
//                throw new ArgumentException("Password is too long.");

//            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
//                throw new ArgumentException(
//                    "Password must contain at least one letter and one number.");

//            if (password != (request.ConfirmPassword ?? ""))
//                throw new ArgumentException("Passwords do not match.");

//            byte[] hash;
//            byte[] salt;
//            PasswordHasher.CreateHash(password, out hash, out salt);

//            // The display name is the username.
//            int newID = userDAL.Register(username, username, hash, salt);

//            return new User
//            {
//                ID = newID,
//                Username = username,
//                FullName = username,
//                Role = "User"
//            };
//        }

//        // ===== SIGN IN =====
//        public User Login(string username, string password, out string token)
//        {
//            token = null;

//            if (string.IsNullOrWhiteSpace(username))
//                throw new ArgumentException("Username is required.");

//            if (string.IsNullOrEmpty(password))
//                throw new ArgumentException("Password is required.");

//            username = username.Trim();

//            if (username.Length > 100)
//                return null;

//            UserCredentials creds = userDAL.GetByUsername(username);

//            // Same answer for "no such user" and "wrong password".
//            if (creds == null || !creds.IsActive)
//                return null;

//            bool valid;

//            if (creds.PasswordSalt == null)
//            {
//                // Old account (plain SHA-256). Verify, then upgrade to salted PBKDF2.
//                valid = PasswordHasher.VerifyLegacy(password, creds.PasswordHash);

//                if (valid)
//                {
//                    try
//                    {
//                        byte[] newHash;
//                        byte[] newSalt;
//                        PasswordHasher.CreateHash(password, out newHash, out newSalt);
//                        userDAL.UpdatePassword(creds.ID, newHash, newSalt);
//                    }
//                    catch (Exception)
//                    {
//                        // Upgrade failure must not block the sign in.
//                    }
//                }
//            }
//            else
//            {
//                valid = PasswordHasher.Verify(password, creds.PasswordSalt, creds.PasswordHash);
//            }

//            if (!valid)
//                return null;

//            token = CreateToken();

//            Sessions[token] = new SessionInfo
//            {
//                Username = creds.Username,
//                Role = creds.Role,
//                ExpiresAtUtc = DateTime.UtcNow.Add(SessionLifetime)
//            };

//            return new User
//            {
//                ID = creds.ID,
//                Username = creds.Username,
//                FullName = creds.FullName,
//                Role = creds.Role
//            };
//        }

//        // ===== TOKEN HELPERS =====
//        public static string GetUserNameByToken(string token)
//        {
//            string username;
//            string role;

//            return TryGetSession(token, out username, out role) ? username : null;
//        }

//        public static bool TryGetSession(string token, out string username, out string role)
//        {
//            username = null;
//            role = null;

//            SessionInfo session;

//            if (!string.IsNullOrEmpty(token) && Sessions.TryGetValue(token, out session))
//            {
//                if (session.ExpiresAtUtc > DateTime.UtcNow)
//                {
//                    username = session.Username;
//                    role = session.Role;
//                    return true;
//                }

//                SessionInfo expired;
//                Sessions.TryRemove(token, out expired);
//            }

//            return false;
//        }

//        public static void Logout(string token)
//        {
//            if (string.IsNullOrEmpty(token))
//                return;

//            SessionInfo removed;
//            Sessions.TryRemove(token, out removed);
//        }

//        private static string CreateToken()
//        {
//            byte[] bytes = new byte[32];

//            using (var rng = RandomNumberGenerator.Create())
//            {
//                rng.GetBytes(bytes);
//            }

//            return BitConverter.ToString(bytes).Replace("-", "");
//        }
//    }
//}


using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TrainingShippingSystem.DAL;
using TrainingShippingSystem.Models;

namespace TrainingShippingSystem.BLL
{
    public class AuthBLL
    {
        private static readonly TimeSpan SessionLifetime = TimeSpan.FromHours(8);

        private static readonly Regex UsernameRegex =
            new Regex(@"^[A-Za-z0-9_.]{3,30}$", RegexOptions.Compiled);

        // Sessions live in the database (table UserSessions),
        // so they survive an application restart.
        private static readonly UserSessionDAL SessionDAL = new UserSessionDAL();

        private readonly UserDAL userDAL = new UserDAL();

        // =====================================================
        // SIGN UP
        // =====================================================
        public User Register(RegisterRequest request)
        {
            if (request == null)
                throw new ArgumentException("Registration data is required.");

            string username = (request.Username ?? "").Trim();
            string password = request.Password ?? "";

            if (username.Length == 0)
                throw new ArgumentException("Username is required.");

            if (!UsernameRegex.IsMatch(username))
                throw new ArgumentException(
                    "Username must be 3-30 characters: letters, numbers, dot or underscore.");

            if (password.Length == 0)
                throw new ArgumentException("Password is required.");

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");

            if (password.Length > 100)
                throw new ArgumentException("Password is too long.");

            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
                throw new ArgumentException(
                    "Password must contain at least one letter and one number.");

            if (password != (request.ConfirmPassword ?? ""))
                throw new ArgumentException("Passwords do not match.");

            byte[] hash;
            byte[] salt;
            PasswordHasher.CreateHash(password, out hash, out salt);

            // The display name is the username.
            int newID = userDAL.Register(username, username, hash, salt);

            return new User
            {
                ID = newID,
                Username = username,
                FullName = username,
                Role = "User"
            };
        }

        // =====================================================
        // SIGN IN
        // =====================================================
        public User Login(string username, string password, out string token)
        {
            token = null;

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required.");

            username = username.Trim();

            if (username.Length > 100)
                return null;

            UserCredentials creds = userDAL.GetByUsername(username);

            // Same answer for "no such user" and "wrong password".
            if (creds == null || !creds.IsActive)
                return null;

            bool valid;

            if (creds.PasswordSalt == null)
            {
                // Old account (plain SHA-256). Verify, then upgrade to salted PBKDF2.
                valid = PasswordHasher.VerifyLegacy(password, creds.PasswordHash);

                if (valid)
                {
                    try
                    {
                        byte[] newHash;
                        byte[] newSalt;
                        PasswordHasher.CreateHash(password, out newHash, out newSalt);
                        userDAL.UpdatePassword(creds.ID, newHash, newSalt);
                    }
                    catch (Exception)
                    {
                        // Upgrade failure must not block the sign in.
                    }
                }
            }
            else
            {
                valid = PasswordHasher.Verify(password, creds.PasswordSalt, creds.PasswordHash);
            }

            if (!valid)
                return null;

            string newToken = CreateToken();

            // Only the hash of the token is stored in the database.
            SessionDAL.Create(
                HashToken(newToken),
                creds.ID,
                DateTime.UtcNow.Add(SessionLifetime));

            token = newToken;

            return new User
            {
                ID = creds.ID,
                Username = creds.Username,
                FullName = creds.FullName,
                Role = creds.Role
            };
        }

        // =====================================================
        // TOKEN HELPERS
        // =====================================================
        public static bool TryGetSession(string token, out string username, out string role)
        {
            username = null;
            role = null;

            if (string.IsNullOrEmpty(token) || token.Length > 128)
                return false;

            try
            {
                UserSessionInfo session = SessionDAL.GetByTokenHash(HashToken(token));

                if (session == null)
                    return false;

                username = session.Username;
                role = session.Role;
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Session lookup failed: {0}", ex.Message);
                return false;
            }
        }

        public static string GetUserNameByToken(string token)
        {
            string username;
            string role;

            return TryGetSession(token, out username, out role) ? username : null;
        }

        public static void Logout(string token)
        {
            if (string.IsNullOrEmpty(token) || token.Length > 128)
                return;

            try
            {
                SessionDAL.Delete(HashToken(token));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Logout failed: {0}", ex.Message);
            }
        }

        private static byte[] HashToken(string token)
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            }
        }

        private static string CreateToken()
        {
            byte[] bytes = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}