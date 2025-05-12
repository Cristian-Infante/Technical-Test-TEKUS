using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
{
    public User? GetByUserName(string userName) =>
        Context.Users.AsNoTracking()
            .SingleOrDefault(u => u.Username == userName);
}