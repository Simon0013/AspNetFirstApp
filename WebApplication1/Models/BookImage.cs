using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Models
{
	public class BookImage
	{
		[Display(Name = "Идентификатор книги")]
		public int BookId { get; set; }
		[Display(Name = "Выберите изображение")]
		public IFormFile BookImg { get; set; }
	}
}
