using System;
using System.Threading.Tasks;

namespace SFA.DAS.WhitelistService.Core
{
    public interface ISubmissionValidationService
    {
        SubmissionValidationResultEntity Validate(string fullName, string ipAddress);
    }
}
