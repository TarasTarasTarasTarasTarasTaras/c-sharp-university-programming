namespace Staff_Project.Models.Menu
{
    using System;

    internal class BaseMenu : IMenu
    {
        public Tuple<string, int> PrintMenu()
        {
            int choice = -1;
            while (choice < 0 || choice > 2)
            {
                Console.WriteLine("\n----------------- Online Bank -----------------\n" +
                                  "You are not logged in. Please log in!\n\n" +
                                  "1. Log in\n" +
                                  "2. Sign up\n" +
                                  "0. Get out of online banking\n" +
                                  "------------------------------------------------\n");
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch { continue; }
            }

            return new Tuple<string, int>("BaseMenu", choice);
        }
    }
}
