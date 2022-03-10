using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
	public class Members_discussions
	{
		[Key]
		public int Id { get; set; }
		public int User_id { get; set; }
		public int Discuss_id { get; set; }
		public Members_discussions(int id, int user_id, int discuss_id)
		{
			Id = id;
			User_id = user_id;
			Discuss_id = discuss_id;
		}
		public Members_discussions(): this(0, 0, 0)
        {}
	}
}
