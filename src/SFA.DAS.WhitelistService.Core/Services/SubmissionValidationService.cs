using System;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Core.Services
{
    public class SubmissionValidationService : ISubmissionValidationService
    {
        public SubmissionValidationResultEntity Validate(string resourceName, string resourceGroupName, string fullName, string ipAddress)
        {
            var result = true;
            var message = "Submission is valid!";

            if (String.IsNullOrEmpty(resourceName))
            {
                message = "Resource Name cannot be null";
                result = false;
            }

            if (String.IsNullOrEmpty(resourceGroupName))
            {
                message = "Resource Group Name cannot be null";
                result = false;
            }

            if (String.IsNullOrEmpty(fullName))
            {
                message = "Full Name cannot be null";
                result = false;
            }

            if (String.IsNullOrEmpty(ipAddress))
            {
                message = "IP Address cannot be null";
                result = false;
            }

            IPAddress addr;
            if (!IPAddress.TryParse(ipAddress, out addr))
            {
                message = $"{ipAddress} is not valid";
                result = false;
            }
            
            return new SubmissionValidationResultEntity{IsValid = result, Message = message};
        }
    }
}
