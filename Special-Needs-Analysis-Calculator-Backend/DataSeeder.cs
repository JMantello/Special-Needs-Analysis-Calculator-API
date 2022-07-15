using Special_Needs_Analysis_Calculator.Data.Database;
using Special_Needs_Analysis_Calculator.Data.Models.People;
using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
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
                Address = "108 Blane Street",
                StateOfResidence = "Missouri",
                Expenses = new ExpensesModel(1500)
            };

            UserDocument newUser2 = new UserDocument();
            newUser2.User = new UserModel
            {
                FirstName = "Torren",
                LastName = "Bower",
                Address = "420 Tax Street",
                StateOfResidence = "Illinois",
                Expenses = new ExpensesModel(2000)
            };

            UserDocument newUser3 = new UserDocument();
            newUser3.User = new UserModel
            {
                FirstName = "Uly",
                LastName = "Roots",
                Address = "123 Stop Light Street",
                StateOfResidence = "Virginia",
                Expenses = new ExpensesModel(2000)
            };

            context.Users.Add(newUser1);
            context.Users.Add(newUser2);
            context.Users.Add(newUser3);

            context.SaveChanges();
        }
    }
}
