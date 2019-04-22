using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Drones.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class FarmerDetailViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Phone]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }

    public class AddFarmAddressViewModel
    {
        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "Suburb")]
        public string Suburb { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display (Name = "Latitude")]
        public string Latitude { get; set; }
        [Required]
        [Display(Name = "Longitude")]
        public string Longitude { get; set; }
        [Required]
        [Display(Name = "Farm Size (HA)")]
        public int FarmSize { get; set; }
    }

    public class AddUAVImageViewModel {
        [Required]
        [Display(Name = "Select the images that you would like to upload:")]
        public string ImageText { get; set; }
    }
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class CropViewModel
    {
        [Required]
        [Display(Name = "Crop Description")]
        public string CropDescription { get; set; }

        [Required]
        [Display(Name = "Crop Name")]
        public string CropName { get; set; }

        [Required]
        [Display(Name = "Preferred Climate Minimum")]
        public int IdealClimateLowerRange { get; set; }

        [Required]
        [Display(Name = "Preferred Climate Maximum")]
        public int IdealClimateUpperRange { get; set; }

        [Required]
        [Display(Name = "Preferred Soil")]
        public string IdealSoil { get; set; }

        [Required]
        [Display(Name = "Common Pest")]
        public string MostCommonPest { get; set; }

        [Required]
        [Display(Name = "Soil Description")]
        public string SoilDescription { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}