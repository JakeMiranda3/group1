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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace System_Information_Group_1.View.UserControls
{
    /// <summary>
    /// Interaction logic for TopAppBar.xaml
    /// </summary>
    public partial class TopAppBar : UserControl
    {
        public TopAppBar()
        {
            this.InitializeComponent();
        }

        public String AppBarTitle
        {
            get => this.barTitle.Text;
            set => this.barTitle.Text = value;
        }

        public void setUsernameAndRole(String role, String username)
        {
            this.roleAndUsernameTextblock.Text = $"{role}: {username}";
        }
    }
}
