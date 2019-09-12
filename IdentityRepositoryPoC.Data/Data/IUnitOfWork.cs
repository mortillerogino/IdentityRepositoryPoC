using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityRepositoryPoC.Data.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IUserClaimRepository UserClaimRepository { get; }

        void Commit();

        Task<int> CommitAsync();
    }
}
