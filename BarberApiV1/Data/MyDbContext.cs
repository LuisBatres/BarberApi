using Microsoft.EntityFrameworkCore;

namespace BarberApiV1.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
}