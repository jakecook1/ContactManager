using System;

namespace ContactManagerWeb.Models
{
    public class Contact : IEntity
    {
        public int ContactId { get; set; }

        public User User { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string HomePhone { get; set; }

        public string CellPhone { get; set; }

        public string OfficeExtension { get; set; }

        public string IrdNumber { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}