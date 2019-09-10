using IdentityRepositoryPoC.Data.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityRepositoryPoC.Data.Models
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>
    {
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
                var applicationUser = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.NormalizedUserName == user.NormalizedUserName);
                return applicationUser.Id;
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
                return applicationUser.UserName;
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
                applicationUser.NormalizedUserName = normalizedName;
                _unitOfWork.UserRepository.Update(applicationUser);
                await _unitOfWork.CommitAsync();
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
                applicationUser.UserName = userName;
                _unitOfWork.UserRepository.Update(applicationUser);
                await _unitOfWork.CommitAsync();
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

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls
        private readonly IUnitOfWork _unitOfWork;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserStore()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

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
