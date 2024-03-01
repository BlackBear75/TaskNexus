using System.ComponentModel.DataAnnotations;

namespace TaskNexus.Models.ViewModel.User
{
    public class RegisterViewModel
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

       


    }
}
