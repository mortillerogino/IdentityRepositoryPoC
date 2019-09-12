using IdentityRepositoryPoC.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityRepositoryPoC.Data.Data.EntityFramework
{
    public class UserClaimRepository : Repository<ApplicationUserClaim>, IUserClaimRepository
    {
        public UserClaimRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
