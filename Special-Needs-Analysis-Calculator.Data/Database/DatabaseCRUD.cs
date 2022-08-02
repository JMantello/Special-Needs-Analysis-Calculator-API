﻿using Microsoft.EntityFrameworkCore;
using Special_Needs_Analysis_Calculator.Data.Models;
using Special_Needs_Analysis_Calculator.Data.Models.InputModels;
using Special_Needs_Analysis_Calculator.Data.Models.Login;
using Special_Needs_Analysis_Calculator.Data.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Database
{
    public interface IDatabaseCRUD
    {
        public Task<bool> CreateUser(CreateUserModel createUserModel);
        public Task<UserDocument?> FindUserBySessionToken(string sessionToken);
        public Task<List<BeneficiaryModel>?> FindBeneficiariesBySessionToken(string sessionToken);
        public Task<bool> UpdateUser(UpdateUserModel updateUserModel);
        public Task<bool> DeleteUser(string sessionToken);
        public Task<bool> AddBeneficiary(AddBeneficiaryModel addBeneficiaryModel);
        public Task<string?> Login(UserLogin loginRequest);
        public Task<bool> Logout(SessionTokenModel session);
    }

    // Singleton
    public class DatabaseCRUD : IDatabaseCRUD
    {
        private readonly SpecialNeedsAnalysisDbContext context;

        /// <summary>
        /// Constructor that sets up the connection to the database for CRUD operations
        /// </summary>
        /// <param name="context">holds the connection to the database</param>
        public DatabaseCRUD(SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates a new user inside the database. This function will store user information in Users
        /// and the hashed password inside of UserLogin along with the salt.
        /// </summary>
        /// <param name="createUserModel">User information fed in by the client</param>
        /// <returns>true/false for success or failure of the method</returns>
        public async Task<bool> CreateUser(CreateUserModel createUserModel)
        {
            await context.Users.AddAsync(new UserDocument(createUserModel.UserModel));
            
            string salt = Guid.NewGuid().ToString();

            await context.UserLogin.AddAsync(new UserLogin(createUserModel.UserModel.Email, SHA256Hash.PasswordHash(createUserModel.Password, salt), salt));
            
            await context.SaveChangesAsync();
            
            return true;
        }

        /// <summary>
        /// Take's in a session token and uses that to query user
        /// information.
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <returns>all the user's information stored in the Users table</returns>
        public async Task<UserDocument?> FindUserBySessionToken(string sessionToken)
        {
            SessionTokenModel? session = await context.Sessions
                .Where(s => s.SessionToken == sessionToken).FirstOrDefaultAsync();
            
            if (session == null) return null;

            UserDocument? user = await context.Users.FindAsync(session.Email);
            return user;
        }

        public async Task<List<BeneficiaryModel>?> FindBeneficiariesBySessionToken(string sessionToken)
        {
            UserDocument? userDocument = await FindUserBySessionToken(sessionToken);
            if (userDocument == null) return null;
            UserModel? user = userDocument.User;
            return user.Beneficiaries;
        }


        /// <summary>
        /// Updates user information inside the User's table
        /// </summary>
        /// <param name="updateUserModel">Object that holds the session token and all the updated info about the user 
        /// replace old info</param>
        /// <returns>true/false success or failure</returns>
        public async Task<bool> UpdateUser(UpdateUserModel updateUserModel)
        {
            UserDocument? userDocument = await FindUserBySessionToken(updateUserModel.SessionToken);
            if (userDocument == null) return false;
            userDocument.User = updateUserModel.UserModel;
            context.Users.Update(userDocument); // I wonder if this step is necessary
            await context.SaveChangesAsync(); // For if SaveChangesAsync takes care of the update.
            return true;
        }

        /// <summary>
        /// "Deletes" the user from the database. In reality it chages a stored variable
        /// that lets the frontend know they have deleted their account. This was done to 
        /// ensure easy restoration of user information.
        /// </summary>
        /// <param name="sessionToken">token to specify which user needs to be deleted in the Users table</param>
        /// <returns>true/false success or failure</returns>
        public async Task<bool> DeleteUser(string sessionToken)
        {
            UserDocument? userDocument = await FindUserBySessionToken(sessionToken);
            if (userDocument == null) return false;
            userDocument.User.IsAccountActive = false;
            context.Users.Update(userDocument);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Adds a Benificiary to an exisiting user. This allows the user to 
        /// create as many dependents as necessary, or even designate themselves
        /// as a benificiary of their own estate.
        /// </summary>
        /// <param name="addBeneficiaryModel">Everything needed to add a benificiary in Users table</param>
        /// <returns>true/false success or failure</returns>
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

        /// <summary>
        /// Logs an existing user into the website. This is done through
        /// password authentication then upon success creates a session for
        /// that particular user to remain logged in.
        /// </summary>
        /// <param name="loginRequest">Email and Passsword</param>
        /// <returns>returns a string representing a sessionid or null on faiure</returns>
        public async Task<string?> Login(UserLogin loginRequest)
        {
            UserLogin? attemptedLoginCredential = await context.UserLogin.Where(ul => ul.Email == loginRequest.Email).FirstOrDefaultAsync();

            if (attemptedLoginCredential == null) return null;

            string reHashedPassword = SHA256Hash.PasswordHash(loginRequest.Password, attemptedLoginCredential.Salt);

            if(attemptedLoginCredential.Password != reHashedPassword) return null;

            // If user's already logged in
            SessionTokenModel? existingSession = await context.Sessions.Where(s => s.Email == loginRequest.Email).FirstOrDefaultAsync();
            if (existingSession.SessionToken != null) return existingSession.SessionToken;

            string sessionToken = Guid.NewGuid().ToString();

            await context.Sessions.AddAsync(
                new SessionTokenModel(loginRequest.Email, sessionToken));

            await context.SaveChangesAsync();

            return sessionToken;
        }

        /// <summary>
        /// Lougouts a users session which forces them to log back
        /// in. In order to see their information.
        /// </summary>
        /// <param name="session">object that holds information to delete user's session</param>
        /// <returns>true/false success or failure</returns>
        public async Task<bool> Logout(SessionTokenModel session)
        {
            var TokenModel = await context.Sessions.FindAsync(session.Email);

            if (TokenModel != null && TokenModel.Email == session.Email)
            {
                context.Sessions.Remove(TokenModel);
                context.SaveChanges();
                return true;
            }
            else return false;
        }
    }
}