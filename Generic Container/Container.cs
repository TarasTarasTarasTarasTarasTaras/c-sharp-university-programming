namespace Generic_Container
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    class Container<T> where T : new()
    {
        private string _filename;
        public List<T> Data { get; private set; }
        private PropertyInfo[] _properties;

        public int Count() => Data.Count;

        public Container(string filename)
        {
            SetFileName(filename);
            Data = new List<T>();
            _properties = Services<T>.GetPropertiesClassService();
            this.ReadFromFile();
        }

        public void Add(T _object)
        {
            Data.Add(_object);
            this.WriteToFile();
        }

        public List<T> Search(string searchString)
        {
            Console.WriteLine($">>> Search string: {searchString}");
            List<T> objects = new List<T>();

            foreach (T _object in Data)
                foreach (PropertyInfo property in _properties)
                    if (property.GetValue(_object).ToString().Contains(searchString))
                    {
                        objects.Add(_object);
                        break;
                    }
            Console.WriteLine($"Successfully found objects: {objects.Count}\n");
            return objects;
        }

        public void Edit(int id)
        {
            while (true)
            {
                T _object = this.Data[id];
                if (_object == null)
                {
                    Console.WriteLine("There is no object with this ID");
                    return;
                }

                int index = this.EnterIntForMenuEditAction(_properties);
                if (index == -1) return;

                Console.Write($"Enter new {_properties[index].Name}: ");
                string newValue = Console.ReadLine();
                try
                {
                    _properties[index].SetValue(_object, newValue);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        Console.WriteLine($">>> Error! {ex.InnerException.Message}");
                    else
                        Console.WriteLine($">>> Error! {ex.Message}");
                    continue;
                }

                Console.WriteLine($"\n{_properties[index].Name} edited successfully");
                this.WriteToFile();
            }
        }

        public void Sort(int key, SortingTypeEnum sortingType)
        {
            Data.Sort(new SortBy<T>(_properties[key]));
            if (sortingType == SortingTypeEnum.Descending)
                Data.Reverse();
            this.WriteToFile();
        }

        public void Delete(int id)
        {
            try
            {
                Data.RemoveAt(id);
            }
            catch
            {
                Console.WriteLine(">>> Error! There is no object with this ID");
                return;
            }
            Console.WriteLine($"Object with id {id} was deleted successfully");
            this.WriteToFile();
        }

        public void ReadFromFile()
        {
            StreamReader file = new StreamReader(Services<T>.PathProject + _filename);
            int numberOfObject = 1;

            while (!file.EndOfStream)
            {
                var propertiesDictionary = new Dictionary<string, string>();
                var line = file.ReadLine().Split(": ");
                if (line[0] == "") break;
                while (line[0][0] != '=')
                {
                    propertiesDictionary.Add(line[0], line[1]);
                    line = file.ReadLine().Split(": ");
                }

                T _object = new T();
                var errors = this.Deserialize(propertiesDictionary, _object);

                if (errors.Count > 0)
                {
                    Console.WriteLine($"Object number {numberOfObject} is defective!!!\n  Errors: {errors.Count}");
                    for (int i = 1; i <= errors.Count; ++i)
                        Console.WriteLine($">>> {i}. {errors[i - 1]}");
                    Console.WriteLine("------------------------------------------------------");
                }
                else
                    Data.Add(_object);

                numberOfObject++;
            }
            file.Close();
            Console.WriteLine(">>> Data was read from the file\n");
        }

        public async void WriteToFile()
        {
            StreamWriter sw = new StreamWriter(Services<T>.PathProject + _filename, false);
            await sw.WriteLineAsync(this.ToString());
            sw.Close();
            Console.WriteLine("The data was written to the file successfully");
        }

        public override string ToString()
        {
            string objects = String.Empty;
            foreach (var _object in Data)
                objects += _object.ToString() + "============================\n";
            return objects;
        }

        public void SetFileName(string filename)
            => _filename = filename;

        private List<string> Deserialize(Dictionary<string, string> propertiesDictionary, T _object)
        {
            List<string> errors = new List<string>();

            for (int i = 0; i < _properties.Length; i++)
            {
                try
                {
                    if (_properties[i].GetGetMethod().ReturnType == typeof(int))
                    {
                        int value = Int32.Parse(propertiesDictionary[_properties[i].Name]);
                        _properties[i].SetValue(_object, value);
                    }
                    else if (_properties[i].GetGetMethod().ReturnType == typeof(decimal))
                    {
                        decimal value = Decimal.Parse(propertiesDictionary[_properties[i].Name]);
                        _properties[i].SetValue(_object, value);
                    }
                    else if (_properties[i].GetGetMethod().ReturnType == typeof(DateTime))
                    {
                        DateTime value = DateTime.Parse(propertiesDictionary[_properties[i].Name]);
                        _properties[i].SetValue(_object, value);
                    }
                    else
                    {
                        _properties[i].SetValue(_object, propertiesDictionary[_properties[i].Name]);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        errors.Add($"{ex.InnerException.Message}");
                    else
                        errors.Add($"{ex.Message}");
                }
            }

            return errors;
        }

        private int EnterIntForMenuEditAction(PropertyInfo[] properties)
        {
            int index = -1;
            while (index < 0 || index >= properties.Length)
            {
                Console.WriteLine("\nWhich field do you want to edit?");
                for (int i = 0; i < properties.Length; i++)
                    Console.WriteLine($"  {i}. {properties[i].Name}");
                Console.WriteLine("  <. Back");

                string action = Console.ReadLine();
                if (action == "<") return -1;
                try
                {
                    index = Convert.ToInt32(action);
                }
                catch
                {
                    Console.WriteLine(">>> Error! You must enter a integer number!!!");
                }
            }
            return index;
        }
    }
}