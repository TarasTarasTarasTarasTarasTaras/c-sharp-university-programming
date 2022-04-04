namespace Staff_Project
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        private delegate void Action();

        static void Main(string[] args)
        {
            Logic logic = new Logic();

            Dictionary<string, Dictionary<int, Action>> map = new Dictionary<string, Dictionary<int, Action>>()
            {
                { "BaseMenu", new Dictionary<int, Action>() {
                    { 1, new Action(logic.Login) },
                    { 2, new Action(logic.Registration) }}
                },
                { "StaffMenu", new Dictionary<int, Action>() {
                    { 1, new Action(logic.CreateNewPayment) },
                    { 2, new Action(logic.EditPaymentWithDraftStatus) },
                    { 3, new Action(logic.DeletePaymentWithDraftStatus) },
                    { 4, new Action(logic.PrintAllUserPayments) },
                    { 5, new Action(logic.PrintAllUserPaymentsByFilter) },
                    { 6, new Action(logic.PrintAllApprovedPayments) },
                    { 7, new Action(logic.EditPaymentWithDraftStatus) },
                    { 8, new Action(logic.LogOut) }}
                },
                { "AdminMenu", new Dictionary<int, Action>() {
                    { 1, new Action(logic.CreateNewPayment) },
                    { 2, new Action(logic.EditPaymentWithDraftStatus) },
                    { 3, new Action(logic.DeletePaymentWithDraftStatus) },
                    { 4, new Action(logic.PrintAllUserPayments) },
                    { 5, new Action(logic.PrintAllUserPaymentsByFilter) },
                    { 6, new Action(logic.PrintAllApprovedPayments) },
                    { 7, new Action(logic.EditPaymentWithDraftStatus) },
                    { 8, new Action(logic.LogOut) },
                    { 9, new Action(logic.AdminPanelPrintAllPayments)},
                    {10, new Action(logic.AdminPanelPrintAllRejectedPayments) },
                    {11, new Action(logic.AdminPanelSetStatusOfPayment) }}
                }
            };

            while (true)
            {
                var action = logic.PrintMenu();
                if (action.Item2 == 0) break; // if exit
                map[action.Item1][action.Item2]();
            }
        }
    }
}
