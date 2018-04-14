namespace WooliesXFunctionApp.Service
{
    using WooliesXFunctionApp.Core;
    using WooliesXFunctionApp.Entity;

    public class UserService
    {
        /// <summary>
        /// Returns a User object with Name and Token.
        /// </summary>
        /// <returns>User object</returns>
        public User GetUser()
        {
            return new User { Name = AppConstants.Name, Token = AppConstants.Token };
        }
    }
}
