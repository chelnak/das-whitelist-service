using System;

namespace SFA.DAS.WhitelistService.Web.Models
{
    public class IndexViewModel
    {
        public string FullName { get; set; }
        public string IPAddress {get; set;}
        public string ResourceGroupName { get; set; }
        public string ResourceName { get; set; }
    }
}