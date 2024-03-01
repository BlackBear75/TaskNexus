using System.ComponentModel.DataAnnotations;

namespace TaskNexus.Models.ViewModel.User
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }
      

    }
}
