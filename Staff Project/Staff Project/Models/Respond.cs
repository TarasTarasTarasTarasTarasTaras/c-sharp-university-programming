using System;
using System.Collections.Generic;
using System.Text;

namespace Staff_Project.Models
{
    public class Respond : Payment
    {
        public string Message { get; set; }

        public override string ToString()
        {
            string respond = base.ToString();
            respond += $"--> Message: {Message}\n";
            return respond;
        }
    }
}
