﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public interface IDatabaseCrud
    {
        public Task<bool> CreateUser(UserModel userInfo);
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
                context.Users.Add(userInfo);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


    }
}
