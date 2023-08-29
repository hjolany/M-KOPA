using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SMSMicroService.Entities.Models;

namespace SMSMicroService.Infrastructures
{
    public class SmsDbContext : DbContext
    {
        public SmsDbContext(DbContextOptions<SmsDbContext> options):base(options)
        {
        } 
        public DbSet<MessageModel> Messages { get; set; }

        public static SmsDbContext Create()
        {
            DbContextOptionsBuilder<SmsDbContext> optionsBuilder = new DbContextOptionsBuilder<SmsDbContext>();

            optionsBuilder.UseInMemoryDatabase("SmsDb");
            return new SmsDbContext(optionsBuilder.Options);
        }
    }
}
