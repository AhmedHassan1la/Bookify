using Bookify.Contracts.ViewModels.User;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Application.Services.Users
{
    public interface IUserService
    {
        List<UserViewModel> GetAllUsers();
        CreateUserViewModel GetCreateUserViewModel();
        (bool Success, List<string> Errors) CreateUser(CreateUserViewModel model, string createdById);
        bool IsUserNameAllowed(string username, string? id);
        bool IsEmailAllowed(string email, string? id);
        (bool Success, string Message, bool IsDeleted, DateTime? LastUpdatedOn) ToggleUserStatus(string id);
        ResetPasswordViewModel? GetResetPasswordViewModel(string id);
        (bool Success, List<string> Errors) ResetPassword(ResetPasswordViewModel model);
        UserViewModel? GetEditUserViewModel(string id);
        (bool Success, List<string> Errors) UpdateUser(UserViewModel model);
        bool UnlockUser(string id);
        List<SelectListItem> GetRoles();
    }
}
