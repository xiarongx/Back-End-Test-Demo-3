using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService
{
    public class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string FullName {
            get { if (MiddleName == null)
                {
                    return FirstName + ' ' + LastName;
                }
                else
                {
                    return FirstName + ' ' + MiddleName + ' '+ LastName;
                }
                 }
            set {; }
        }
        public Client(string first, string last)
        {
            FirstName = first;
            LastName = last;
        }
        public Client(string fullName)
        {
            string[] nameArray = fullName.Split(' ');
            if(nameArray.Length == 2)
            {
                FirstName = nameArray[0];
                LastName = nameArray[1];
            }
            else if(nameArray.Length == 3)
            {
                FirstName = nameArray[0];
                MiddleName = nameArray[1];
                LastName = nameArray[2];
            }
            else
            {
                throw new ArgumentException(fullName + " is not a valid name");
            }
            
        }

        public Client(string first, string last, int age, string location)
        {
            FirstName = first;
            LastName = last;
            Age = age;
            Location = location;
        }

        public Client(string fullName, int age, string location)
        {
            string[] nameArray = fullName.Split(' ');
            if (nameArray.Length == 2)
            {
                FirstName = nameArray[0];
                LastName = nameArray[1];
            }
            else if (nameArray.Length == 3)
            {
                FirstName = nameArray[0];
                MiddleName = nameArray[1];
                LastName = nameArray[2];
            }
            else
            {
                throw new ArgumentException(fullName + " is not a valid name");
            }

            Age = age;
            Location = location;
        }
    }
}
