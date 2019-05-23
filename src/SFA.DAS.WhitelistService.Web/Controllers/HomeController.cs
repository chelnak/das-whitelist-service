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
using SFA.DAS.WhitelistService.Core.Services;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        private readonly IFirewallMessageManagementService firewallMessageManagementService;
        private readonly ISubmissionValidationService submissionValidationService;

        public HomeController(ILogger<HomeController> _logger, IFirewallMessageManagementService _firewallMessageManagementService, ISubmissionValidationService _submissionValidationService)
        {
            logger = _logger;
            firewallMessageManagementService = _firewallMessageManagementService;
            submissionValidationService = _submissionValidationService;
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
            var validationResult = submissionValidationService.Validate(indexViewModel.ResourceGroupName, indexViewModel.ResourceName, indexViewModel.FullName, indexViewModel.IPAddress);
            if (!validationResult.IsValid){
                logger.LogError(1, validationResult.Message);
                return new BadRequestObjectResult(validationResult.Message);
            }

            logger.LogInformation(1, validationResult.Message);

            var WhitelistEntry = new QueueMessageEntity
            {
                Id = Guid.NewGuid().ToString(),
                Type = SupportedMessageTypeEnum.SQLServer,
                Name = indexViewModel.FullName.Trim(),
                IPAddress = indexViewModel.IPAddress,
                ResourceGroupName = indexViewModel.ResourceGroupName,
                ResourceName = indexViewModel.ResourceName
            };

            await firewallMessageManagementService.AddMessage(WhitelistEntry);
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
