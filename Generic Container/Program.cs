namespace Generic_Container
{
    using System;

    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainMenu();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    Console.WriteLine($">>> Error! {ex.InnerException.Message}\n");
                else
                    Console.WriteLine($">>> Error! {ex.Message}\n");
            }
        }

        static private void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("What class do you want to work with?");
                Console.WriteLine("  1. Payment");
                Console.WriteLine("  2. Jewelry");
                Console.WriteLine("  3. Receipt");
                Console.WriteLine("  <. Exit");
                string choice = Console.ReadLine();

                if (choice == "<")
                    return;
                else if (choice == "1")
                {
                    Menu<PaymentModel> menu = new Menu<PaymentModel>();
                    Container<PaymentModel> container = new Container<PaymentModel>("payments.txt");
                    while (true)
                    {
                        int action = PrintMenu();
                        if (action == 0) break; // if user choose to exit
                        try
                        {
                            menu.dictionaryActions[action](container);
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                                Console.WriteLine($">>> Error! {ex.InnerException.Message}");
                            else
                                Console.WriteLine($">>> Error! {ex.Message}");
                        }
                    }
                }
                else if (choice == "2")
                {
                    Menu<JewelryModel> menu = new Menu<JewelryModel>();
                    Container<JewelryModel> container = new Container<JewelryModel>("jewelry.txt");
                    while (true)
                    {
                        int action = PrintMenu();
                        if (action == 0) break; // if user choose to exit
                        try
                        {
                            menu.dictionaryActions[action](container);
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                                Console.WriteLine($">>> Error! {ex.InnerException.Message}");
                            else
                                Console.WriteLine($">>> Error! {ex.Message}");
                        }
                    }
                }
                else if (choice == "3")
                {
                    Menu<ReceiptModel> menu = new Menu<ReceiptModel>();
                    Container<ReceiptModel> container = new Container<ReceiptModel>("receipt.txt");
                    while (true)
                    {
                        int action = PrintMenu();
                        if (action == 0) break; // if user choose to exit
                        try
                        {
                            menu.dictionaryActions[action](container);
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                                Console.WriteLine($">>> Error! {ex.InnerException.Message}");
                            else
                                Console.WriteLine($">>> Error! {ex.Message}");
                        }
                    }
                }
                else
                    continue;
            }
        }

        static private int PrintMenu()
        {
            while (true)
            {
                string strMenu = "\n ========== Select an operation ==========\n"
                               + "   1. Print container\n"
                               + "   2. Add object to container by keyboard\n"
                               + "   3. Edit attribute of object by ID\n"
                               + "   4. Delete object from container by ID\n"
                               + "   5. Search objects by attribute\n"
                               + "   6. Sort container by attribute\n"
                               + "   0. Exit\n"
                               + " ==========================================\n";
                Console.Write(strMenu);
                int action;
                try
                {
                    action = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine(">>> Error! You must enter integer number");
                    continue;
                }
                if (action >= 0 || action <= 6) return action;
            }
        }
    }
}