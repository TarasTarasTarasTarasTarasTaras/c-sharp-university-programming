namespace Generic_Container
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    class Menu<T> where T : new()
    {

        private static PropertyInfo[] properties;
        public delegate void Action(Container<T> container);

        public Menu()
        {
            properties = Services<T>.GetPropertiesClassService();
        }

        public readonly Dictionary<int, Action> dictionaryActions = new Dictionary<int, Action>()
            {
                { 1, new Action(Print) }, { 2, new Action(Add) }, { 3, new Action(Edit) },
                { 4, new Action(Delete) },{ 5, new Action(Search) }, { 6, new Action(Sort) },
            };

        private static void Print(Container<T> container)
        {
            Console.WriteLine(container.ToString());
        }

        private static void Add(Container<T> container)
        {
            string errors = string.Empty;
            var data = EnterData(properties);
            T _object = new T();

            foreach (PropertyInfo property in properties)
            {
                try
                {
                    property.SetValue(_object, data[property.Name]);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        errors += $">>> Error! {ex.InnerException.Message}\n";
                    else
                        errors += $">>> Error! {ex.Message}\n";
                }
            }
            if (errors != string.Empty) throw new BadModelException(errors);
            container.Add(_object);
            Console.WriteLine("The item was successfully added to the container");
        }

        private static void Edit(Container<T> container)
        {
            Console.Write("Enter id to edit: ");
            int editId = Int32.Parse(Console.ReadLine());
            container.Edit(editId);
            Console.WriteLine("edit completed");
        }

        private static void Delete(Container<T> container)
        {
            Console.Write("Enter id to delete: ");
            int deleteId = Int32.Parse(Console.ReadLine());
            container.Delete(deleteId);
            Console.WriteLine("delete completed");
        }

        private static void Search(Container<T> container)
        {
            Console.Write("Enter a value to search: ");
            string searchValue = Console.ReadLine();
            List<T> objects = container.Search(searchValue);
            Console.WriteLine(objects.ToString());
            Console.WriteLine("search completed");
        }

        private static void Sort(Container<T> container)
        {
            int index = -1;
            while (index < 0 || index >= properties.Length)
            {
                Console.WriteLine("Select a key to sort: ");
                for (int i = 0; i < properties.Length; i++)
                    Console.WriteLine($"  {i}. {properties[i].Name}");
                Console.WriteLine("  <. Back");

                string action = Console.ReadLine();
                if (action == "<") return;
                try
                {
                    index = Convert.ToInt32(action);
                }
                catch
                {
                    Console.WriteLine(">>> Error! You must enter a integer number!!!");
                }
            }

            Console.Write("Sort ascending or descending? (a/d): ");
            string sortType = string.Empty;
            while (sortType != "a" && sortType != "d")
                sortType = Console.ReadLine();

            if (sortType == "a") container.Sort(index, SortingTypeEnum.Ascending);
            else if (sortType == "d") container.Sort(index, SortingTypeEnum.Descending);
            Console.WriteLine("Sort completed successfully");
        }

        private static Dictionary<string, string> EnterData(PropertyInfo[] properties)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            foreach (PropertyInfo property in properties)
            {
                Console.Write($"Enter {property.Name}: ");
                data.Add(property.Name, Console.ReadLine());
            }

            return data;
        }
    }
}