using Special_Needs_Analysis_Calculator.Data.Models.Login;
using Special_Needs_Analysis_Calculator.Data.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Database
{
    public interface IDatabaseCrud
    {
        public Task<bool> CreateUser(UserModel userInfo);
        public Task<UserDocument?> FindUser(string email);
        public Task<bool> UpdateUser(UserModel userInfo);
        public Task<bool> DeleteUser(string email);
        public Task<bool> AddBeneficiary(string email, BeneficiaryModel dependant);
        public Task<string?> Login(string email, string password);

    }

    // Singleton
    public class DatabaseCrud : IDatabaseCrud
    {
        private readonly SpecialNeedsAnalysisDbContext context;

        public DatabaseCrud(SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateUser(UserModel userInfo)
        {
            try
            {
                await context.Users.AddAsync(new UserDocument(userInfo));
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<UserDocument?> FindUser(string email)
        {
            UserDocument? user = await context.Users.FindAsync(email);
            return user;
        }

        public async Task<bool> UpdateUser(UserModel userInfo)
        {
            UserDocument? userDocument = await FindUser(userInfo.Email);
            if (userDocument == null) return false;
            userDocument.User = userInfo;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(string email)
        {
            UserDocument? userDocument = await FindUser(email);
            if (userDocument == null) return false;
            userDocument.User.IsAccountActive = false;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddBeneficiary(string email, BeneficiaryModel dependant)
        {
            UserDocument? userDocument = await FindUser(email);
            if (userDocument == null) return false;

            if (userDocument.User.Beneficiaries == null)
                userDocument.User.Beneficiaries = new List<BeneficiaryModel>();

            userDocument.User.Beneficiaries.Add(dependant);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> Login(string email, string password)
        {
            UserLogin? validCredentials = context.UserLogin
                .Where(ul => ul.Email == email && ul.Password == password)
                .FirstOrDefault();

            if (validCredentials == null) return null;

            string sessionToken = Guid.NewGuid().ToString();

            await context.Sessions.AddAsync(new SessionTokenModel(email, sessionToken));
            await context.SaveChangesAsync();

            return sessionToken;
        }
    }
}
