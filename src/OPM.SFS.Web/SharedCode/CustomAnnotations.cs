using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OPM.SFS.Web.Shared
{
    public class CustomAnnotations : ValidationAttribute
    {
        public class PhoneNumber : RegularExpressionAttribute
        {
            private const string _phoneRegex = @"^[0-9.\-+\\/() ]+$";
            public PhoneNumber(string ErrorMessage = @"Phone Numbers must contain only numbers and any of the following characters (.),(+),(\),(/),(-), space and left and right parentheses.") : base(_phoneRegex) => base.ErrorMessage = ErrorMessage;
        }

        public class NoDangerousCharacters : RegularExpressionAttribute
        {
            private const string _dangerousCharsRegex = "^[^><&]+$";
            public override object TypeId => "nodangerouscharacters";
            public NoDangerousCharacters(string ErrorMessage = "Field may not contain the following characters (<),(>), and (&).") : base(_dangerousCharsRegex) => base.ErrorMessage = ErrorMessage;
        }

        public class EmailAddress : RegularExpressionAttribute
        {
            private const string _emailRegex = @"^([\w][\w#$%&'*+-/=?^_`{|}~]*[\w]|[\w])@(?![\w.]*sfs.gov[\w.]*)(([\w-]+[.])+[\w]+)$";
            public EmailAddress(string ErrorMessage = "This is an invalid email address. Please follow the format 'username@domain.com' and don't include any spaces.") : base(_emailRegex) => base.ErrorMessage = ErrorMessage;
        }
    }
}
