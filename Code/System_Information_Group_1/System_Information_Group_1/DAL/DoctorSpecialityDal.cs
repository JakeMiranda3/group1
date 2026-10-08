using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_Information_Group_1.Model;
using MySqlConnector;
using DBAccess.DAL;
namespace System_Information_Group_1.DAL
{
    public class DoctorSpecialityDal
    {
        #region Access Methods
        public List<DoctorSpeciality> getDoctorSpecialitiesByPersonId(int id)
        {

            var specialities = new List<DoctorSpeciality>();

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = "select * from doctor_speciality where person_id = @id;";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            using var reader = command.ExecuteReader();
            var personIdOrdinal = reader.GetOrdinal("person_id");
            var specialityNameOrdinal = reader.GetOrdinal("speciality_name");

            while (reader.Read())
            {
                specialities.Add(
                    DoctorSpecialityDal.createDoctorSpeciality(reader, personIdOrdinal, specialityNameOrdinal)
                );

            }
            return specialities;
        }

        public List<DoctorSpeciality> getDoctorSpecialitiesBySpecialityName(string specialityName)
        {
            var specialities = new List<DoctorSpeciality>();
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();
            var query = "select * from doctor_speciality where speciality_name = @specialityName;";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@specialityName", MySqlDbType.VarChar).Value = specialityName;
            using var reader = command.ExecuteReader();
            var personIdOrdinal = reader.GetOrdinal("person_id");
            var specialityNameOrdinal = reader.GetOrdinal("speciality_name");
            while (reader.Read())
            {
                specialities.Add(
                    DoctorSpecialityDal.createDoctorSpeciality(reader, personIdOrdinal, specialityNameOrdinal)
                );

            }
            return specialities;
        }
        #endregion

        private static DoctorSpeciality createDoctorSpeciality(MySqlDataReader reader, int personIdOrdinal, int specialityNameOrdinal)
        {
            return new DoctorSpeciality(
                reader.GetFieldValueCheckNull<int>(personIdOrdinal),
                reader.GetFieldValueCheckNull<string>(specialityNameOrdinal)
            );
        }
    }
}
