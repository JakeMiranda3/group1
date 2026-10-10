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
        public TimeCard()
        {
            InitializeComponent();
        }

        public TimeCard(DateTime date, String doctorName) : this()
        {
            this.doctorLabel.Content = doctorName;
            this.dateLabel.Content = date.ToString("D", CultureInfo.CurrentCulture);
            this.timeLabel.Content = date.ToString("t", CultureInfo.CurrentCulture);
            this.patientNameLabel.Content = DefaultEmptyMessage;
        }
        public TimeCard(DateTime date, String doctorName, String patientName, String patientPhone) : this(date, doctorName)
        {
            this.patientNameLabel.Content = patientName;
            this.patientPhoneLabel.Content = patientPhone;
        }
    }
}
