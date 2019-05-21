using System;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using SFA.DAS.WhitelistService.Web.Models;
using SFA.DAS.WhitelistService.Core;

namespace SFA.DAS.WhitelistService.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        private readonly ISQLServerFirewallManagementService sqlServerFirewallManagementService;

        public HomeController(ILogger<HomeController> _logger, ISQLServerFirewallManagementService _sqlServerFirewallManagementService)
        {
            logger = _logger;
            sqlServerFirewallManagementService = _sqlServerFirewallManagementService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index", new IndexViewModel());
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(IndexViewModel indexViewModel)
        {
            if (String.IsNullOrEmpty(indexViewModel.FullName))
            {
                var fullNameValidationMessage = "Full Name cannot be null";
                logger.LogError(1, fullNameValidationMessage);
                return new BadRequestResult();
            }

            if (String.IsNullOrEmpty(indexViewModel.IPAddress))
            {
                var ipAddressValidationMessage = "IP Address cannot be null";
                logger.LogError(1, ipAddressValidationMessage);
                return new BadRequestObjectResult(ipAddressValidationMessage);
            }

            IPAddress ipAddress;
            if (!IPAddress.TryParse(indexViewModel.IPAddress, out ipAddress))
            {
                var ipAddressFormatValidationMessage = $"{indexViewModel.IPAddress} is not valid";
                logger.LogError(1, ipAddressFormatValidationMessage);
                return new BadRequestObjectResult(ipAddressFormatValidationMessage);
            }

            var WhitelistEntry = new QueueMessageEntity
            {
                Id = Guid.NewGuid().ToString(),
                Type = SupportedMessageTypeEnum.SQLServer,
                Name = indexViewModel.FullName.Trim(),
                IPAddress = indexViewModel.IPAddress
            };

            await sqlServerFirewallManagementService.AddWhitelistEntry(WhitelistEntry);
            logger.LogInformation(1, $"Submitting request: {WhitelistEntry.Id}");

            return View("SubmitConfirmation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier });
        }
    }
}
