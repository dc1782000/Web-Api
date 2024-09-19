﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Model
{
	public class Title
	{
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Products Products { get; set; }
    }
}

