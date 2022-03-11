using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class Book
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Название")]
		public string Name { get; set; }
		[Display(Name = "Издательство")]
		public string Publisher { get; set; }
		[Display(Name = "Год выпуска")]
		public int CreatingYear { get; set; }
		[Display(Name = "Количество в наличии")]
		public int Count { get; set; }
		[Display(Name = "Ссылка на содержимое")]
		public Uri ContentUri { get; set; }
		public string ImageUri { get; set; }
		
		public Book(int id, string name, string publisher, int creatingYear, int count, Uri contentUri, string imageUri)
		{
			Id = id;
			Name = name;
			Publisher = publisher;
			CreatingYear = creatingYear;
			Count = count;
			ContentUri = contentUri;
			ImageUri = imageUri;
		}
		public Book(): this(0, "", "", DateTime.Now.Year, 0, null, "")
        {}

		public bool addInDatabase(ApplicationContext db)
		{
			if (inDatabase(db))
				return false;
			try
			{
				db.Books.Add(this);
				db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}
		public bool dropFromDatabase(ApplicationContext db)
		{
			if (!inDatabase(db))
				return false;
			try
			{
				db.Books.Remove(this);
				db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}
		public static bool dropFromDatabaseById(int id, ApplicationContext db)
		{
			Book book = db.Books.Find(id);
			if (book == null)
				return false;
			try
			{
				db.Books.Remove(book);
				db.SaveChanges();
				return true;
			}
			catch
			{
				return false;
			}
		}
		public bool inDatabase(ApplicationContext db)
		{
			Book book = db.Books.Find(Id);
			if (book == null)
				return false;
			return true;
		}
	}
}
