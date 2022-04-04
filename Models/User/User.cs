namespace Staff_Project.Models.User
{
    using System.Collections.Generic;

    public abstract class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
        public List<Payment> Payments { get; set; }

        /*protected User(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Payments = new List<Payment>();
        }*/
    }
}
