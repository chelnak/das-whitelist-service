using System;

namespace SFA.DAS.WhitelistService.Core
{
    public class SubmissionValidationResultEntity
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
