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
                UserDocument userDocument = await FindUser(userInfo.Email);
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
    }
}
