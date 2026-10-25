namespace System_Information_Group_1.Model;

/// <summary>
///     This class represents a visit.
///     @author Colby
///     @version Fall 2026
/// </summary>
public class Visit
{
    #region Properties

    /// <summary>
    /// Gets the appointment identifier.
    /// </summary>
    /// <value>
    /// The appointment identifier.
    /// </value>
    public int AppointmentId { get; private set; }
    /// <summary>
    /// Gets or sets the symptoms.
    /// </summary>
    /// <value>
    /// The symptoms.
    /// </value>
    public string Symptoms { get; set; }
    /// <summary>
    /// Gets or sets the nurse identifier.
    /// </summary>
    /// <value>
    /// The nurse identifier.
    /// </value>
    public int NurseId { get; set; }
    public int SystolicBloodPressure { get; set; }
    /// <summary>
    /// Gets or sets the diastolic blood pressure.
    /// </summary>
    /// <value>
    /// The diastolic blood pressure.
    /// </value>
    public int DiastolicBloodPressure { get; set; }
    /// <summary>
    /// Gets or sets the temperature.
    /// </summary>
    /// <value>
    /// The temperature.
    /// </value>
    public double Temperature { get; set; }
    /// <summary>
    /// Gets or sets the pulse.
    /// </summary>
    /// <value>
    /// The pulse.
    /// </value>
    public int Pulse { get; set; }
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public int Height { get; set; }
    /// <summary>
    /// Gets or sets the weight.
    /// </summary>
    /// <value>
    /// The weight.
    /// </value>
    public int Weight { get; set; }
    /// <summary>
    /// Gets or sets the initial diagnosis.
    /// </summary>
    /// <value>
    /// The initial diagnosis.
    /// </value>
    public string InitialDiagnosis { get; set; }
    /// <summary>
    /// Gets or sets the final diagnosis.
    /// </summary>
    /// <value>
    /// The final diagnosis.
    /// </value>
    public string FinalDiagnosis { get; set; }

    #endregion

    #region Constructors    
    /// <summary>
    /// Initializes a new instance of the <see cref="Visit"/> class.
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
    public Visit(int appointmentId, string symptoms, int nurseId, int systolicBloodPressure,
        int diastolicBloodPressure, double temperature, int pulse, int height, int weight, string initialDiagnosis,
        string finalDiagnosis)
    {
        this.AppointmentId = appointmentId;
        this.Symptoms = symptoms;
        this.NurseId = nurseId;
        this.SystolicBloodPressure = systolicBloodPressure;
        this.DiastolicBloodPressure = diastolicBloodPressure;
        this.Temperature = temperature;
        this.Pulse = pulse;
        this.Height = height;
        this.Weight = weight;
        this.InitialDiagnosis = initialDiagnosis;
        this.FinalDiagnosis = finalDiagnosis;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Visit"/> class.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <param name="nurseId">The nurse identifier.</param>
    public Visit(int appointmentId, int nurseId)
    {
        this.AppointmentId = appointmentId;
        this.NurseId = nurseId;
    }

    #endregion
}