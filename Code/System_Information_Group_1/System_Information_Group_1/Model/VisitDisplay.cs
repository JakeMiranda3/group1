using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System_Information_Group_1.Model
{
    public class VisitDisplay : Visit, INotifyPropertyChanged
    {
        public VisitDisplay(int appointmentId, string symptoms, int nurseId, int systolicBloodPressure, int diastolicBloodPressure, double temperature, int pulse, int height, int weight, string initialDiagnosis, string finalDiagnosis) : base(appointmentId, symptoms, nurseId, systolicBloodPressure, diastolicBloodPressure, temperature, pulse, height, weight, initialDiagnosis, finalDiagnosis)
        {
        }

        public VisitDisplay(int appointmentId, int nurseId) : base(appointmentId, nurseId)
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
