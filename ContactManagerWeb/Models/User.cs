using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ContactManagerWeb.Models
{
    public class User : IdentityUser
    {
        [DataMember]
        public override string Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        [DataMember]
        public override string UserName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        [DataMember]
        public override bool EmailConfirmed
        {
            get { return base.EmailConfirmed; }
        }

        [DataMember]
        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [DataMember]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string AddressFull
        {
            get 
            {
                var addressFull = new StringBuilder();

                if (!string.IsNullOrEmpty(AddressLine1))
                    addressFull.Append(AddressLine1);

                if (!string.IsNullOrEmpty(AddressLine2))
                {
                    if (addressFull.Length > 0)
                        addressFull.Append(", ");
                    
                    addressFull.Append(AddressLine2);
                }

                if (!string.IsNullOrEmpty(Suburb))
                {
                    if (addressFull.Length > 0)
                        addressFull.Append(", ");
                    
                    addressFull.Append(Suburb);
                }

                if (!string.IsNullOrEmpty(City))
                {
                    if (addressFull.Length > 0)
                        addressFull.Append(", ");
                    
                    addressFull.Append(City);
                }

                if (!string.IsNullOrEmpty(PostCode))
                {
                    if (addressFull.Length > 0)
                    {
                        addressFull.Append(string.IsNullOrEmpty(City) ? ", " : " ");
                    }

                    addressFull.Append(PostCode);
                }

                return addressFull.ToString();
            }
        }

        [DataMember]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string AddressLine2 { get; set; }

        [DataMember]
        public string Suburb { get; set; }

        [DataMember]
        public string PostCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public override string PhoneNumber
        {
            get { return base.PhoneNumber; }
            set { base.PhoneNumber = value; }
        }

        [NotMapped]
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public DateTime UpdatedAt { get; set; }
    }
}