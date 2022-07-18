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

            UserDocument newUser1 = new UserDocument();
            newUser1.User = new UserModel
            {
                FirstName = "Iris",
                LastName = "Rowe",
                Address = "108 Blane Street",
                StateOfResidence = "Missouri",
                ConditionStatus = new ConditionStatusModel(true, true, true, true),
                Eligibility = new EligibilityModel(true, true, true),
                Expenses = new ExpensesModel(1500),
                ContactInfo = new ContactInfoModel("Iris@gmail.com", "298-639-9285", "298-798-7578")
            };
            newUser1.Email = newUser1.User.ContactInfo.Email;

            UserDocument newUser2 = new UserDocument();
            newUser2.User = new UserModel
            {
                FirstName = "Torren",
                LastName = "Bower",
                Address = "420 Tax Street",
                StateOfResidence = "Illinois",
                ConditionStatus = new ConditionStatusModel(false, false, false, false),
                Eligibility = new EligibilityModel(false, false, false),
                Expenses = new ExpensesModel(2000),
                ContactInfo = new ContactInfoModel("Torren@gmail.com", "366-462-9431", "366-823-9554")
            };
            newUser2.Email = newUser2.User.ContactInfo.Email;

            UserDocument newUser3 = new UserDocument();
            newUser3.User = new UserModel
            {
                FirstName = "Tree",
                LastName = "Roots",
                Address = "123 Stop Light Street",
                StateOfResidence = "Virginia",
                ConditionStatus = new ConditionStatusModel(false, false, true, true),
                Eligibility = new EligibilityModel(false, false, true),
                Expenses = new ExpensesModel(2500),
                ContactInfo = new ContactInfoModel("Trees@gmail.com", "465-823-9554")
            };
            newUser3.Email = newUser3.User.ContactInfo.Email;

            context.Users.Add(newUser1);
            context.Users.Add(newUser2);
            context.Users.Add(newUser3);

            context.SaveChanges();
        }

    }
}