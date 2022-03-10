using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class Comment
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Комментарий")]
		public string Body { get; set; }
		[Display(Name = "Идентификатор дискуссии")]
		public int DiscussId { get; set; }
		public Discussion Discuss { get; set; }
		public int CreaterId { get; set; }
		public User Creater { get; set; }
		public DateTime CreatingDate { get; set; }

		public Comment(int id, string body, Discussion discussion, User creater, DateTime creatingDate)
		{
			Id = id;
			Body = body;
			Discuss = discussion;
			Creater = creater;
			CreatingDate = creatingDate;
		}
		public Comment(int id, string body, Discussion discussion, User creater):
			this(id, body, discussion, creater, DateTime.Now)
        {}
		public Comment(): this(0, "", null, null)
        {}

		public bool addInDatabase(ApplicationContext db)
		{
			if (inDatabase(db))
				return false;
			try
			{
				db.Comments.Add(this);
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
				db.Comments.Remove(this);
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
			Comment comment = db.Comments.Find(id);
			if (comment == null)
				return false;
			try
			{
				db.Comments.Remove(comment);
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
			Comment comment = db.Comments.Find(Id);
			if (comment == null)
				return false;
			return true;
		}
	}
}
