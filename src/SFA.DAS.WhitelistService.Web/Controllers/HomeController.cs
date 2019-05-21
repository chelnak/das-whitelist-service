using System;
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
                logger.LogError(1, "Full Name cannot be null");
                return new BadRequestResult();
            }

            if (String.IsNullOrEmpty(indexViewModel.IPAddress))
            {
                logger.LogError(1, "IP Address cannot be null");
                return new BadRequestResult();
            }

            var WhitelistEntry = new AzureStorageQueueMessageEntity{
                Type = SupportedMessageTypeEnum.SQLServer,
                Name = indexViewModel.FullName.Trim(),
                IPAddress =indexViewModel.IPAddress
            };

            await sqlServerFirewallManagementService.AddWhitelistEntry(WhitelistEntry);
            logger.LogInformation(1, $"Submitting request for: {indexViewModel.FullName}");

            // return RedirectToAction("Index", "Home");
            return View("SubmitConfirmation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier });
        }
    }
}
