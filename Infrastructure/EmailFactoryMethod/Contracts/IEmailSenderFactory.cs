using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EmailFactoryMethod.Contracts
{
    public interface IEmailSenderFactory
    {
        IEmailSenderService Create(string implementation);
    }

}
