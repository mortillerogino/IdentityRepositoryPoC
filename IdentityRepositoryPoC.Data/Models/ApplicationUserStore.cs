using IdentityRepositoryPoC.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityRepositoryPoC.Data.Models
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>
    {
        private bool _disposedValue = false;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _unitOfWork.UserRepository.Insert(user);
                await _unitOfWork.CommitAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                var error = GetErrors(ex);
                return IdentityResult.Failed(error);
            }
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _unitOfWork.UserRepository.Delete(user.Id);
                await _unitOfWork.CommitAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                var error = GetErrors(ex);
                return IdentityResult.Failed(error);
            }
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetById(userId);
                return applicationUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.NormalizedUserName == normalizedUserName);
                return applicationUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetById(user.Id);
                return applicationUser.NormalizedUserName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetById(user.Id);
                if (applicationUser != null)
                {
                    return applicationUser.Id;
                }
                else
                {
                    return user.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    return applicationUser.UserName;
                }
                else
                {
                    return user.UserName;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    applicationUser.NormalizedUserName = normalizedName;
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    user.NormalizedUserName = normalizedName;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                
                if (applicationUser != null)
                {
                    applicationUser.UserName = userName;
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    user.UserName = userName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CommitAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                var error = GetErrors(ex);
                return IdentityResult.Failed(error);
            }
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    applicationUser.PasswordHash = passwordHash;
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    user.PasswordHash = passwordHash;
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    return applicationUser.PasswordHash;
                }
                else
                {
                    return user.PasswordHash;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    return !string.IsNullOrEmpty(applicationUser.PasswordHash);
                }
                else
                {
                    return !string.IsNullOrEmpty(user.PasswordHash);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    applicationUser.Email = email;
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    user.Email = email;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    return applicationUser.Email;
                }
                else
                {
                    return user.Email;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    return applicationUser.EmailConfirmed;
                }
                else
                {
                    return user.EmailConfirmed;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    applicationUser.EmailConfirmed = confirmed;
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    user.EmailConfirmed = confirmed;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.NormalizedEmail == normalizedEmail);
                return applicationUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    return applicationUser.NormalizedEmail;
                }
                else
                {
                    return user.NormalizedEmail;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    applicationUser.NormalizedEmail = normalizedEmail;
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    user.NormalizedEmail = normalizedEmail;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    applicationUser.SecurityStamp = stamp;
                    _unitOfWork.UserRepository.Update(applicationUser);
                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    user.SecurityStamp = stamp;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == user.Id);
                if (applicationUser != null)
                {
                    return applicationUser.SecurityStamp;
                }
                else
                {
                    return user.SecurityStamp;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                _disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }

        private IdentityError GetErrors(Exception ex)
        {
            var error = new IdentityError()
            {
                Code = ex.GetType().Name,
                Description = ex.Message
            };

            return error;
        }

        
    }
}
