using System;

namespace WebApplication1.Models
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Patronymic { get; set; }
		public DateTime Registration { get; set; }
		public bool IsAdmin { get; set; }
		public string Email { get; set; }
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
		public bool addNewComment(string id, Discussion discussion)
        {
			Comment comment = new Comment(id, discussion, this);
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
