using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Special_Needs_Analysis_Calculator.Data.Database;
using Special_Needs_Analysis_Calculator.Data.Models.Login;
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
            //AddUserLogins(context);
        }

        /*private static void AddUserLogins(SpecialNeedsAnalysisDbContext context)
        {
            UserLogin? userLogin = context.UserLogin.FirstOrDefault();
            if (userLogin != null) return;

            context.Add(new UserLogin { Email = "Iris@gmail.com", Password = SHA256Hash.PasswordHash("iris")});
            context.Add(new UserLogin { Email = "Torren@gmail.com", Password = SHA256Hash.PasswordHash("torren")});
            context.Add(new UserLogin { Email = "Roots@gmail.com", Password = SHA256Hash.PasswordHash("roots")});
            context.SaveChanges();
        }*/

        private static void AddUsers(SpecialNeedsAnalysisDbContext context)
        {
            UserDocument? user = context.Users.FirstOrDefault();
            if (user != null) return;

            context.Add(new UserDocument(new UserModel
            {
                FirstName = "Iris",
                LastName = "Rowe",
                Email = "Iris@gmail.com",
                PrimaryPhoneNumber = "298-639-9285",
                SecondaryPhoneNumber = "298-798-7578",

                Beneficiaries =
                new List<BeneficiaryModel> 
                {
                    new BeneficiaryModel
                    {
                        FirstName = "Cherry",
                        LastName = "Rowe",
                        StateOfResidence = "Missouri",
                        IsStudent = true,
                        ConditionStatus = new ConditionStatusModel(true, true, true, false),
                        Eligibility = new EligibilityModel(true, true, true),
                        Expenses = new ExpensesModel(1500)
                    }
                }

            }));

            context.Add(new UserDocument(new UserModel
            {
                FirstName = "Torren",
                LastName = "Bower",
                Email = "Torren@gmail.com",
                PrimaryPhoneNumber = "366-462-9431",
                SecondaryPhoneNumber = "366-823-9554",
            }));

            context.Add(new UserDocument(new UserModel
            {
                FirstName = "Tree",
                LastName = "Roots",
                Email = "Roots@gmail.com",
                PrimaryPhoneNumber = "465-823-9554"
            }));

            context.SaveChanges();
        }
    }
}