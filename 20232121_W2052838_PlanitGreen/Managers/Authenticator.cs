using _20232121_W2052838_PlanitGreen.Data;
using _20232121_W2052838_PlanitGreen.Models;

namespace _20232121_W2052838_PlanitGreen.Managers
{
    public class Authenticator
    {
        private readonly ApplicationDbContext _context;

        public Authenticator(ApplicationDbContext context)
        {
            _context = context;
        }

        //Checks database User table to see if there are any identical usernames
        public bool IsUsernameTaken(string username)
        {
            return _context.User.Any(user => user.Username == username);
        }

        //Checks database User table whether a matching user exists and returs user/null 
        public User? AuthenticateUser(string username, string password)
        {
            return _context.User.FirstOrDefault(user => user.Username == username && user.Password == password);
        }
    }
}
