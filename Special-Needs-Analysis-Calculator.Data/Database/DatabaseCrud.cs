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
        public async Task<bool> CreateUser(UserModel userInfo, string password)
        {
            var salt = Guid.NewGuid().ToString();

            await context.Users.AddAsync(new UserDocument(userInfo));
            await context.UserLogin.AddAsync(new UserLogin(userInfo.Email, SHA256Hash.PasswordHash(password, salt), salt));
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Queries the UserLogin table to obtain Login information 
        /// </summary>
        /// <param name="email">PK used to associate login information with their login info</param>
        /// <returns>a UserLogin object that holds all DB table info</returns>
        public async Task<UserLogin?> FindUserLogin(string email)
        {
            UserLogin? userLogin = await context.UserLogin.FindAsync(email);
            return userLogin;
        }

        /// <summary>
        /// Queries the sessions table to obtain loggin sessions. This can be
        /// used to verify that a user has been successfully logged in.
        /// </summary>
        /// <param name="email">PK used to associate login information with their session info</param>
        /// <returns>a SessionTokenModel object that holds all the session table info</returns>
        public async Task<SessionTokenModel?> FindUserSessions(string email)
        {
            SessionTokenModel? userSessions = await context.Sessions.FindAsync(email);
            return userSessions;
        }

        /// <summary>
        /// Queries the Users table to obtain information stored about the user. This
        /// information will be used in calculations further down the line
        /// </summary>
        /// <param name="email">PK used to associate the User with their own information</param>
        /// <returns>a UserDocument object that holds all the User's info</returns>
        public async Task<UserDocument?> FindUser(string email)
        {
            UserDocument? user = await context.Users.FindAsync(email);
            return user;
        }

        /// <summary>
        /// Updates user information inside the User's table
        /// </summary>
        /// <param name="userInfo">Object that holds all the info about the user will 
        /// replace old info</param>
        /// <returns>true/fale success or failure</returns>
        public async Task<bool> UpdateUser(UserModel userInfo)
        {
            UserDocument? userDocument = await FindUser(userInfo.Email);
            if (userDocument == null) return false;
            userDocument.User = userInfo;
            context.Users.Update(userDocument);
            await context.SaveChangesAsync(); // Save update inside the DB
            return true;
        }

        /// <summary>
        /// "Deletes" the user from the database. In reality it chages a stored variable
        /// that lets the frontend know they have deleted their account. This was done to 
        /// ensure easy restoration of user information.
        /// </summary>
        /// <param name="email">PK to specify which user needs to be deleted in the Users table</param>
        /// <returns>true/false success or failure</returns>
        public async Task<bool> DeleteUser(string email)
        {
            UserDocument? userDocument = await FindUser(email);
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
        /// <param name="email">PK to specifiy which user needs a benificiary in Users table</param>
        /// <param name="dependant">object that holds information about the benificiary being added</param>
        /// <returns>true/false success or failure</returns>
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

        /// <summary>
        /// Logs an existing user into the website this is done through
        /// password authentication then upon success creating a session for
        /// that particular user to remain logged in.
        /// </summary>
        /// <param name="userLogin">object that contains the information necesary to login a user</param>
        /// <returns>returns a string representing a sessionid or null on faiure</returns>
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