using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Имя")]
		public string Name { get; set; }
		[Display(Name = "Фамилия")]
		public string Surname { get; set; }
		[Display(Name = "Отчество")]
		public string Patronymic { get; set; }
		[Display(Name = "Дата регистрации")]
		public DateTime Registration { get; set; }
		public bool IsAdmin { get; set; }
		[Display(Name = "Электронная почта")]
		public string Email { get; set; }
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		public User(int id, string name, string surname, string patronymic, DateTime registration, bool isAdmin, string email, string password)
		{
			Id = id;
			Name = name;
			Surname = surname;
			Patronymic = patronymic;
			Registration = registration;
			IsAdmin = isAdmin;
			Email = email;
			Password = password;
		}
		public User(int id, string name, string surname, DateTime registration, bool isAdmin, string email, string password):
			this(id, name, surname, "-", registration, isAdmin, email, password)
		{}
		public User(int id, string name, string surname, string patronymic, bool isAdmin, string email, string password):
			this(id, name, surname, patronymic, DateTime.Now, isAdmin, email, password)
		{}
		public User(int id, string name, string surname, bool isAdmin, string email, string password) :
			this(id, name, surname, DateTime.Now, isAdmin, email, password)
		{}
		public User(): this(0, "", "", false, "", "")
        {}

		public bool addInDatabase(ApplicationContext db)
        {
			if (inDatabase(db))
				return false;
			try
            {
				db.Users.Add(this);
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
				db.Users.Remove(this);
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
			User user = db.Users.Find(id);
			if (user == null)
				return false;
			try
            {
				db.Users.Remove(user);
				db.SaveChanges();
				return true;
            }
			catch
            {
				return false;
            }
		}
		public bool addNewComment(int id, string body, Discussion discussion, ApplicationContext db)
        {
			Comment comment = new Comment(id, body, discussion, this);
			if (comment.addInDatabase(db))
				return true;
			return false;
        }
		public bool createDiscussion(int id, string theme, ApplicationContext db)
        {
			Discussion discussion = new Discussion(id, theme, this);
			if (discussion.addInDatabase(db))
				return true;
			return false;
		}
		public bool inDatabase(ApplicationContext db)
        {
			User user = db.Users.Find(Id);
			if (user == null)
				return false;
			return true;
        }
	}
}
