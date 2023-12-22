using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SitePustok.ExternalServices.Interfaces;
using SitePustok.Helpers;
using SitePustok.Models;
using SitePustok.ViewModels.AuthVM;

namespace SitePustok.Contollers
{
    public class AuthController : Controller
    {
        SignInManager<AppUser> _signInManager { get; }
        UserManager<AppUser> _userManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }
        IEmailService _emailService { get; }
        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult SendMail()
        {
            _emailService.Send("adilibadov654@gmail.com", "Welcome Pustok", "Subject: Welcome to Site A Pustok - Your Gateway to a World of Knowledge!\r\n\r\nDear [Narmin],\r\n\r\nWelcome to Site A Pustok, your new destination for all things literature and knowledge! We are thrilled to have you on board and excited to embark on this journey together.\r\n\r\nAt Site A Pustok, we believe in the power of words to inspire, educate, and entertain. Whether you're a passionate reader, a knowledge seeker, or someone looking for a literary adventure, you've come to the right place.\r\n\r\nHere's what you can expect from your experience at Site A Pustok:\r\n\r\n1. **Diverse Collection:** Explore our extensive library featuring a diverse collection of books spanning various genres. From timeless classics to contemporary bestsellers, there's something for every taste.\r\n\r\n2. **Personalized Recommendations:** Our recommendation engine is designed to understand your preferences and suggest books tailored to your interests. Get ready to discover new favorites!\r\n\r\n3. **Community of Readers:** Join our vibrant community of readers and engage in discussions, book clubs, and events. Share your thoughts, connect with fellow book lovers, and expand your literary horizons.\r\n\r\n4. **Exclusive Offers:** As a valued member of Site A Pustok, you'll enjoy exclusive discounts, promotions, and early access to new releases. We believe in rewarding our community for their passion for reading.\r\n\r\nTo get started, simply log in to your account and begin exploring the rich tapestry of literature waiting for you. If you have any questions or need assistance, our support team is here to help.\r\n\r\nThank you for choosing Site A Pustok as your literary companion. We look forward to being a part of your reading adventures!\r\n\r\nHappy reading!\r\n\r\nBest regards,\r\n\r\n[Adil]\r\nSite A Pustok Team");
            return Ok();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string? returnUrl, LoginVM vm)
        {
            AppUser user;
            if (vm.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Too many attempts wait until " + DateTime.Parse(user.LockoutEnd.ToString()).ToString("HH:mm"));
                }
                else if (!user.EmailConfirmed)
                {
                    var param = new
                    {
                        username = user.UserName

                    };
                    ViewBag.Link = $"Confirm Your Email , <a href='{Url.Action("SendConfirmationEmail", "Auth", param)}'> Cick Here </a>";
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
                return View(vm);
            }
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new AppUser
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong. Please contact Us");
                return View(vm);
            }
            await _sendConfirmation(user);
            return RedirectToAction("Index", "Home");
        }


        //Email Confirmation Start

        public async Task<IActionResult> EmailConfirmed(string token, string username)
        {
            var result = await _userManager.ConfirmEmailAsync(await _userManager.FindByNameAsync(username), token);
            if (result.Succeeded) return Ok();
            return Problem();

        }
        async Task _sendConfirmation(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("EmailConfirmed", "Auth", new
            {
                token = token,
                username = user.UserName
            }, Request.Scheme);
            using StreamReader reader = new StreamReader(Path.Combine(PathConstants.RootPath, "ConfirmEmailTemplate.html"));
            string template =reader.ReadToEnd();
            template = template.Replace("[[[username]]]", user.UserName);
            template = template.Replace("[[[link]]]", link);
            _emailService.Send(user.Email, "Email confirmation", template);
        }

        public async Task<IActionResult> SendConfirmationEmail(string username)
        {
            await _sendConfirmation(await _userManager.FindByNameAsync(username));
            return Content("Email Sent");

        }

        //Email Confirmation End


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<bool> CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString()
                    });
                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
