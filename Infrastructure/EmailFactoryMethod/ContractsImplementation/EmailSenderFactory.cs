using Infrastructure.EmailFactoryMethod.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EmailFactoryMethod.ContractsImplementation
{
    public class EmailSenderFactory : IEmailSenderFactory
    {
        public IEmailSenderService Create(string sendEmailProvider) => sendEmailProvider switch
        {
            "Yahoo" => new SendEmailByYahoo(),
            "Gmail" => new SendEmailByGmail(),
            _ => new SendEmailByGmail(),
        };
    }
}
