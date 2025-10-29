using Bookify.Domain.Consts;
using Bookify.Web.Core.ViewModels.User;
using Bookify.Application.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Bookify.Contracts.ViewModels.User;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = _userService.GetCreateUserViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _userService.GetRoles();
                return View(model);
            }

            var createdById = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var (success, errors) = _userService.CreateUser(model, createdById);

            if (!success)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);

                model.Roles = _userService.GetRoles();
                return View(model);
            }

            TempData["SuccessMessage"] = "User created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AllowUserName(CreateUserViewModel model)
        {
            var result = _userService.IsUserNameAllowed(model.UserName, model.Id);
            return Json(result);
        }

        public IActionResult AllowEmail(CreateUserViewModel model)
        {
            var result = _userService.IsEmailAllowed(model.Email, model.Id);
            return Json(result);
        }

        [HttpPost]
        public IActionResult ToggleStatus(string id)
        {
            var result = _userService.ToggleUserStatus(id);
            if (!result.Success)
                return StatusCode(500, new { message = result.Message });

            return Ok(new
            {
                isDeleted = result.IsDeleted,
                lastUpdatedOn = result.LastUpdatedOn?.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            var model = _userService.GetResetPasswordViewModel(id);
            if (model == null) return NotFound("User not found.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, errors) = _userService.ResetPassword(model);
            if (success)
            {
                TempData["SuccessMessage"] = "Password reset successfully.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in errors)
                ModelState.AddModelError(string.Empty, error);

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var model = _userService.GetEditUserViewModel(id);
            if (model == null) return NotFound("User not found.");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _userService.GetRoles();
                return View(model);
            }

            var (success, errors) = _userService.UpdateUser(model);
            if (!success)
            {
                foreach (var error in errors)
                    ModelState.AddModelError(string.Empty, error);

                model.Roles = _userService.GetRoles();
                return View(model);
            }

            TempData["SuccessMessage"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Unlock(string id)
        {
            var success = _userService.UnlockUser(id);
            if (!success)
                return StatusCode(500, new { message = "Failed to unlock the user." });

            return Ok(new { message = "User unlocked successfully." });
        }
    }
}
