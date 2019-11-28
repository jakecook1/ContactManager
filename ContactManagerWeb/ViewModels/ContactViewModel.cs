using ContactManagerWeb.Models;

namespace ContactManagerWeb.ViewModels
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string HomePhone { get; set; }

        public string CellPhone { get; set; }

        public string OfficeExtension { get; set; }

        public string IrdNumber { get; set; }

        public bool Active { get; set; }
    }
}