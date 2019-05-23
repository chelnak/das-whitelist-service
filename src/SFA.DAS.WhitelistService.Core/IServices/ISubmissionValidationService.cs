using System;
using System.Threading.Tasks;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Core.IServices
{
    public interface ISubmissionValidationService
    {
        SubmissionValidationResultEntity Validate(string resourceName, string resourceGroupName, string fullName, string ipAddress);
    }
}
