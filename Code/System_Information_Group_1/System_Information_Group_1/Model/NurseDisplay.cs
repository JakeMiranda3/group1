using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Information_Group_1.Model
{
    public class NurseDisplay
    {
        public int NurseId { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public String Zip { get; set;  }

        public String City { get; set;  }

        public String State { get; set; }

        public NurseDisplay(int NurseId, int personId, String firstName, String lastName, DateTime dateOfBirth, String gender, String phoneNumber, String address, String zip, String city, String state) 
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("firstName cannot be null or empty.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("lastName cannot be null or empty.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("phoneNumber cannot be null or empty.", nameof(phoneNumber));
            
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("address cannot be null or empty.", nameof(address));
            if (string.IsNullOrWhiteSpace(zip))
                throw new ArgumentException("address cannot be null or empty.", nameof(zip));
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("address cannot be null or empty.", nameof(city));
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("address cannot be null or empty.", nameof(state));

            this.NurseId = NurseId;
            this.PersonId = personId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.PhoneNumber = phoneNumber;
            this.Address = address;
            this.Zip = zip;
            this.City = city;
            this.State = state;
        }
    }
}
