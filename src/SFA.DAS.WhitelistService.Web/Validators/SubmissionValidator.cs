using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.WhitelistService.Web.Models;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Web.Validators
{
    public class SubmissionValidator
    {
        public static SubmissionValidationResultEntity Validate(IndexViewModel indexViewModel)
        {
            var result = true;
            var message = "Submission is valid!";

            if (String.IsNullOrEmpty(indexViewModel.ResourceName))
            {
                message = "Resource Name cannot be null";
                result = false;
            }

            if (String.IsNullOrEmpty(indexViewModel.ResourceGroupName))
            {
                message = "Resource Group Name cannot be null";
                result = false;
            }

            if (String.IsNullOrEmpty(indexViewModel.FullName))
            {
                message = "Full Name cannot be null";
                result = false;
            }

            if (String.IsNullOrEmpty(indexViewModel.IPAddress))
            {
                message = "IP Address cannot be null";
                result = false;
            }

            IPAddress addr;
            if (!IPAddress.TryParse(indexViewModel.IPAddress, out addr))
            {
                message = $"{indexViewModel.IPAddress} is not valid";
                result = false;
            }
            
            return new SubmissionValidationResultEntity{IsValid = result, Message = message};
        }
    }
}
