using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace System_Information_Group_1.View.UserControls
{
    /// <summary>
    /// Interaction logic for TimeCard.xaml
    /// </summary>
    public partial class TimeCard : UserControl
    {
        private const String DefaultEmptyMessage = "Available";

        public TimeCardState State
        {
            get => (TimeCardState)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            name: nameof(State),
            propertyType: typeof(TimeCardState),
            ownerType: typeof(TimeCard),
            new PropertyMetadata(TimeCardState.Open));

        /// <summary>
        /// Gets the scheduled time.
        /// </summary>
        /// <value>
        /// The scheduled time.
        /// </value>
        public DateTime scheduledTime { get; private set; }

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
            this.scheduledTime = date;
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
            this.scheduledTime = date;
        }
    }
}
