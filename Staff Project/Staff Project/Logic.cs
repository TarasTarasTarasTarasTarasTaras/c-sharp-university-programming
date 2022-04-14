namespace Staff_Project
{
    using System;
    using Models;
    using Models.User;
    using Models.Menu;
    using System.Collections.Generic;
    using System.Reflection;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;

    public class Logic
    {
        public User User { get; private set; }
        public IMenu Menu { get; private set; }
        public List<User> AllUsers { get; private set; }
        public List<Respond> Responds { get; private set; }

        public Logic()
        {
            Menu = new BaseMenu();
            ReadAllUsers();
            ReadAllResponds();
        }

        public Tuple<string, int> PrintMenu()
        {
            Tuple<string, int> action = Menu.PrintMenu();
            return action;
        }

        public void Login()
        {
            Dictionary<string, string> data = Helpers.EnterDataAtLogin(AllUsers);
            User = AllUsers.First(u => u.Email == data["email"] && u.Password == data["password"]);

            if (User.Role == Role.Staff)
                Menu = new StaffMenu();
            else if (User.Role == Role.Admin)
                Menu = new AdminMenu();
        }

        public void Registration()
        {
            Dictionary<string, string> data = Helpers.EnterDataAtRegistration(AllUsers);

            User = new Staff();

            User.FirstName = data["firstName"];
            User.LastName = data["lastName"];
            User.Email = data["email"];
            User.Password = data["password"];


            AllUsers.Add(User);
            WriteUsersToFile();
        }

        public void LogOut()
        {
            User = null;
            Menu = new BaseMenu();
        }

        public void CreateNewPayment()
        {
            if (User.Role != Role.Admin && !CheckIsStaffUser()) return;
            Console.WriteLine("\n------- Create a new payment -------");

            Payment payment = new Payment();
            SetPropertiesForPayment(payment);

            payment.PayerEmail = User.Email;
            payment.RequestDate = DateTime.Now;
            payment.StatusOfPayment = StatusOfPayment.Draft;

            if (User.Payments is null) User.Payments = new List<Payment>();
            User.Payments.Add(payment);
            WriteUsersToFile();
        }

        public void EditPaymentWithDraftStatus()
        {
            if (User.Role != Role.Admin && !CheckIsStaffUser()) return;
            Console.WriteLine("\n------- Edit payment with draft status -------");

            int paymentNumber = SelectIndexPaymentByStatus(User.Payments, StatusOfPayment.Draft);

            if (paymentNumber == 0) return; // if cancel

            SetPropertiesForPayment(User.Payments[paymentNumber - 1]);
            User.Payments[paymentNumber - 1].RequestDate = DateTime.Now;
            WriteUsersToFile();
        }

        public void DeletePaymentWithDraftStatus()
        {
            if (User.Role != Role.Admin && !CheckIsStaffUser()) return;
            Console.WriteLine("\n------- Delete payment with draft status -------");

            int paymentNumber = SelectIndexPaymentByStatus(User.Payments, StatusOfPayment.Draft);

            if (paymentNumber == 0) return; // if cancel
            User.Payments.RemoveAt(paymentNumber - 1);
            WriteUsersToFile();
        }

        public void PrintAllUserPayments()
        {
            PrintAllUserPayments(User.Payments);
        }

        public void PrintAllUserPaymentsByFilter()
        {
            int status = -1, numberOfPaymentStatuses = Enum.GetValues(typeof(StatusOfPayment)).Length;
            do
            {
                Console.WriteLine("Select status of payment");
                foreach (StatusOfPayment st in Enum.GetValues(typeof(StatusOfPayment)))
                    Console.WriteLine($"  {(int)st}. {st}");
                try
                {
                    status = int.Parse(Console.ReadLine());
                }
                catch { continue; }
            } while (status < 0 || status >= numberOfPaymentStatuses);

            PrintAllUserPaymentsByFilter(User.Payments, (StatusOfPayment)status);
        }

        public void PrintAllApprovedPayments()
        {
            foreach(User user in AllUsers)
            {
                Console.WriteLine($"\n----- Approved payments of user: {user.FirstName} {user.LastName} -----\n");
                PrintAllUserPaymentsByFilter(user.Payments, StatusOfPayment.Approved);
                Console.WriteLine("----------------------------------------------------");
            }
        }

        public void EditAndSendForModeration()
        {
            int index = SelectIndexPaymentByStatus(User.Payments, StatusOfPayment.Rejected);
            if (index == 0) return;

            Respond respond = Helpers.GetRespondFromRejectedPayments(Responds, User.Payments[index - 1]);
            Responds.Remove(respond);

            SetPropertiesForPayment(User.Payments[index - 1]);
            User.Payments[index - 1].StatusOfPayment = StatusOfPayment.Draft;

            WriteUsersToFile();
            WriteRespondsToFile();
        }

        public void AdminPanelPrintAllPayments()
        {
            List<Payment> payments = AdminPanelGetAllPayments();
            Console.WriteLine($"\n----- [Admin Panel] All payments -----");
            PrintAllUserPayments(payments);
        }

        public void AdminPanelSetStatusOfPayment()
        {
            if (!CheckIsAdminUser()) return;
            List<Payment> allPayments = AdminPanelGetAllPaymentsByFilter(StatusOfPayment.Draft);

            Console.WriteLine("\n------- [Admin Panel] Set status of payments with draft status -------");
            int paymentNumber = SelectIndexPaymentByStatus(allPayments, StatusOfPayment.Draft);

            if (paymentNumber == 0) return; // if cancel
            AdminPanelSetStatusOfPayment(allPayments[paymentNumber - 1]);
            WriteUsersToFile();
        }

        public void AdminPanelPrintAllRejectedPayments()
        {
            if (!CheckIsAdminUser()) return;
            Console.WriteLine("\n------- [Admin Panel] All payments with rejected status -------");
            List<Payment> rejectedPayments = AdminPanelGetAllPaymentsByFilter(StatusOfPayment.Rejected);
            PrintAllUserPayments(rejectedPayments);
        }

        private void PrintAllUserPayments(List<Payment> payments)
        {
            for (int i = 1; i <= payments.Count; i++)
            {
                Console.WriteLine($"----- Payment Number {i} -----");
                Console.Write(payments[i - 1]);
                if (payments[i - 1].StatusOfPayment == StatusOfPayment.Rejected)
                {
                    string message = Helpers.GetRespondFromRejectedPayments(Responds, payments[i - 1]).Message;
                    Console.WriteLine($" --> Message: {message}");
                }
                Console.WriteLine("------------------------------\n");
            }
        }

        private void PrintAllUserPaymentsByFilter(List<Payment> payments, StatusOfPayment filter)
        {
            for (int i = 1; i <= payments.Count; i++)
            {
                if (payments[i - 1].StatusOfPayment == filter)
                {
                    Console.WriteLine($"----- Payment Number {i} -----");
                    Console.Write(payments[i - 1]);
                    if(filter == StatusOfPayment.Rejected)
                    {
                        string message = Helpers.GetRespondFromRejectedPayments(Responds, payments[i - 1]).Message;
                        Console.WriteLine($" --> Message: {message}");
                    }
                    Console.WriteLine("------------------------------\n");
                }
            }
        }

        private List<Payment> AdminPanelGetAllPayments()
        {
            List<Payment> payments = new List<Payment>();

            foreach (User user in AllUsers)
                foreach (Payment payment in user.Payments)
                    payments.Add(payment);

            return payments;
        }

        private List<Payment> AdminPanelGetAllPaymentsByFilter(StatusOfPayment filter)
        {
            List<Payment> payments = new List<Payment>();

            foreach (User user in AllUsers)
                foreach(Payment payment in user.Payments)
                    if(payment.StatusOfPayment == filter)
                        payments.Add(payment);

            return payments;
        }

        private int SelectIndexPaymentByStatus(List<Payment> payments, StatusOfPayment status)
        {
            int paymentNumber = -1, numberOfPayments = payments.Count;
            do
            {
                PrintAllUserPaymentsByFilter(payments, status);
                Console.WriteLine("  >> Enter 0 if you want to cancel editing");
                try
                {
                    Console.Write(" Payment No: ");
                    paymentNumber = int.Parse(Console.ReadLine());
                    if (paymentNumber == 0) return 0;
                    if (payments[paymentNumber - 1].StatusOfPayment != status)
                        paymentNumber = -1;
                }
                catch { continue; }
            } while (paymentNumber < 0 || paymentNumber - 1 >= numberOfPayments);

            return paymentNumber;
        }

        private bool CheckIsStaffUser()
        {
            if (User == null || User.Role != Role.Staff)
            {
                Console.WriteLine("This method is only available to authorized staff users");
                return false;
            }
            return true;
        }

        private bool CheckIsAdminUser()
        {
            if (User == null || User.Role != Role.Admin)
            {
                Console.WriteLine("This method is only available to authorized admin users");
                return false;
            }
            return true;
        }

        private Dictionary<string, string> EnterDataForPayment()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            foreach (PropertyInfo property in typeof(Payment).GetProperties())
            {
                if (_notEnteredForPayment.FirstOrDefault(u => u==property.Name) is null)
                {
                    Console.Write($"Enter {property.Name}: ");
                    data.Add(property.Name, Console.ReadLine());
                }
            }

            return data;
        }

        private void SetPropertiesForPayment(Payment payment)
        {
            while (true)
            {
                string errors = String.Empty;
                Dictionary<string, string> data = EnterDataForPayment();
                foreach (PropertyInfo property in typeof(Payment).GetProperties())
                {
                    try
                    {
                        if (_notEnteredForPayment.FirstOrDefault(u => u == property.Name) is null)
                            Helpers.SetPropertyPayment(payment, property, data[property.Name]);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                            errors += $">>> Error! {ex.InnerException.Message}\n";
                        else
                            errors += $">>> Error! {ex.Message}\n";
                    }
                }
                if (errors == String.Empty) break;

                Console.WriteLine(errors);
            }
        }

        private void AdminPanelSetStatusOfPayment(Payment payment)
        {
            int status = -1, numberOfPaymentStatuses = Enum.GetValues(typeof(StatusOfPayment)).Length;
            do
            {
                Console.WriteLine("Select new status of payment");
                foreach (StatusOfPayment st in Enum.GetValues(typeof(StatusOfPayment)))
                    Console.WriteLine($"  {(int)st}. {st}");
                try
                {
                    status = int.Parse(Console.ReadLine());
                } catch { continue; }
            } while (status < 0 || status > numberOfPaymentStatuses);

            payment.StatusOfPayment = (StatusOfPayment)status;
            if(payment.StatusOfPayment == StatusOfPayment.Rejected)
            {
                Console.Write(" Enter your message: ");
                string message = Console.ReadLine();

                Respond respond = new Respond
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    PayerEmail = payment.PayerEmail,
                    RequestDate = payment.RequestDate,
                    DueToDate = payment.DueToDate,
                    TransactionId = payment.TransactionId,
                    StatusOfPayment = payment.StatusOfPayment,
                    Message = message
                };

                Responds.Add(respond);
                WriteRespondsToFile();
            }
        }

        private void ReadAllUsers()
        {
            if (!File.Exists(_pathUsersFile))
                File.Create(_pathUsersFile).Close();

            try
            {
                string jsonUsers = File.ReadAllText(_pathUsersFile);

                JsonConverter[] converters = { new UserConverter() };
                AllUsers = JsonConvert.DeserializeObject<List<User>>(jsonUsers, new JsonSerializerSettings() { Converters = converters });
            }
            catch { AllUsers = new List<User>(); }
        }

        private void WriteUsersToFile()
        {
            string jsonUsers = JsonConvert.SerializeObject(AllUsers, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText(_pathUsersFile, jsonUsers);
        }

        private void ReadAllResponds()
        {
            if (!File.Exists(_pathRespondsFile))
                File.Create(_pathRespondsFile).Close();

            try
            {
                string jsonResponds = File.ReadAllText(_pathRespondsFile);

                Responds = JsonConvert.DeserializeObject<List<Respond>>(jsonResponds);
            }
            catch { Responds = new List<Respond>(); }
        }

        private void WriteRespondsToFile()
        {
            string jsonResponds = JsonConvert.SerializeObject(Responds, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            File.WriteAllText(_pathRespondsFile, jsonResponds);
        }

        private const string _pathUsersFile = "../../../Data/users.json";
        private const string _pathRespondsFile = "../../../Data/responds.json";
        private readonly string[] _notEnteredForPayment = new string[] { "PayerEmail", "StatusOfPayment", "RequestDate" };
    }
}
