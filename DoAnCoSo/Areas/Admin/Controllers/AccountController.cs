using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public AccountController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            var roles = new Dictionary<string, List<string>>();

            foreach (var user in users)
            {
                var userRoles = _userManager.GetRoles(user.Id);
                roles.Add(user.Id, userRoles.ToList());
            }

            ViewBag.Roles = roles;
            ViewBag.RoleList = new SelectList(_context.Roles, "Name", "Name");

            return View(users);
        }

        [Authorize(Roles = "Admin")]
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Create()
        {
            var roles = _context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            var model = new RegisterViewModel
            {
                RoleList = new SelectList(roles, "Value", "Text")
            };

            ViewData["RoleList"] = model.RoleList; // Thêm dòng này để gán giá trị cho ViewData

            return View(model);
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user.Id, model.SelectedRole);

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // Reload role list in case of validation error
            var roles = _context.Roles.Select(r => r.Name).ToList();
            model.RoleList = new SelectList(roles);
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [Authorize(Roles = "Admin")]
        // GET: Account/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            AddErrors(result);

            return View(user);
        }               

    }
}
