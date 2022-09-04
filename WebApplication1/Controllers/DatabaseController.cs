using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Controllers
{
	public class DatabaseController: Controller
	{
		private ApplicationContext db;
		private IWebHostEnvironment _appEnvironment;
		public DatabaseController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
			db = context;
			_appEnvironment = appEnvironment;
        }
		private async Task Authenticate(string userName)
		{
			var claims = new List<Claim> {new Claim(ClaimsIdentity.DefaultNameClaimType, userName)};
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}
		[HttpPost]
		public async Task<IActionResult> Registration(User user)
        {
			if (string.IsNullOrEmpty(user.Name))
			{
				ViewData["Message"] = "Имя пользователя не задано";
				return View();
			}
			if (string.IsNullOrEmpty(user.Surname))
			{
				ViewData["Message"] = "Фамилия пользователя не задана";
				return View();
			}
			if (string.IsNullOrEmpty(user.Email))
			{
				ViewData["Message"] = "E-mail пользователя не задан";
				return View();
			}
			if (string.IsNullOrEmpty(user.Password))
			{
				ViewData["Message"] = "Пароль пользователя не задан";
				return View();
			}
			if (string.IsNullOrEmpty(user.Patronymic))
				user.Patronymic = "-";
			int indx1 = user.Email.IndexOf("@");
			int indx2 = user.Email.IndexOf(".");
			if ((indx1 < 1) || ((indx2 - indx1) < 2))
            {
				ViewData["Message"] = "E-mail введен некорректно";
				return View();
			}
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
			if (userFound == null)
			{
				if (user.addInDatabase(db))
                {
					await Authenticate(user.Email);
					return View("SuccessLogin");
				}
				else
                {
					ViewData["Message"] = "Не удалось зарегистрировать пользователя";
					return View();
				}
			}
			ViewData["Message"] = "Указанный e-mail уже зарегистрирован в портале";
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Autorisation(User user)
        {
			if (string.IsNullOrEmpty(user.Email))
            {
				ViewData["Message"] = "E-mail пользователя не задан";
				return View();
			}
			if (string.IsNullOrEmpty(user.Password))
			{
				ViewData["Message"] = "Пароль пользователя не задан";
				return View();
			}
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
			if (userFound != null)
			{
				await Authenticate(user.Email);
				return View("SuccessLogin");
			}
			ViewData["Message"] = "Неверный логин или пароль пользователя";
			return View();
        }
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> DelAccount(User user)
        {
			string email = User.Identity.Name;
			if (user.Email != email)
            {
				ViewData["Message"] = "Данные учётной записи введены неправильно!";
				return View();
            }
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound != null)
            {
				if (user.Password == userFound.Password)
                {
					if (Models.User.dropFromDatabaseById(userFound.Id, db))
                    {
						await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
						return RedirectToAction("Autorisation", "Home");
					}
					ViewData["Message"] = "Не удалось удалить учётную запись по техническим причинам! " + userFound.Id;
					return View();
				}
            }
			ViewData["Message"] = "Данные учётной записи введены неправильно!";
			return View();
		}
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> AddBook()
        {
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound != null)
            {
				if (userFound.IsAdmin)
					return View();
				return View("NotAdmin");
			}
			return RedirectToAction("Autorisation", "Home");
        }
		[HttpPost]
		public IActionResult AddBook(Book book)
        {
			if (string.IsNullOrEmpty(book.Name))
            {
				ViewData["Message"] = "Имя книги не задано";
				return View();
			}
			if (string.IsNullOrEmpty(book.Publisher))
            {
				ViewData["Message"] = "Издатель книги не задан";
				return View();
			}
			if (book.ContentUri == null)
            {
				ViewData["Message"] = "Ссылка на файл книги не задана";
				return View();
			}
			if (book.addInDatabase(db))
				return View("Books", db);
			ViewData["Message"] = "Не удалось добавить новую книгу";
			return View();
        }
		[Authorize]
		[HttpGet]
		public IActionResult AddBookImg(int id)
        {
			ViewData["BookId"] = id;
			return View();
        }
		[HttpPost]
		public IActionResult AddBookImg(BookImage img)
        {
			if (img.BookImg != null)
            {
				if (!img.BookImg.ContentType.Contains("image"))
                {
					ViewData["Message"] = "Это не изображение. Необходимо выбрать медиа файл с расширением png, jpg, jpeg и других форматов изображений.";
					return View();
				}
				string path = "/images/" + img.BookImg.FileName;
				using (FileStream fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
					img.BookImg.CopyTo(fileStream);
                }
				Book book = db.Books.Find(img.BookId);
				book.ImageUri = "~" + path;
				try
                {
					db.Books.Update(book);
					db.SaveChanges();
					return View("Books", db);
                }
				catch
                {
					ViewData["Message"] = "Не удалось добавить ссылку на изображение";
					return View();
				}
            }
			ViewData["Message"] = "Не выбрано изображение";
			return View();
        }
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> EditBook(int id)
		{
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound != null)
			{
				if (userFound.IsAdmin)
				{
					ViewData["Id"] = id;
					return View("AddBook", db.Books.Find(id));
				}
				return View("NotAdmin");
			}
			return RedirectToAction("Autorisation", "Home");
		}
		[HttpPost]
		public IActionResult EditBook(Book book)
        {
			if (string.IsNullOrEmpty(book.Name))
			{
				ViewData["Message"] = "Имя книги не задано";
				return View();
			}
			if (string.IsNullOrEmpty(book.Publisher))
			{
				ViewData["Message"] = "Издатель книги не задан";
				return View();
			}
			if (book.ContentUri == null)
			{
				ViewData["Message"] = "Ссылка на файл книги не задана";
				return View();
			}
			try
            {
				db.Books.Update(book);
				db.SaveChanges();
				return View("Books", db);
			}
			catch
            {
				ViewData["Message"] = "Не удалось редактировать книгу";
				ViewData["Id"] = book.Id;
				return View("AddBook", book);
			}
		}
		[Authorize]
		public async Task<IActionResult> DeleteBook(int id)
        {
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound != null)
			{
				if (userFound.IsAdmin)
				{
					if (Book.dropFromDatabaseById(id, db))
						return View("Books", db);
					ViewData["Message"] = "Не удалось удалить книгу";
					return View("Books", db);
				}
				return View("NotAdmin");
			}
			ViewData["Message"] = "Не удалось удалить книгу";
			return View("Books", db);
        }
		public IActionResult ShowDiscussion(int id)
        {
			Discussion discussion = db.Discussions.Find(id);
			if (discussion == null)
				return NotFound();
			Dictionary<ApplicationContext, Discussion> pair = new Dictionary<ApplicationContext, Discussion>();
			pair.Add(db, discussion);
			return View(pair);
        }
		[Authorize]
		[HttpGet]
		public IActionResult CreateDiscussion()
        {
			return View();
        }
		[HttpPost]
		public async Task<IActionResult> CreateDiscussion(Discussion discussion)
        {
			if (string.IsNullOrEmpty(discussion.Theme))
            {
				ViewData["Message"] = "Тема дискуссии не задана";
				return View();
			}
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound == null)
            {
				ViewData["Message"] = "Не удалось определить создателя дискуссии";
				return View();
			}
			discussion.Creater = userFound;
			if (discussion.addInDatabase(db))
			{
				Members_discussions member = new Members_discussions(0, userFound.Id, db.Discussions.ToList().Last().Id);
				db.Members_discussions.Add(member);
				db.SaveChanges();
				return View("Discussions", db);
			}
			ViewData["Message"] = "Не удалось добавить новую дискуссию";
			return View();
        }
		[Authorize]
		public async Task<IActionResult> JoinToDiscussion(int id)
        {
			var members = db.Members_discussions.Where(p => p.Discuss_id == id).ToList();
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			foreach (Members_discussions member in members)
            {
				if (member.User_id == userFound.Id)
                {
					ViewData["Message"] = "Вы уже состоите в этой дискуссии";
					return View("Discussions", db);
                }
            }
			try
            {
				Members_discussions member = new Members_discussions(0, userFound.Id, id);
				db.Members_discussions.Add(member);
				db.SaveChanges();
				ViewData["Message"] = "Теперь Вы участник дискуссии №" + id;
			}
			catch
            {
				ViewData["Message"] = "Произошла ошибка, не удалось присоединиться к дискуссии";
			}
			return View("Discussions", db);
		}
		[Authorize]
		public async Task<IActionResult> LeaveDiscussion(int id)
		{
			var members = db.Members_discussions.Where(p => p.Discuss_id == id).ToList();
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			foreach (Members_discussions member in members)
			{
				if (member.User_id == userFound.Id)
				{
					Members_discussions memberToDel = await db.Members_discussions.FirstOrDefaultAsync(p => (p.Discuss_id == id) && (p.User_id == userFound.Id));
					db.Members_discussions.Remove(memberToDel);
					db.SaveChanges();
					ViewData["Message"] = "Вы покинули дискуссию №" + id;
					return View("Discussions", db);
				}
			}
			ViewData["Message"] = "Вы не состоите в дискуссии";
			return View("Discussions", db);
		}
		[Authorize]
		public async Task<IActionResult> DeleteDiscussion(int id)
        {
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound != null)
			{
				if (userFound.IsAdmin || (userFound.Id == db.Discussions.Find(id).CreaterId))
				{
					List<Comment> commentsList = db.Comments.ToList();
					foreach (Comment comment in commentsList)
					{
						if (comment.DiscussId == id)
						{
							db.Comments.Remove(comment);
							db.SaveChanges();
						}
					}
					if (Discussion.dropFromDatabaseById(id, db))
						return View("Discussions", db);
					ViewData["Message"] = "Не удалось удалить дискуссию";
					return View("Discussions", db);
				}
				return View("NotAdmin");
			}
			ViewData["Message"] = "Не удалось удалить дискуссию";
			return View("Discussions", db);
		}
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> AddComment(int id)
		{
			List<Members_discussions> members = db.Members_discussions.Where(p => p.Discuss_id == id).ToList();
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			foreach (Members_discussions member in members)
            {
				if (member.User_id == userFound.Id)
                {
					ViewData["DiscussId"] = id;
					return View();
				}

			}
			ViewData["Message"] = "Чтобы оставить комментарий, Вам сначала надо присоединиться к дискуссии";
			return View("Discussions", db);
		}
		[HttpPost]
		public async Task<IActionResult> AddComment(Comment comment)
        {
			if (string.IsNullOrEmpty(comment.Body))
            {
				ViewData["Message"] = "Тело комментария не задано";
				return View();
			}
			if (comment.DiscussId == 0)
            {
				ViewData["Message"] = "Не удалось определить дискуссию";
				return View();
			}
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound == null)
			{
				ViewData["Message"] = "Не удалось определить автора комментария";
				return View();
			}
			comment.Creater = userFound;
			comment.Id = 0;
			if (comment.addInDatabase(db))
				return RedirectToAction("ShowDiscussion", new { id = comment.DiscussId });
			ViewData["Message"] = "Не удалось добавить новый комментарий";
			return View();
		}
		[Authorize]
		public async Task<IActionResult> DeleteComment(int id)
        {
			string email = User.Identity.Name;
			User userFound = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (userFound != null)
			{
				if (userFound.IsAdmin || (userFound.Id == db.Comments.Find(id).CreaterId))
				{
					int discussId = db.Comments.Find(id).DiscussId;
					if (Comment.dropFromDatabaseById(id, db))
						return RedirectToAction("ShowDiscussion", new { id = discussId });
					ViewData["Message"] = "Не удалось удалить комментарий";
					return RedirectToAction("ShowDiscussion", new { id = db.Comments.Find(id).DiscussId });
				}
				return View("NotAdmin");
			}
			ViewData["Message"] = "Не удалось удалить комментарий";
			return RedirectToAction("ShowDiscussion", new { id = db.Comments.Find(id).DiscussId });
		}
	}
}
