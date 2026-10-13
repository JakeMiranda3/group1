using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace System_Information_Group_1.View.Dialogs
{
    /// <summary>
    /// Interaction logic for ReasonDialog.xaml
    /// </summary>
    public partial class ReasonDialog : Window
    {

        /// <summary>
        /// Gets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReasonDialog"/> class.
        /// </summary>
        public ReasonDialog()
        {
            this.InitializeComponent();
            this.Reason = "";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.reasonTextBox.Text))
            {
                MessageBox.Show("Please enter a valid reason.");
                return;
            }

            this.Reason = this.reasonTextBox.Text.Trim();

            DialogResult = true;
        }
    }
}
