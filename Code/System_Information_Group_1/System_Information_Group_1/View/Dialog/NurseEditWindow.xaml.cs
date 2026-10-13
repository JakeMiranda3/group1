using System;
using System.Windows;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View
{
    public partial class NurseEditWindow : Window
    {
        public NurseEditWindow(NurseDisplay nurse)
        {
            InitializeComponent();
            this.DataContext = nurse;
        }

        public NurseDisplay EditedNurse
        {
            get { return (NurseDisplay)this.DataContext; }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add validation here 
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}