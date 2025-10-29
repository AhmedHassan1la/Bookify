using Bookify.Contracts.ViewModels.User;
using Bookify.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserViewModel> GetAllUsers()
        {
            return _userManager.Users
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName!,
                    Email = u.Email!,
                    IsDeleted = u.IsDeleted,
                    CreatedOn = u.CreatedOn,
                    LastUpdatedOn = u.LastUpdatedOn,
                    IsLockedOut = _userManager.IsLockedOutAsync(u).Result
                }).ToList();
        }

        public CreateUserViewModel GetCreateUserViewModel()
        {
            return new CreateUserViewModel
            {
                Roles = GetRoles()
            };
        }

        public (bool Success, List<string> Errors) CreateUser(CreateUserViewModel model, string createdById)
        {
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                CreatedById = createdById
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToList());

            if (model.SelectedRoles != null && model.SelectedRoles.Any())
                _userManager.AddToRolesAsync(user, model.SelectedRoles).Wait();

            return (true, new List<string>());
        }

        public bool IsUserNameAllowed(string username, string? id)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            return user == null || user.Id == id;
        }

        public bool IsEmailAllowed(string email, string? id)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            return user == null || user.Id == id;
        }

        public (bool Success, string Message, bool IsDeleted, DateTime? LastUpdatedOn) ToggleUserStatus(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return (false, "User not found.", false, null);

            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedOn = DateTime.UtcNow;

            var result = _userManager.UpdateAsync(user).Result;

            if (!result.Succeeded)
                return (false, "Failed to update user status.", user.IsDeleted, user.LastUpdatedOn);

            return (true, "Success", user.IsDeleted, user.LastUpdatedOn);
        }

        public ResetPasswordViewModel? GetResetPasswordViewModel(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            return user == null ? null : new ResetPasswordViewModel { UserId = id };
        }

        public (bool Success, List<string> Errors) ResetPassword(ResetPasswordViewModel model)
        {
            var user = _userManager.FindByIdAsync(model.UserId).Result;
            if (user == null)
                return (false, new List<string> { "User not found." });

            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var result = _userManager.ResetPasswordAsync(user, token, model.Password).Result;

            return result.Succeeded
                ? (true, new List<string>())
                : (false, result.Errors.Select(e => e.Description).ToList());
        }

        public UserViewModel? GetEditUserViewModel(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            return new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                IsDeleted = user.IsDeleted,
                CreatedOn = user.CreatedOn,
                LastUpdatedOn = user.LastUpdatedOn,
                SelectedRoles = _userManager.GetRolesAsync(user).Result.ToList(),
                Roles = GetRoles()
            };
        }

        public (bool Success, List<string> Errors) UpdateUser(UserViewModel model)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null)
                return (false, new List<string> { "User not found." });

            if (model.UserName != user.UserName &&
                _userManager.Users.Any(u => u.UserName == model.UserName))
            {
                return (false, new List<string> { "The username is already taken." });
            }

            if (model.Email != user.Email &&
                _userManager.Users.Any(u => u.Email == model.Email))
            {
                return (false, new List<string> { "The email is already taken." });
            }

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.IsDeleted = model.IsDeleted;
            user.LastUpdatedOn = DateTime.Now;

            var updateResult = _userManager.UpdateAsync(user).Result;

            return updateResult.Succeeded
                ? (true, new List<string>())
                : (false, updateResult.Errors.Select(e => e.Description).ToList());
        }

        public bool UnlockUser(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null) return false;

            var result = _userManager.SetLockoutEndDateAsync(user, null).Result;
            return result.Succeeded;
        }

        public List<SelectListItem> GetRoles()
        {
            return _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
        }
    }
}
