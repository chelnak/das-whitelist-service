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
using SFA.DAS.WhitelistService.Web.Validators;
using SFA.DAS.WhitelistService.Web.Models;
using SFA.DAS.WhitelistService.Core.Services;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Web.Controllers
{
    [Authorize(Policy="ValidOrgsOnly")]
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        private readonly IFirewallMessageManagementService firewallMessageManagementService;

        public HomeController(ILogger<HomeController> _logger, IFirewallMessageManagementService _firewallMessageManagementService)
        {
            logger = _logger;
            firewallMessageManagementService = _firewallMessageManagementService;
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
            logger.LogInformation("Validating index view model");
            var validationResult = SubmissionValidator.Validate(indexViewModel);
            if (!validationResult.IsValid){
                logger.LogError(validationResult.Message);
                return new BadRequestObjectResult(validationResult.Message);
            }

            logger.LogInformation(validationResult.Message);

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
            logger.LogInformation($"Submitting request: {WhitelistEntry.Id}");

            return RedirectToAction("SubmitConfirmation");
        }

        [Route("SubmitConfirmation")]
        public ActionResult SubmitConfirmation()
        {
            return View("SubmitConfirmation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier });
        }
    }
}
