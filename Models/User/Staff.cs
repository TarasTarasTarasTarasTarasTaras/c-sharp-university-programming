namespace Staff_Project.Models.User
{
    using System;

    class Staff : User
    {
        public decimal? Salary { get; set; } 
        public DateTime? FirstDayInCompany { get; set; }

        /*public Staff(string firstName, string lastName, string email, string password)
            :base(firstName, lastName, email, password)
        {
            Salary = 0;
            Role = Role.Staff;
            FirstDayInCompany = DateTime.Now;
        }*/
    }
}
