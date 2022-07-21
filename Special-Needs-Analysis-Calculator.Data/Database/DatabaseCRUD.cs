﻿using Microsoft.EntityFrameworkCore;
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
        public Task<UserDocument?> FindUserBySessionToken(string sessionToken);
        public Task<bool> UpdateUser(UpdateUserModel updateUserModel);
        public Task<bool> DeleteUser(string sessionToken);
        public Task<bool> AddBeneficiary(AddBeneficiaryModel addBeneficiaryModel);
        public Task<string?> Login(UserLogin loginRequest);

    }

    // Singleton
    public class DatabaseCrud : IDatabaseCrud
    {
        private readonly SpecialNeedsAnalysisDbContext context;

        /// <summary>
        /// Constructor that sets up the connection to the database for CRUD operations
        /// </summary>
        /// <param name="context">holds the connection to the database</param>
        public DatabaseCrud(SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates a new user inside the database. This function will store user information in Users
        /// and the hashed password inside of UserLogin along with the salt.
        /// </summary>
        /// <param name="userInfo">User information fed in by the client</param>
        /// <param name="password">Password associated with the user's email (PK)</param>
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
        /// Queries the UserLogin table to obtain Login information 
        /// </summary>
        /// <param name="email">PK used to associate login information with their login info</param>
        /// <returns>a UserLogin object that holds all DB table info</returns>

        //public async Task<UserLogin?> FindUserLogin(string email)
        //{
        //    UserLogin? userLogin = await context.UserLogin.FindAsync(email);
        //    return userLogin;
        //}

        /// <summary>
        /// Queries the sessions table to obtain loggin sessions. This can be
        /// used to verify that a user has been successfully logged in.
        /// </summary>
        /// <param name="email">PK used to associate login information with their session info</param>
        /// <returns>a SessionTokenModel object that holds all the session table info</returns>

        //public async Task<SessionTokenModel?> FindUserSessions(string email)
        //{
        //    SessionTokenModel? userSessions = await context.Sessions.FindAsync(email);
        //    return userSessions;
        //}

        /// <summary>
        /// Queries the Users table to obtain information stored about the user. This
        /// information will be used in calculations further down the line
        /// </summary>
        /// <param name="email">PK used to associate the User with their own information</param>
        /// <returns>a UserDocument object that holds all the User's info</returns>

        //public async Task<UserDocument?> FindUser(string email)
        //{
        //    UserDocument? user = await context.Users.FindAsync(email);
        //    return user;
        //}

        public async Task<UserDocument?> FindUserBySessionToken(string sessionToken)
        {
            SessionTokenModel? session = await context.Sessions
                .Where(s => s.SessionToken == sessionToken).FirstOrDefaultAsync();
            
            if (session == null) return null;

            UserDocument? user = await context.Users.FindAsync(session.Email);
            return user;
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
        /// <param name="sessionToken">PK to specify which user needs to be deleted in the Users table</param>
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

            string sessionToken = Guid.NewGuid().ToString();

            await context.Sessions.AddAsync(
                new SessionTokenModel(loginRequest.Email, sessionToken));

            await context.SaveChangesAsync();

            return sessionToken;
        }
    }
}