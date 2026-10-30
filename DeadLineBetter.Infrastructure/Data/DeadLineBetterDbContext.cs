using Microsoft.EntityFrameworkCore;


namespace DeadLineBetter.Infrastructure.Data
{
    public sealed class DeadLineBetterDbContext(DbContextOptions<DeadLineBetterDbContext> options) : DbContext(options)
    {
    }
}
