using System.ComponentModel.DataAnnotations;

namespace ExpenseApp.Models
{
    public class UserMaster
    {

        public UserMaster() { }

        private String username;
        private String password;
        private String mobilenumber;

        [MaxLength(10)]
        public string Username { get => username; set => username = value; }
        [MaxLength(14)]
        public string Password { get => password; set => password = value; }

        [MaxLength(10)]
        public string Mobilenumber { get => mobilenumber; set => mobilenumber = value; }
    }
}
