﻿using MemoryPack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	public class CourierEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("order_id")]
		public long OrderId { get; set; }

		[Column("last_name")]
		public string LastName { get; set; } = null!;

		[Column("first_name")]
		public string FirstName { get; set; } = null!;

		[Column("middle_name")]
		public string MiddleName {  get; set; } = null!;

		[Column("phone_number")]
		public string PhoneNumber { get; set; } = null!;

		[Column("password")]
		public string Password { get; set; } = null!;

		[Column("passport_number")]
		public string PassportNumber { get; set; } = null!;

		[Column("passport_letter")]
		public string PassportLetter { get; set; } = null!;

		[Column("passport_PINFL")]
		public string PassportPINFL {  get; set; } = null!;
	}
}
