using System;

namespace WebApplication1.Models
{
	public class Comment
	{
		public string Id { get; set; }
		public string Body { get; set; }
		public Discussion Discuss { get; set; }
		public User Creater { get; set; }
		public DateTime CreatingDate { get; set; }

		public Comment(string id, string body, Discussion discussion, User creater, DateTime creatingDate)
		{
			Id = id;
			Body = body;
			Discuss = discussion;
			Creater = creater;
			CreatingDate = creatingDate;
		}
		public Comment(string id, string body, Discussion discussion, User creater):
			this(id, body, discussion, creater, DateTime.Now)
        {}
		public Comment(): this("", "", null, null)
        {}

		public static Comment getCommentById(string id)
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
			if (!getCommentById(id).inDatabase())
				return false;
			return false;
		}
		public bool inDatabase()
		{
			return false;
		}
	}
}
