using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class DatabaseController: Controller
	{
		[HttpPost]
		public IActionResult Registration(User user)
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
			return View("SuccessLogin");
        }
		[HttpPost]
		public IActionResult Autorisation(User user)
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
			return View("SuccessLogin");
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
			return View();
        }
		[HttpPost]
		public IActionResult CreateDiscussion(Discussion discussion)
        {
			if (string.IsNullOrEmpty(discussion.Theme))
            {
				ViewData["Message"] = "Тема дискуссии не задана";
				return View();
			}
			if (discussion.Creater == null)
            {
				ViewData["Message"] = "Не удалось определить создателя дискуссии";
				return View();
			}
			return View();
        }
		[HttpPost]
		public IActionResult AddComment(Comment comment)
        {
			if (string.IsNullOrEmpty(comment.Body))
            {
				ViewData["Message"] = "Тело комментария не задано";
				return View();
			}
			if (comment.Discuss == null)
            {
				ViewData["Message"] = "Не удалось определить дискуссию";
				return View();
			}
			if (comment.Creater == null)
            {
				ViewData["Message"] = "Не удалось определить автора комментария";
				return View();
			}
			return View();
        }
	}
}
