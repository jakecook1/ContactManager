using System;
using System.Text;

namespace ContactManagerWeb.Models
{
    public class Contact : IEntity
    {
        public int ContactId { get; set; }

        public User User { get; set; }

        public string FullName 
        {
            get
            {
                var fullName = new StringBuilder();

                 if (!string.IsNullOrEmpty(FirstName))
                    fullName.Append(FirstName);

                if (!string.IsNullOrEmpty(MiddleInitial))
                {
                    if (fullName.Length > 0)
                        fullName.Append(" ");
                    
                    fullName.Append(MiddleInitial);
                }

                if (!string.IsNullOrEmpty(LastName))
                {
                    if (fullName.Length > 0)
                        fullName.Append(" ");
                    
                    fullName.Append(LastName);
                }

                return fullName.ToString();
            }
        }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string HomePhone { get; set; }

        public string CellPhone { get; set; }

        public string OfficeExtension { get; set; }

        public string IrdNumber { get; set; }

        public string ImagePublicId { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}