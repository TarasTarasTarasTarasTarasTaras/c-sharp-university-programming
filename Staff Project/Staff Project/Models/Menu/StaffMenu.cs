namespace Staff_Project.Models.Menu
{
    using System;

    internal class StaffMenu : IMenu
    {
        public Tuple<string, int> PrintMenu()
        {
            int choice = -1;
            while(choice < 0 || choice > 8)
            {
                Console.WriteLine("\n----------------- Online Bank -----------------\n" +
                                  "You are authorized in the system. Welcome!\n\n" +
                                  "1. Make a new payment\n" +
                                  "2. Edit a payment with draft status\n" +
                                  "3. Delete a payment with draft status\n" +
                                  "4. View all my payments\n" +
                                  "5. View all my payments by filter\n" +
                                  "6. View all other users' approved payments\n" +
                                  "7. Edit and send rejected payment for moderation\n" +
                                  "8. Log out\n" +
                                  "0. Get out of online banking\n" +
                                  "------------------------------------------------\n");
                try
                {
                    choice = int.Parse(Console.ReadLine());
                } catch { continue; }
            }

            return new Tuple<string, int>("StaffMenu", choice);
        }
    }
}
