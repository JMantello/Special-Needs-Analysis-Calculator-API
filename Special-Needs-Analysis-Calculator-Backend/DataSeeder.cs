using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<SpecialNeedsAnalysisDbContext>();
            context.Database.EnsureCreated();
            AddUsers(context);
        }

        private static void AddUsers(SpecialNeedsAnalysisDbContext context)
        {
            UserDocument? user = context.Users.FirstOrDefault();
            if (user != null) return;               // exit seeder on user exists

            UserDocument newUser1 = new UserDocument();
            newUser1.User = new UserModel
            {
                FirstName = "Iris",
                LastName = "Rowe",
                Email = "irowe2@gmail.com"
            };

            UserDocument newUser2 = new UserDocument();
            newUser2.User = new UserModel
            {
                FirstName = "Torren",
                LastName = "Bower",
                Email = "tbower@gmail.com"
            };

            UserDocument newUser3 = new UserDocument();
            newUser3.User = new UserModel
            {
                FirstName = "Uly",
                LastName = "Roots",
                Email = "uroots55@gmail.com"
            };

            context.Users.Add(newUser1);
            context.Users.Add(newUser2);
            context.Users.Add(newUser3);

            context.SaveChanges();
        }
    }
}
