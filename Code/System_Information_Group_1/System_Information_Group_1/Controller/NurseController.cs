using System;
using System.Collections.Generic;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller
{
    public class NurseController
    {
        private NurseDal nurseDAL;
        private PersonDal personDAL;
        public NurseController()
        {
            this.nurseDAL = new NurseDal();
            this.personDAL = new PersonDal();
        }

        public List<NurseDisplay> HandleNurseSearch(String firstName, String lastName, DateTime? dateOfBirth)
        {
            return this.nurseDAL.GetNurseBy(firstName, lastName, dateOfBirth);
        }

        public bool UpdateNurse(NurseDisplay editedNurse)
        {
            if (editedNurse == null)
            {
                return false;

            }

            Person personToEdit = new Person(
                editedNurse.PersonId,
                editedNurse.LastName,
                editedNurse.FirstName,
                editedNurse.DateOfBirth,
                editedNurse.PhoneNumber,
                editedNurse.Address,
                editedNurse.Zip,
                editedNurse.City,
                editedNurse.State,
                editedNurse.Gender
            );

            this.personDAL.EditPerson(personToEdit);

            return true;
        }
    }
}
