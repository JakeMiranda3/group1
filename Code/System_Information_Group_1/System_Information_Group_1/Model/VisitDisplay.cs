using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System_Information_Group_1.Model
{
    /// <summary>
    /// A visit that notifies the view when its properties change, used for display.
    /// </summary>
    public class VisitDisplay : Visit, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisitDisplay"/> class.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="symptoms">The symptoms.</param>
        /// <param name="nurseId">The nurse identifier.</param>
        /// <param name="systolicBloodPressure">The systolic blood pressure.</param>
        /// <param name="diastolicBloodPressure">The diastolic blood pressure.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="pulse">The pulse.</param>
        /// <param name="height">The height.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="initialDiagnosis">The initial diagnosis.</param>
        /// <param name="finalDiagnosis">The final diagnosis.</param>
        /// <param name="finalDiagnosisEnteredDatetime">The time the final diagnosis was entered.</param>
        public VisitDisplay(int appointmentId, string symptoms, int nurseId, int systolicBloodPressure, int diastolicBloodPressure, double temperature, int pulse, double height, double weight, string initialDiagnosis, string finalDiagnosis, DateTime? finalDiagnosisEnteredDatetime) : base(appointmentId, symptoms, nurseId, systolicBloodPressure, diastolicBloodPressure, temperature, pulse, height, weight, initialDiagnosis, finalDiagnosis, finalDiagnosisEnteredDatetime)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitDisplay"/> class with only an appointment and nurse.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <param name="nurseId">The nurse identifier.</param>
        public VisitDisplay(int appointmentId, int nurseId) : base(appointmentId, nurseId)
        {

        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field to the given value and raises <see cref="PropertyChanged"/> if the value changed.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="field">The backing field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">Name of the property being set.</param>
        /// <returns><c>true</c> if the value changed; otherwise, <c>false</c>.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
