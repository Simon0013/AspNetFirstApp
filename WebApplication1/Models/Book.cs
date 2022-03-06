using System;

namespace WebApplication1.Models
{
	public class Book
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Publisher { get; set; }
		public int CreatingYear { get; set; }
		public int Count { get; set; }
		public Uri ContentUri { get; set; }
		
		public Book(string id, string name, string publisher, int creatingYear, int count, Uri contentUri)
		{
			Id = id;
			Name = name;
			Publisher = publisher;
			CreatingYear = creatingYear;
			Count = count;
			ContentUri = contentUri;
		}

		public static Book getBookById(string id)
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
