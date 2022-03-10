using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class Discussion
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Тема дискуссии")]
		public string Theme { get; set; }
		public int CreaterId { get; set; }
		public User Creater { get; set; }
		public DateTime CreatingDate { get; set; }
		
		public Discussion(int id, string theme, User creater, DateTime creatingDate)
		{
			Id = id;
			Theme = theme;
			Creater = creater;
			CreatingDate = creatingDate;
		}
		public Discussion(int id, string theme, User creater):
			this(id, theme, creater, DateTime.Now)
		{}
		public Discussion(): this(0, "", null)
		{}

		public bool addInDatabase(ApplicationContext db)
		{
			if (inDatabase(db))
				return false;
			try
			{
				db.Discussions.Add(this);
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
				db.Discussions.Remove(this);
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
			Discussion discussion = db.Discussions.Find(id);
			if (discussion == null)
				return false;
			try
			{
				db.Discussions.Remove(discussion);
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
			Discussion discussion = db.Discussions.Find(Id);
			if (discussion == null)
				return false;
			return true;
		}
	}
}
