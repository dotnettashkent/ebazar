﻿using Shared.Infrastructures;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	public class UserEntity
	{
		[Column("id")]
		public long Id { get; set; }

		[Column("first_name")]
		public string FirstName { get; set; } = null!;

		[Column("last_name")]
		public string LastName { get; set; } = null!;

		[Column("middle_name")]
		public string? MiddleName { get; set; }

		[Column("email")]
		public string? Email { get; set; }

		[Column("phone_number")]
		public string PhoneNumber { get; set; } = null!;

		/*[Column("sms_code")]
		public string? SmsCode { get; set; }*/

		[Column("password")]
		public string Password { get; set; } = null!;


		[Column("gender")]
		public Gender Gender { get; set; }

		[Column("date_of_birth")]
		public DateTime? DateOfBirth { get; set; }

		[Column("created_at")]
		public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
		[Column("updated_at")]
		public DateTime? UpdatedAt { get; set; }

	}
}
