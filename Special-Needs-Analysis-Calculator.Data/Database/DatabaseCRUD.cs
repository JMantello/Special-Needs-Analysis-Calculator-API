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
        public Task<UserDocument> FindUser(string email);
        public Task<bool> UpdateUser(UserModel userInfo);
        public Task<bool> DeleteUser(string email);
        public Task<bool> AddDependant(string guardianEmail, DependentModel dependant);

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
                context.Users.Add(new UserDocument(userInfo));
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<UserDocument> FindUser(string email)
        {
            return context.Users.Find(email);
        }

        public async Task<bool> UpdateUser(UserModel userInfo)
        {
            try
            {
                UserDocument userDocument = await FindUser(userInfo.ContactInfo.Email);
                userDocument.User = userInfo;
                context.Users.Update(userDocument);
                context.SaveChanges();
                return true;
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUser(string email)
        {
            try
            {
                UserDocument userDocument = await FindUser(email);
                userDocument.User.IsActive = false;
                return await UpdateUser(userDocument.User);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddDependant(string guardianEmail, DependentModel dependant)
        {
            UserDocument userDocument = await FindUser(guardianEmail);
            if (userDocument == null) return false;
            
            if(userDocument.User.Dependents == null)
                userDocument.User.Dependents = new List<DependentModel>();
            
            userDocument.User.Dependents.Add(dependant);

            return await UpdateUser(userDocument.User);
        }
    }
}
