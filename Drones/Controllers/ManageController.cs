using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Drones.Models;
using Drones.Entities;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Drones.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _applicationDbContext;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationDbContext applicationDbContext)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            DbContext = _applicationDbContext;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationDbContext DbContext
        {
            get
            {
                return _applicationDbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _applicationDbContext = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // GET: /Manage/AddFarmerDetail
        public async Task<ActionResult> AddFarmerDetail()
        {
            FarmerDetailViewModel farmerDetailViewModel = new FarmerDetailViewModel() ;
            Farmer farmer = GetFarmerUserDetail();

            if (farmer != null)
            {
                farmerDetailViewModel.FirstName = farmer.Name;
                farmerDetailViewModel.LastName = farmer.Surname;
                farmerDetailViewModel.PhoneNumber = GetUserPhoneNumber();
                return View(farmerDetailViewModel);
            }
            else
            {
                return View();
            }
        }

        public string GetUserPhoneNumber() {
            string userId = User.Identity.GetUserId();
            var user = DbContext.Users.Where(x => x.Id.Equals(userId)).FirstOrDefault();
           return user.PhoneNumber;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveFarmerDetailAsync(FarmerDetailViewModel farmerDetailViewModel)
        {
            var userId = User.Identity.GetUserId();
            Farmer farmer = GetFarmerUserDetail();
            ApplicationUser user = UserManager.FindById(userId);

            if (farmer != null)
            {
                await UpdateUserDetail(farmerDetailViewModel, user);

                await UpdateFarmerUserDetail(farmerDetailViewModel);

                return RedirectToAction("Index", "Manage");
            }
            else
            {
                await AddUserDetail(farmerDetailViewModel, user);

                await AddFarmerUserDetail(farmerDetailViewModel);

                return RedirectToAction("AddFarmAddress", "Manage");
            }

        }

        public ActionResult AddUAVImage()
        {
            return View();
        }

        public ActionResult PrintReport()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadUAVImage(AddUAVImageViewModel uploadUAVImageViewModel) {
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                string pathToSave = Server.MapPath("~/Documents");
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
            }
            return RedirectToAction("PrintReport", "Manage");
        }

        public Farmer GetFarmerUserDetail()
        {
            Farmer farmer = new Farmer();
            string userId = User.Identity.GetUserId();
            farmer = DbContext.Farmer.Include("Farms").Where(x => x.UserId.Equals(userId)).SingleOrDefault();
            return farmer;

        }

        public ActionResult AddFarmerUserDetail()
        {
            return View();
        }

        public ActionResult AddFarmAddress()
        {
            AddFarmAddressViewModel addFarmAddressViewModel = new AddFarmAddressViewModel();
            Farmer farmer = GetFarmerUserDetail();
            if (farmer.Farms != null)
            {
                addFarmAddressViewModel.FarmSize = farmer.Farms.FarmSize;
                addFarmAddressViewModel.Latitude = farmer.Farms.Latitude;
                addFarmAddressViewModel.Longitude = farmer.Farms.Longitude;
                addFarmAddressViewModel.PostalCode = farmer.Farms.PostalCode;
                addFarmAddressViewModel.StreetAddress = farmer.Farms.StreetAddress;
                addFarmAddressViewModel.Suburb = farmer.Farms.Suburb;
                return View(addFarmAddressViewModel);
            }
            else
            {
                return View();
            }
         
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<ActionResult> AddFarmerUserDetail(FarmerDetailViewModel farmerDetailViewModel)
        {
            Farmer farmer = new Farmer();
            farmer.Name = farmerDetailViewModel.FirstName;
            farmer.Surname = farmerDetailViewModel.LastName;
            farmer.UserId = User.Identity.GetUserId();
            DbContext.Farmer.Add(farmer);
            await  DbContext.SaveChangesAsync();
            return RedirectToAction("AddFarmAddress", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task UpdateFarmerUserDetail(FarmerDetailViewModel farmerDetailViewModel)
        {
            Farmer farmer = GetFarmerUserDetail();
            farmer.Name = farmerDetailViewModel.FirstName;
            farmer.Surname = farmerDetailViewModel.LastName;
            await DbContext.SaveChangesAsync();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<ActionResult> AddUserDetail(FarmerDetailViewModel farmerDetailViewModel, ApplicationUser user)
        {
            string phoneNumber = farmerDetailViewModel.PhoneNumber;
            user.PhoneNumber = phoneNumber;
            user.PhoneNumberConfirmed = true;
            await DbContext.SaveChangesAsync();
            return RedirectToAction("AddFarmAddress", "Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private async Task<ActionResult> UpdateUserDetail(FarmerDetailViewModel farmerDetailViewModel, ApplicationUser user)
        {
            string phoneNumber = farmerDetailViewModel.PhoneNumber;
            user.PhoneNumber = phoneNumber;
            user.PhoneNumberConfirmed = true;
            await DbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Manage");
        }

        //TODO: refactor code into stardard form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeFarmAddressAsync(AddFarmAddressViewModel farmAddressViewModel)
        {
            Farmer farmer = GetFarmerUserDetail();

            if (farmer.Farms != null)
            {
                await UpdateFarmAddressAsync(farmAddressViewModel);
            }
            else
            {
                await SaveFarmAddressAsync(farmAddressViewModel);
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task SaveFarmAddressAsync(AddFarmAddressViewModel farmAddressViewModel)
        {
            Farm farm = new Farm();
            farm.StreetAddress = farmAddressViewModel.StreetAddress;
            farm.Suburb = farmAddressViewModel.Suburb;
            farm.PostalCode = farmAddressViewModel.PostalCode;
            farm.FarmSize = farmAddressViewModel.FarmSize;
            farm.Latitude = farmAddressViewModel.Latitude;
            farm.Longitude = farmAddressViewModel.Longitude;
            DbContext.Farms.Add(farm);
            await DbContext.SaveChangesAsync();

            Farmer farmer = GetFarmerUserDetail();
            farmer.Farms = farm;

            await DbContext.SaveChangesAsync();
        }
        private async Task UpdateFarmAddressAsync(AddFarmAddressViewModel farmAddressViewModel)
        {
            Farmer farmer = GetFarmerUserDetail();
            Farm  farm= farmer.Farms;
           
            farm.StreetAddress = farmAddressViewModel.StreetAddress;
            farm.Suburb = farmAddressViewModel.Suburb;
            farm.PostalCode = farmAddressViewModel.PostalCode;
            farm.FarmSize = farmAddressViewModel.FarmSize;
            farm.Latitude = farmAddressViewModel.Latitude;
            farm.Longitude = farmAddressViewModel.Longitude;
            await DbContext.SaveChangesAsync();
        }


        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}