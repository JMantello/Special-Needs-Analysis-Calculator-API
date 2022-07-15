using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public class UserDB : DbContext
    {
        private SpecialNeedsAnalysisDbContext context;

        public UserDB (SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddUser(UserModel userInfo)
        {
            UserDocument userDocument = new UserDocument();
            userDocument.User = userInfo;
            context.Users.Add(userDocument);
           
            context.SaveChanges();
            return true;
        }
        // jmant@notmyemail.com

        public async Task<UserDocument> FindUser(int Id)
        {
            var user = context.Users.Find(Id);

            return user;
        }
    }
    
}
