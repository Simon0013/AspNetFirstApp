using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class User
	{
		public string Id { get; set; }
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

		public User(string id, string name, string surname, string patronymic, DateTime registration, bool isAdmin, string email, string password)
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
		public User(string id, string name, string surname, DateTime registration, bool isAdmin, string email, string password):
			this(id, name, surname, "-", registration, isAdmin, email, password)
		{}
		public User(string id, string name, string surname, string patronymic, bool isAdmin, string email, string password):
			this(id, name, surname, patronymic, DateTime.Now, isAdmin, email, password)
		{}
		public User(string id, string name, string surname, bool isAdmin, string email, string password) :
			this(id, name, surname, DateTime.Now, isAdmin, email, password)
		{}
		public User(): this("", "", "", false, "", "")
        {}

		public static User getUserById(string id)
        {
			return null;
        }
		public bool addInDatabase()
        {
			if (inDatabase())
				return false;
			return false;
		}
		public bool dropFromDatabase()
        {
			if (!inDatabase())
				return false;
			return false;
		}
		public static bool dropFromDatabaseById(string id)
        {
			if (!getUserById(id).inDatabase())
				return false;
			return false;
		}
		public bool addNewComment(string id, string body, Discussion discussion)
        {
			Comment comment = new Comment(id, body, discussion, this);
			if (comment.addInDatabase())
				return true;
			return false;
        }
		public bool createDiscussion(string id, string theme)
        {
			Discussion discussion = new Discussion(id, theme, this);
			if (discussion.addInDatabase())
				return true;
			return false;
		}
		public bool inDatabase()
        {
			return false;
        }
	}
}
