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
        public Task<bool> CreateUser(UserModel userInfo, string password);
        public Task<UserDocument?> FindUser(string email);
        public Task<bool> UpdateUser(UserModel userInfo);
        public Task<bool> DeleteUser(string email);
        public Task<bool> AddBeneficiary(string email, BeneficiaryModel dependant);
        public Task<string?> Login(UserLogin userLogin);

    }

    // Singleton
    public class DatabaseCrud : IDatabaseCrud
    {
        private readonly SpecialNeedsAnalysisDbContext context;

        public DatabaseCrud(SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateUser(UserModel userInfo, string password)
        {
            var salt = Guid.NewGuid().ToString();

            await context.Users.AddAsync(new UserDocument(userInfo));
            await context.UserLogin.AddAsync(new UserLogin(userInfo.Email, SHA256Hash.PasswordHash(password, salt), salt));
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<UserLogin?> FindUserLogin(string email)
        {
            UserLogin? userLogin = await context.UserLogin.FindAsync(email);
            return userLogin;
        }

        public async Task<SessionTokenModel?> FindUserSessions(string email)
        {
            SessionTokenModel? userSessions = await context.Sessions.FindAsync(email);
            return userSessions;
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
            context.Users.Update(userDocument); // I wonder if this step is necessary
            await context.SaveChangesAsync(); // For if SaveChangesAsync takes care of the update.
            return true;
        }

        public async Task<bool> DeleteUser(string email)
        {
            UserDocument? userDocument = await FindUser(email);
            if (userDocument == null) return false;
            userDocument.User.IsAccountActive = false;
            context.Users.Update(userDocument);
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
            context.Users.Update(userDocument);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> Login(UserLogin userLogin)
        {
            // get salt from the database
            var userDocument = await FindUser(userLogin.Email);
            var userLoginInfo = await FindUserLogin(userLogin.Email);
            var UserHash = SHA256Hash.PasswordHash(userLogin.Password, userLoginInfo.Salt);

            UserLogin? validCredentials = context.UserLogin
                .Where(ul => ul.Email == userLogin.Email && ul.Password == UserHash)
                .FirstOrDefault();

            if (validCredentials == null) return null;

            string sessionToken = Guid.NewGuid().ToString();

            await context.Sessions.AddAsync(new SessionTokenModel(userLogin.Email, sessionToken));
            await context.SaveChangesAsync();

            return sessionToken;
        }
    }
}
