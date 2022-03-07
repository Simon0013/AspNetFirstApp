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
	}
}
