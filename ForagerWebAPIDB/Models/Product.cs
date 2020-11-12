﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
 using Microsoft.AspNetCore.Authorization.Infrastructure;

 namespace ForagerWebAPIDB.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, MaxLength(64)] 
        public string Name { get; set; }
        [Required]
        public string ProductCategory { get; set; }
        public string ImagesString { get; set; }
        [NotMapped]
        public List<string> Images { get; set; }
        
        
        public Product()
        {
            if (!(string.IsNullOrEmpty(ImagesString)))
                Images = JsonSerializer.Deserialize<List<string>>(ImagesString);
            else
                Images = new List<string>();
        }

        public void addImage(string url)
        {
            Images.Add(url);
        }
        public override string ToString()
        {
            return $"{nameof(ProductId)}: {ProductId}, {nameof(Name)}: {Name}, {nameof(ProductCategory)}: {ProductCategory}, {nameof(ImagesString)}: {ImagesString}, {nameof(Images)}: {Images}";
        }
    }
}
