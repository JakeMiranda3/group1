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
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for RoutineCheckupWindow.xaml
    /// </summary>
    public partial class RoutineCheckupWindow : Window
    {
        private RoutineCheckupController controller;
        public RoutineCheckupWindow(VisitDisplay visitDisplay)
        {

            InitializeComponent();
            this.controller = new RoutineCheckupController(visitDisplay);
            this.DataContext = this.controller.Visit;

        }
    }
}
