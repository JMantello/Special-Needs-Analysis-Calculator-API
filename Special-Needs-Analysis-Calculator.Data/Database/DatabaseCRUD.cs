﻿using Special_Needs_Analysis_Calculator.Data.Models.People;
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
        public Task<UserDocument> FindUser(string Email);
    }

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
                UserDocument userDocument = new UserDocument();
                userDocument.User = userInfo;
                userDocument.Email = userInfo.ContactInfo.Email;
                context.Users.Add(userDocument);
                
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<UserDocument> FindUser(string Email)
        {
            return context.Users.Find(Email);
        }

    }
}
