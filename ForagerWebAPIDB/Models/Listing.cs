﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForagerWebAPIDB.Models
{
    public class Listing
    {
        [Key]
        public int ListingId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        [Required]
        public Product Product { get; set; }
        [Required, Range(0,999999)]
        public double Price { get; set; }
        [Required, Range(0,999999)]
        public double Quantity { get; set; }
        [Required, MaxLength(16)]
        public string Unit { get; set; }
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string BestBefore { get; set; }
        [Required, MaxLength(256)]
        public string PickupAddress { get; set; }
        [Required, MaxLength(16)]
        public string Postcode { get; set; }
        [Required]
        public bool HasDelivery { get; set; }
        public string PictureList { get; set; }
        public int NumberOfViews { get; set; }
        [Required]
        public bool IsArchived { get; set; }
        [MaxLength(512)]
        public string Comment { get; set; }
        [NotMapped] public List<string> Pictures { get; set; }

        public Listing()
        {
            if (!(string.IsNullOrEmpty(PictureList)))
                Pictures = JsonSerializer.Deserialize<List<string>>(PictureList);
            else
                Pictures = new List<string>();
        }
        public String getCover()
        {
            if (Pictures.Count == 0)
                Pictures = Product.Images;
            return Pictures[0];
        }

        public override string ToString()
        {
            return $"{nameof(ListingId)}: {ListingId}, {nameof(UserId)}: {UserId}, {nameof(ProductId)}: {ProductId}, {nameof(Product)}: {Product}, {nameof(Price)}: {Price}, {nameof(Quantity)}: {Quantity}, {nameof(Unit)}: {Unit}, {nameof(StartDate)}: {StartDate}, {nameof(BestBefore)}: {BestBefore}, {nameof(PickupAddress)}: {PickupAddress}, {nameof(Postcode)}: {Postcode}, {nameof(HasDelivery)}: {HasDelivery}, {nameof(PictureList)}: {PictureList}, {nameof(NumberOfViews)}: {NumberOfViews}, {nameof(IsArchived)}: {IsArchived}, {nameof(Comment)}: {Comment}, {nameof(Pictures)}: {Pictures}";
        }
    }
}
