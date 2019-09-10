using IdentityRepositoryPoC.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityRepositoryPoC.Data.Data.EntityFramework
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
