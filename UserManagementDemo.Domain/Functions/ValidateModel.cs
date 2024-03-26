using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Domain.Utilties
{
    public class ValidateModel
    {
        public static bool IsValidEmail(string emailAddress)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(emailAddress);
                return true;

            } catch (FormatException)
            {
                return false;
            }
        }

    }
}
