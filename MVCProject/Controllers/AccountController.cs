using DataAccessLayer.Models;
using DataAccessLayer.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCProject.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            if(registerUserViewModel.Role == "")
            {
                ModelState.AddModelError("Role", "The Role field is required.");
            }

            if (ModelState.IsValid)
            {
                //Map ViewModel to ApplicationUser Model
                ApplicationUser user = new ApplicationUser();

                user.UserName = registerUserViewModel.UserName;
                user.Email = registerUserViewModel.Email;
                user.Address = registerUserViewModel.Address;
                //user.PasswordHash = registerUserViewModel.Password;

                IdentityResult result = await _userManager.CreateAsync(user, registerUserViewModel.Password);

                if (result.Succeeded)
                {
                    //Save user data in cookie to user it in the current session 
                    //await _signInManager.SignInAsync(user, false);

                    //await _userManager.AddToRoleAsync(user, "Admin");
                    await _userManager.AddToRoleAsync(user, registerUserViewModel.Role);

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }


            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser LoginUser = await _userManager.FindByNameAsync(loginViewModel.UserName);

                if(LoginUser != null)
                {
                    bool isFound = await _userManager.CheckPasswordAsync(LoginUser, loginViewModel.Password);

                    if(isFound)
                    {
                        //Username and password are correct, User is Exist

                        List<Claim> CustomClaims = new List<Claim>();

                        CustomClaims.Add(new Claim("Address", LoginUser.Address));
                        CustomClaims.Add(new Claim("Skill", "SQL"));

                        await _signInManager.SignInWithClaimsAsync(LoginUser, loginViewModel.RememberMe, CustomClaims);

                        //await _signInManager.SignInAsync(LoginUser, loginViewModel.RememberMe); //Save LoginUser data in cookie

                        return RedirectToAction("GetAll", "Student");
                    }
                }

                ModelState.AddModelError("", "Invalid Username or Password");
            }

            return View(loginViewModel);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return  RedirectToAction("Login");
        }

        public IActionResult TestUserProp()
        {
            if(User.Identity.IsAuthenticated)
            {
                Claim Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                Claim Id =  User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                Claim Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                Claim Address = User.Claims.FirstOrDefault(c => c.Type == "Address");

                return Content($"Welcome {Name?.Value}, Your Address is: {Address?.Value} Your Id is:  {Id?.Value}, Your Email is: {Email?.Value}");
            }

            return Content("User is not authenticated, Login First");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, 
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });

            return new EmptyResult();
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            //var claims = result.Principal.Identities
            //    .FirstOrDefault().Claims.Select(claim => new
            //    {
            //        claim.Issuer,
            //        claim.OriginalIssuer,
            //        claim.Type,
            //        claim.Value
            //    });

            if(result?.Principal == null)
            {
                return RedirectToAction("Login");
            }

            var Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var Name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            var User = await _userManager.FindByEmailAsync(Email);

            if(User == null)
            {
                // Correct the username: remove spaces and special characters
                var correctUsername = Name?.Replace(" ", "") ?? Email.Split('@')[0];


                // Check if username already exists, if so, append email prefix
                //var existingUser = await _userManager.FindByNameAsync(sanitizedUsername);
                //if (existingUser != null)
                //{
                //    sanitizedUsername = Email.Split('@')[0]; // Use email prefix as fallback
                //}

                User = new ApplicationUser();
                User.UserName = correctUsername;
                User.Email = Email;

                var createResult = await _userManager.CreateAsync(User);

                if (!createResult.Succeeded)
                {
                    // Handle user creation failure
                    return RedirectToAction("Login");
                }

                // Check if role exists before adding
                if (await _roleManager.RoleExistsAsync("Student"))
                {
                    var roleResult = await _userManager.AddToRoleAsync(User, "Student");

                    if (!roleResult.Succeeded)
                    {
                        
                    }
                }
                else
                {
                    // Create the Student role if it doesn't exist
                    await _roleManager.CreateAsync(new IdentityRole("Student"));
                    await _userManager.AddToRoleAsync(User, "Student");
                }
            }

            await _signInManager.SignInAsync(User, isPersistent: true);

            return RedirectToAction("GetAll", "Student");
        }
    }
}
