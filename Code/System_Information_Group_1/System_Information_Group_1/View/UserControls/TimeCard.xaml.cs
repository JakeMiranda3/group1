using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace System_Information_Group_1.View.UserControls
{
    /// <summary>
    /// Interaction logic for TimeCard.xaml
    /// </summary>
    public partial class TimeCard : UserControl
    {
        private const String DefaultEmptyMessage = "Available";

        /// <summary>
        /// Gets the appointment identifier.
        /// </summary>
        /// <value>
        /// The appointment identifier, or <c>null</c> if this card is an open time slot.
        /// </value>
        public int? AppointmentId { get; private set; }

        /// <summary>
        /// Gets or sets the state of the card, which controls how it is displayed.
        /// </summary>
        /// <value>
        /// The card state.
        /// </value>
        public TimeCardState State
        {
            get => (TimeCardState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="State"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            name: nameof(State),
            propertyType: typeof(TimeCardState),
            ownerType: typeof(TimeCard),
            new PropertyMetadata(TimeCardState.BeforeDeadline));

        /// <summary>
        /// Gets the scheduled time.
        /// </summary>
        /// <value>
        /// The scheduled time.
        /// </value>
        public DateTime ScheduledTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCard"/> class.
        /// </summary>
        public TimeCard()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCard"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="doctorName">Name of the doctor.</param>
        public TimeCard(DateTime date, string doctorName) : this()
        {
            this.doctorLabel.Content = doctorName;
            this.dateLabel.Content = date.ToString("D", CultureInfo.CurrentCulture);
            this.timeLabel.Content = date.ToString("t", CultureInfo.CurrentCulture);
            this.patientNameLabel.Content = DefaultEmptyMessage;
            this.ScheduledTime = date;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCard"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="doctorName">Name of the doctor.</param>
        /// <param name="patientName">Name of the patient.</param>
        /// <param name="patientPhone">The patient phone.</param>
        public TimeCard(DateTime date, string doctorName, string patientName, string patientPhone) : this(date, doctorName)
        {
            this.patientNameLabel.Content = patientName;
            this.patientPhoneLabel.Content = patientPhone;
            this.ScheduledTime = date;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeCard"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="doctorName">Name of the doctor.</param>
        /// <param name="patientName">Name of the patient.</param>
        /// <param name="patientPhone">The patient phone.</param>
        /// <param name="appointmentId">The appointment identifier.</param>
        public TimeCard(DateTime date, string doctorName, string patientName, string patientPhone, int appointmentId) :
            this(date, doctorName, patientName, patientPhone)
        {
            this.AppointmentId = appointmentId;
        }

        
    }
}
