using Infrastructure.EmailFactoryMethod.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EmailFactoryMethod.ContractsImplementation
{
    public class SendEmailByYahoo : IEmailSenderService
    {
        public void SendEmail()
        {
            Console.WriteLine("Sen Email By ");
        }
    }
}
