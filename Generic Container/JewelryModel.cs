namespace Generic_Container
{
    using System;
    using static Validation.Jewelry;

    class JewelryModel
    {
        private int _id;
        private string _title;
        private string _code;
        private string _material;
        private string _type;
        private DateTime _dateOfCreation;
        private decimal _price;

        public int Id                   { get => _id;               set { _id = ValidateId(value);                          }}
        public string Title             { get => _title;            set { _title = ValidateTitle(value);                    }}
        public string Code              { get => _code;             set { _code = ValidateCode(value);                      }}
        public string Material          { get => _material;         set { _material = ValidateMaterial(value);              }}
        public string Type              { get => _type;             set { _type = ValidateType(value);                      }}
        public DateTime DateOfCreation  { get => _dateOfCreation;   set { _dateOfCreation = ValidateDateOfCreation(value);  }}
        public decimal Price            { get => _price;            set { _price = ValidatePrice(value);                    }}

        public override string ToString()
        {
            var properties = Services<JewelryModel>.GetPropertiesClassService();

            string jewelry = String.Empty;

            foreach (var property in properties)
                jewelry += $"{property.Name}: {property.GetValue(this)}\n";

            return jewelry;
        }
    }
}