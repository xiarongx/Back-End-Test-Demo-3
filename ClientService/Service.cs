using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ClientService
{
    public class Service
    {
        public List<string> GetAllAdultClientName(Client[] clients)
        {
            List<string> clientNames = new List<string>();

            foreach(Client client in clients)
            {
                if (client.Age >= 18)
                {
                    clientNames.Add(client.FullName);
                }
            }
            return clientNames;
        }
    }
}
