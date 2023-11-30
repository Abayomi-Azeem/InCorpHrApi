using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Common
{
    public class ResponseMessages
    {
        public const string INVALID_EMAIL_PASS = "Email or Password is Incorrect";
        public const string USER_EXISTS = "User with Email already Exists";
        public const string USER_CREATION_FAILED = "Registration Failed";
        public const string NOT_FOUND = "Record Not Found";
        public const string FAILURE = "Request Failed. Kindly Try again!";
        public const string JOB_NOTFOUND = "Job Not Found";
        public const string JOB_EXPIRED = "Cannot Apply for Expired Job";
        public const string UNVERIFIED_PROFILE = "Unverified Profiles cannot log in.";
    }
}
