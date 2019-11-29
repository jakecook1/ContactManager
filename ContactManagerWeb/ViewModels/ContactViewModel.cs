using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ContactManagerWeb.Models;

namespace ContactManagerWeb.ViewModels
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(1, ErrorMessage = "Initial can only be 1 character long.")]
        [Display(Name = "Int.")]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Home")]
        public string HomePhone { get; set; }

        [Display(Name = "Mobile")]
        public string CellPhone { get; set; }

        [Display(Name = "Ext.")]
        public string OfficeExtension { get; set; }

        [Display(Name = "IRD No.")]
        public string IrdNumber { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        public List<ContactImage> ContactImages { get; set; }

        public ContactImage DefaultImage
        {
            get
            {
                return ContactImages != null ? ContactImages.FirstOrDefault() : null;
            }
        }
    }
}