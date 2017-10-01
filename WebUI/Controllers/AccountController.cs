using Domain.Abstract;
using Domain.ORMEntities;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebUI.Infrastructure;
using WebUI.Models;
using WebUI.Models.Entities;
using WebUI.Providers;

namespace WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IProfileReposiory profileReposiory;
        private readonly ISectionRepository sectionRepository;
        private readonly ISectionModeratorsRepository sectionModeratorsRepository;
        private readonly IRoleRepository roleRepository;
        public AccountController(IUserRepository repository,IProfileReposiory profileReposiory, ISectionRepository sectionRepository,ISectionModeratorsRepository sectionModeratorsRepository,IRoleRepository roleRepository)
        {
            this._repository = repository;
            this.profileReposiory = profileReposiory;
            this.sectionRepository = sectionRepository;
            this.sectionModeratorsRepository = sectionModeratorsRepository;
            this.roleRepository = roleRepository;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var type = HttpContext.User.GetType();
            var iden = HttpContext.User.Identity.GetType();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogOnViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(viewModel.Username, viewModel.Password))
                //Проверяет учетные данные пользователя и управляет параметрами пользователей
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Username, viewModel.RememberMe);
                    //Управляет службами проверки подлинности с помощью форм для веб-приложений
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Section");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password.");
                }
            }
            return View(viewModel);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            if (viewModel.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
            {
                ModelState.AddModelError("Captcha", "Incorrect input.");
                return View(viewModel);
            }

            var anyUser = _repository.GetAllUsers().Any(u => u.E_mail.Contains(viewModel.Email));

            if (anyUser)
            {
                ModelState.AddModelError("", "User with this e-mail address already registered.");
                return View(viewModel);
            }

            anyUser = _repository.GetAllUsers().Any(u => u.Username.Contains(viewModel.Username));

            if (anyUser)
            {
                ModelState.AddModelError("", "User with this username already registered.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                var membershipUser = ((CustomMembershipProvider)Membership.Provider)
                    .CreateUser(viewModel.Email, viewModel.Password,viewModel.Username);

                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Username, false);
                    return RedirectToAction("Index", "Section");
                }
                else
                {
                    ModelState.AddModelError("", "Error registration.");
                }
            }
            return View(viewModel);
        }

        //В сессии создаем случайное число от 1111 до 9999.
        //Создаем в ci объект CatchaImage
        //Очищаем поток вывода
        //Задаем header для mime-типа этого http-ответа будет "image/jpeg" т.е. картинка формата jpeg.
        //Сохраняем bitmap в выходной поток с форматом ImageFormat.Jpeg
        //Освобождаем ресурсы Bitmap
        //Возвращаем null, так как основная информация уже передана в поток вывод
        [AllowAnonymous]
        public ActionResult Captcha()
        {
            Session[CaptchaImage.CaptchaValueKey] =
                new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString(CultureInfo.InvariantCulture);
            var ci = new CaptchaImage(Session[CaptchaImage.CaptchaValueKey].ToString(), 211, 50, "Helvetica");

            // Change the response headers to output a JPEG image.
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();
            return null;
        }

        [ChildActionOnly]
        public ActionResult LoginPartial()
        {
            return PartialView("_LoginPartial");
        }
        [AllowAnonymous]
        public ActionResult Account(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Domain.ORMEntities.User user = _repository.GetUserByUsername(username);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel vm = user.ToViewModel();
            if (user.Username == this.User.Identity.Name) return View("Edit",vm);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account(UserViewModel uservm)
        {
            if (ModelState.IsValid)
            {
                if (uservm.File != null)
                {
                    byte[] image;
                    using (var binaryReader = new BinaryReader(uservm.File.InputStream))
                    {
                        image = binaryReader.ReadBytes(uservm.File.ContentLength);
                    }
                    Domain.ORMEntities.User user = new Domain.ORMEntities.User { UserId = uservm.Id, Image = image };
                    _repository.UpdateUser(user);
                }
                User_additional_info info = uservm.GetAdditionalInfo();
                profileReposiory.Update(info);
                return RedirectToRoute("UserProfile", new { username = uservm.Username });
            }
            return View(uservm);
        }
        [AllowAnonymous]
        public ActionResult DisplayAvatar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
            }
            User user = _repository.GetUserById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.Image == null)
            {
                string file_path = Server.MapPath("~/Content/noavatar.jpg");
                return File(file_path, "image/jpeg");
            }
            return File(user.Image, "image/jpeg");
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult GrantModeratorRights(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _repository.GetUserById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionId = new SelectList(sectionRepository.Sections, "SectionId", "Name");
            ToModerator toModerator = new ToModerator
            {
                UserId = user.UserId,
                Username = user.Username
            };
            return View(toModerator);
        }
        [HttpPost]
        public ActionResult GrantModeratorRights(ToModerator toModerator)
        {
            var entry = new SectionModerator
            {
                UserId = toModerator.UserId,
                SectionId = toModerator.SectionId,
                DateGranted = DateTime.Now
            };
            User user = _repository.GetUserById(toModerator.UserId);
            user.RoleId = roleRepository.GetAllRoles().Single(x => x.Name == "Moderator").RoleId;
            _repository.UpdateUser(user);
            sectionModeratorsRepository.Add(entry);
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            var model = _repository.GetAllUsers().Select(u => new UserEditByModerator()
            {
                UserId = u.UserId,
                Username = u.Username,
                RoleName = u.Role.Name,
                Reputation = u.Reputation ?? 0,
                SectionModerating = u.SectionModerators.ToList()
            }).ToList();
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Top()
        {
            var model = _repository.GetAllUsers().OrderByDescending(u => u.Reputation).Take(5);
            return PartialView(model);
        }
    }
}