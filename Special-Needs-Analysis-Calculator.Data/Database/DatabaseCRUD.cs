using Microsoft.EntityFrameworkCore;
using Special_Needs_Analysis_Calculator.Data.Models.InputModels;
using Special_Needs_Analysis_Calculator.Data.Models.Login;
using Special_Needs_Analysis_Calculator.Data.Models.People;
using Special_Needs_Analysis_Calculator.Data.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Database
{
    public interface IDatabaseCrud
    {
        public Task<bool> CreateUser(CreateUserModel createUserModel);
        public Task<UserDocument?> FindUser(string email);
        public Task<bool> UpdateUser(UpdateUserModel updateUserModel);
        public Task<bool> DeleteUser(string sessionToken);
        public Task<bool> AddBeneficiary(AddBeneficiaryModel addBeneficiaryModel);
        public Task<string?> Login(UserLogin loginRequest);

    }

    // Singleton
    public class DatabaseCrud : IDatabaseCrud
    {
        private readonly SpecialNeedsAnalysisDbContext context;

        public DatabaseCrud(SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateUser(CreateUserModel createUserModel)
        {
            await context.Users.AddAsync(new UserDocument(createUserModel.UserModel));
            
            string salt = Guid.NewGuid().ToString();

            await context.UserLogin.AddAsync(new UserLogin(createUserModel.UserModel.Email, SHA256Hash.PasswordHash(createUserModel.Password, salt), salt));
            
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

        public async Task<UserDocument?> FindUserBySessionToken(string sessionToken)
        {
            SessionTokenModel? session = await context.Sessions
                .Where(s => s.SessionToken == sessionToken).FirstOrDefaultAsync();
            
            if (session == null) return null;

            UserDocument? user = await FindUser(session.Email);
            return user;
        }

        public async Task<bool> UpdateUser(UpdateUserModel updateUserModel)
        {
            UserDocument? userDocument = await FindUserBySessionToken(updateUserModel.SessionToken);
            if (userDocument == null) return false;
            userDocument.User = updateUserModel.UserModel;
            context.Users.Update(userDocument); // I wonder if this step is necessary
            await context.SaveChangesAsync(); // For if SaveChangesAsync takes care of the update.
            return true;
        }

        public async Task<bool> DeleteUser(string sessionToken)
        {
            UserDocument? userDocument = await FindUserBySessionToken(sessionToken);
            if (userDocument == null) return false;
            userDocument.User.IsAccountActive = false;
            context.Users.Update(userDocument);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddBeneficiary(AddBeneficiaryModel addBeneficiaryModel)
        {
            UserDocument? userDocument = await FindUserBySessionToken(addBeneficiaryModel.SessionToken);
            if (userDocument == null) return false;

            if (userDocument.User.Beneficiaries == null)
                userDocument.User.Beneficiaries = new List<BeneficiaryModel>();

            userDocument.User.Beneficiaries.Add(addBeneficiaryModel.BeneficiaryModel);
            
            context.Users.Update(userDocument);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> Login(UserLogin loginRequest)
        {
            UserLogin? attemptedLoginCredential = await context.UserLogin.Where(ul => ul.Email == loginRequest.Email).FirstOrDefaultAsync();

            if (attemptedLoginCredential == null) return null;

            string reHashedPassword = SHA256Hash.PasswordHash(loginRequest.Password, attemptedLoginCredential.Salt);

            if(attemptedLoginCredential.Password != reHashedPassword) return null;

            string sessionToken = Guid.NewGuid().ToString();

            await context.Sessions.AddAsync(
                new SessionTokenModel(loginRequest.Email, sessionToken));

            await context.SaveChangesAsync();

            return sessionToken;
        }
    }
}
