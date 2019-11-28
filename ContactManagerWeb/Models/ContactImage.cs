using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContactManagerWeb.Constants;

namespace ContactManagerWeb.Models
{
    public class ContactImage : IEntity
    {
        [Key]
        public int ContactImageId { get; set; }

        public string Url
        { 
            get 
            {
                return $"{StringConstants.ImageBaseUrl}/{Version}/{PublicId}";
            }
        }

        public string ThumbnailUrl
        { 
            get 
            {
                return $"{StringConstants.ImageBaseThumbnailUrl}/{Version}/{PublicId}";
            }
        }

        public string PublicId { get; set; }

        public string Version { get; set; }

        [NotMapped]
        public bool Delete { get; set; }

        public int ContactId { get; set; }

        public Contact Contact { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}