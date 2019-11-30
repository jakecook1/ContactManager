using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContactManagerWeb.Constants;
using ContactManagerWeb.Models;

namespace ContactManagerWeb.ViewModels
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }

        [Display(Name = "Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First")]
        public string FirstName { get; set; }

        [StringLength(1, ErrorMessage = "Initial can only be 1 character long.")]
        [Display(Name = "Int.")]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Home")]
        [Phone(ErrorMessage = "Please enter a correct number.")]
        public string HomePhone { get; set; }

        [Display(Name = "Mobile")]
        [Phone(ErrorMessage = "Please enter a correct number.")]
        public string CellPhone { get; set; }

        [Display(Name = "Ext.")]
        [Phone(ErrorMessage = "Please enter a correct number.")]
        public string OfficeExtension { get; set; }

        [Display(Name = "IRD No.")]
        [RegularExpression(@"\d{3}-? *\d{3}-? *-?\d{3}", ErrorMessage = "Please enter a correct IRD Number. e.g. (123-456-789)")]
        public string IrdNumber { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        public List<ContactImage> ContactImages { get; set; }

        public string ImagePublicId { get; set; }

        public string ThumbnailUrl
        { 
            get 
            {
                return $"{StringConstants.ImageBaseThumbnailUrl}/{ImagePublicId}";
            }
        }

        public string UploadDetails { get; set; }

        public bool DeletedImage { get; set; }
    }
}