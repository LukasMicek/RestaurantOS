using Microsoft.EntityFrameworkCore;

namespace RestaurantOS.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}