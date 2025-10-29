using Bookify.Contracts.ViewModels.Subscriber;
using Bookify.Contracts.ViewModels.Subscription;
using Bookify.Application.Services.Subscribers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Bookify.Web.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly ISubscriberService _subscriberService;

        public SubscribersController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        [HttpGet]
        public IActionResult Index(string? searchTerm)
        {
            var result = _subscriberService.SearchSubscribers(searchTerm);
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = _subscriberService.GetCreateViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubscriberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var vm = _subscriberService.GetCreateViewModel();
                model.Governorates = vm.Governorates;
                model.Areas = vm.Areas;
                return View(model);
            }

            var success = _subscriberService.CreateSubscriber(model);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to create subscriber.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Subscriber created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var subscriber = _subscriberService.GetSubscriberDetails(id);
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubscription([FromBody] SubscriptionRequest request)
        {
            var (success, message) = _subscriberService.AddSubscription(request);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { success = true, message });
        }
    }
}
