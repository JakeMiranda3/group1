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
    public class NurseDal
    {
        #region Access Methods
        public Nurse getNurseWithPersonId(int id)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();
            var query = "select * from nurse where person_id = @id;";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            using var reader = command.ExecuteReader();
            var nurseIdOrdinal = reader.GetOrdinal("nurse_id");
            var personIdOrdinal = reader.GetOrdinal("person_id");
          
            // this should only return one nurse since this ID is a primary key
            return NurseDal.createNurse(reader, nurseIdOrdinal, personIdOrdinal);
        }

        public Nurse getNurseWithNurseId(int id) { 
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();
            var query = "select * from nurse where nurse_id = @id;";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
            using var reader = command.ExecuteReader();
            var personIdOrdinal = reader.GetOrdinal("person_id");
            var nurseIdOrdinal = reader.GetOrdinal("nurse_id");

            return NurseDal.createNurse(reader, nurseIdOrdinal, personIdOrdinal);
        }

        private static Nurse createNurse(MySqlDataReader reader, int nurseIdOrdinal, int personIdOrdinal)
        {
            return new Nurse(
                reader.GetFieldValueCheckNull<int>(personIdOrdinal),
                reader.GetFieldValueCheckNull<int>(nurseIdOrdinal)
            );
        }
        #endregion
    }
}
