
using Bookify.Application.Services.Authors;
using Bookify.Application.Services.BookCopies;
using Bookify.Application.Services.Books;
using Bookify.Application.Services.Categories;
using Bookify.Application.Services.Rentals;
using Bookify.Application.Services.Subscribers;
using Bookify.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
          
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookCopyService, BookCopyService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<ISubscriberService, SubscriberService>();
            services.AddScoped<IUserService, UserService>();
       


            return services;
        }
    }
}
