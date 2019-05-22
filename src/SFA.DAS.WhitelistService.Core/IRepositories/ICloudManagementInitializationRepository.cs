using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;

namespace SFA.DAS.WhitelistService.Core
{
    public interface ICloudManagementInitializationRepository
    {
        IAzure Initialize(string clientId, string clientSecret, string tenantId);
    }
}
