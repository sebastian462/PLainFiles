using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainFiles.Core
{
    public  class Validator
    {
        public static bool ValidId(string? input, out int id)
        {
            return int.TryParse(input, out id) && id > 0;
        }

        public static bool ValidName(string? input)
        {
            return !string.IsNullOrWhiteSpace(input) && input.All(char.IsLetter);
        }

        public static bool ValidPhone(string? input)
        {
            return !string.IsNullOrWhiteSpace(input)
                  && input.Length >= 7 && input.Length <= 10
                  && input.All(char.IsDigit);
        }


        public static bool ValidBalance(string? input, out decimal balance)
        {
            return decimal.TryParse(input, out balance) && balance >=0;
        }
          
    }
}
