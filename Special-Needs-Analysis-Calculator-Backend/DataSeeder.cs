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
            UserModel? user = context.Users.FirstOrDefault();
            if (user != null) return;

            context.Users.Add(new UserModel
            {
                FirstName = "Iris",
                LastName = "Rowe",
                Email = "irowe2@gmail.com"
            });

            context.Users.Add(new UserModel
            {
                FirstName = "Torren",
                LastName = "Bower",
                Email = "tbower@gmail.com"
            });

            context.Users.Add(new UserModel
            {
                FirstName = "Uly",
                LastName = "Roots",
                Email = "uroots55@gmail.com"
            });

            context.SaveChanges();
        }
    }
}
