using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Special_Needs_Analysis_Calculator.Data.Database;
using Special_Needs_Analysis_Calculator.Data.Models.People;
using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Database
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
            if (user != null) return;

            context.Add(new UserDocument(new UserModel
            {
                FirstName = "Iris",
                LastName = "Rowe",
                Address = "108 Blane Street",
                StateOfResidence = "Missouri",
                Email = "Iris@gmail.com",
                PrimaryPhoneNumber = "298-639-9285",
                SecondaryPhoneNumber = "298-798-7578",
                ConditionStatus = new ConditionStatusModel(true, true, true, true),
                Eligibility = new EligibilityModel(true, true, true),
                Expenses = new ExpensesModel(1500),
            }));

            context.Add(new UserDocument(new UserModel
            {
                FirstName = "Torren",
                LastName = "Bower",
                Address = "420 Tax Street",
                StateOfResidence = "Illinois",
                Email = "Torren@gmail.com",
                PrimaryPhoneNumber = "366-462-9431",
                SecondaryPhoneNumber = "366-823-9554",
                ConditionStatus = new ConditionStatusModel(false, false, false, false),
                Eligibility = new EligibilityModel(false, false, false),
                Expenses = new ExpensesModel(2000),
            }));

            context.Add(new UserDocument(new UserModel
            {
                FirstName = "Tree",
                LastName = "Roots",
                Address = "123 Stop Light Street",
                StateOfResidence = "Virginia",
                Email = "Trees@gmail.com",
                PrimaryPhoneNumber = "465-823-9554",
                ConditionStatus = new ConditionStatusModel(false, false, true, true),
                Eligibility = new EligibilityModel(false, false, true),
                Expenses = new ExpensesModel(2500),
            }));

            context.SaveChanges();
        }
    }
}