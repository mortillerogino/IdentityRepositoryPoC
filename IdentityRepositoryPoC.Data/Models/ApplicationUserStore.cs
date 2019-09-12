using IdentityRepositoryPoC.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityRepositoryPoC.Data.Models
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>, 
        IUserPasswordStore<ApplicationUser>, 
        IUserEmailStore<ApplicationUser>, 
        IUserSecurityStampStore<ApplicationUser>,
        IUserClaimStore<ApplicationUser>
        
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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
                ThrowIfDisposed();
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

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                ThrowIfDisposed();
                var userClaims = await _unitOfWork.UserClaimRepository.Get(c => c.UserId == user.Id);

                var claims = new ConcurrentBag<Claim>();

                if (userClaims.Count > 0)
                {
                    Parallel.ForEach(userClaims, currentUserClaim =>
                    {
                        claims.Add(currentUserClaim.ToClaim());
                    });
                }

                return claims.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                ThrowIfDisposed();

                var taskList = new List<Task>();

                foreach (Claim c in claims)
                {
                    var userClaim = new ApplicationUserClaim
                    {
                        UserId = user.Id,
                        ClaimType = c.Type,
                        ClaimValue = c.Value
                    };

                    taskList.Add(_unitOfWork.UserClaimRepository.Insert(userClaim));
                }

                await Task.WhenAll(taskList);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                ThrowIfDisposed();
                var matches = await _unitOfWork.UserClaimRepository.Get(uc => uc.UserId == user.Id && uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value);

                foreach (ApplicationUserClaim uc in matches)
                {
                    uc.ClaimValue = newClaim.Value;
                    uc.ClaimType = newClaim.Type;
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                ThrowIfDisposed();

                var taskList = new List<Task>();

                foreach (var claim in claims)
                {
                    var matchedClaims = await _unitOfWork.UserClaimRepository.Get(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
                    foreach (var c in matchedClaims)
                    {
                        taskList.Add(_unitOfWork.UserClaimRepository.Delete(c));
                    }
                }

                await Task.WhenAll(taskList);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var matchingUserClaims = await _unitOfWork.UserClaimRepository.Get(uc => uc.ClaimType == claim.Type && uc.ClaimValue == claim.Value);

            var taskList = new List<Task<ApplicationUser>>();

            foreach (ApplicationUserClaim uc in matchingUserClaims)
            {
                taskList.Add(_unitOfWork.UserRepository.GetById(uc.UserId));
            }

            var result = await Task.WhenAll(taskList.ToList());
            return result;
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
            GC.SuppressFinalize(this);
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

        private void ThrowIfDisposed()
        {
            if (_disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        
    }
}
