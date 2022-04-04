namespace Staff_Project
{
    using System;
    using Models;
    using Models.User;
    using System.Collections.Generic;
    using static Validation.UserT;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Linq;

    static class Helpers
    {
        public static Dictionary<string, string> EnterDataAtLogin(List<User> users)
        {
            Console.WriteLine("----- Login -----");
            string email;
            do
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine();
            } while (EmailIsNotValid(email) || !EmailAlreadyExist(email, users, registration: false));

            string password;
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();
                if (PasswordIsNotValid(password)) continue;

                password = GetHash(password);
            } while (PasswordIsIncorrect(email, password, users));

            Dictionary<string, string> data = new Dictionary<string, string>();
            data["email"] = email;
            data["password"] = password;
            return data;
        }

        public static Dictionary<string, string> EnterDataAtRegistration(List<User> users)
        {
            Console.WriteLine("----- Register -----");
            string firstName;
            do
            {
                Console.Write("Enter first name: ");
                firstName = Console.ReadLine();
            } while (NameIsNotValid(firstName));

            string lastName;
            do
            {
                Console.Write("Enter last name: ");
                lastName = Console.ReadLine();
            } while (NameIsNotValid(lastName));

            string email;
            do
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine();
            } while (EmailIsNotValid(email) || EmailAlreadyExist(email, users));

            string password;
            do
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();
            } while (PasswordIsNotValid(password));

            Dictionary<string, string> data = new Dictionary<string, string>();
            data["firstName"] = firstName;
            data["lastName"] = lastName;
            data["email"] = email;
            data["password"] = password;
            return data;
        }

        public static void SetPropertyPayment(Payment payment, PropertyInfo property, string value)
        {
            if (property.GetGetMethod().ReturnType == typeof(DateTime))
                property.SetValue(payment, DateTime.Parse(value));
            else if (property.GetGetMethod().ReturnType == typeof(decimal))
                property.SetValue(payment, decimal.Parse(value));
            else if (property.GetGetMethod().ReturnType == typeof(int))
                property.SetValue(payment, int.Parse(value));
            else
                property.SetValue(payment, value);
        }

        private static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
    }
}
