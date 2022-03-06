using System;

namespace WebApplication1.Models
{
	public class Discussion
	{
		public string Id { get; set; }
		public string Theme { get; set; }
		public User Creater { get; set; }
		public DateTime CreatingDate { get; set; }
		
		public Discussion(string id, string theme, User creater, DateTime creatingDate)
		{
			Id = id;
			Theme = theme;
			Creater = creater;
			CreatingDate = creatingDate;
		}
		public Discussion(string id, string theme, User creater):
			this(id, theme, creater, DateTime.Now)
		{}

		public static Discussion getDiscussionById(string id)
		{
			return null;
		}
		public bool addInDatabase()
		{
			if (inDatabase())
				return false;
			return false;
		}
		public bool inDatabase()
		{
			return false;
		}
	}
}
