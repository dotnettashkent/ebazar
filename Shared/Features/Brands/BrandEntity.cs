﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Features
{
    [Table("brands")]
    public class BrandEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        public string Link { get; set; } = null!;

        [Column("photo")]
        public string? ImageOne { get; set; }
    }
}
