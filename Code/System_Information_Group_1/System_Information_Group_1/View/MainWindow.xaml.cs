using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Constructors    
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private ObservableCollection<Person> people = new ObservableCollection<Person>();
    private readonly PersonDal personDal = new PersonDal();
    private readonly NurseDal nurseDal = new NurseDal();
    private readonly DoctorDal doctorDal = new DoctorDal();
    private readonly AdministratorDal administratorDal = new AdministratorDal();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        RefreshPersons();
    }

    private void RefreshPersons()
    {
        people.Clear();
        var list = personDal.GetAllPersons();
        foreach (var p in list)
        {
            var roles = personDal.GetPersonRoles(p.PersonId);
            p.Roles = roles.Length == 0 ? string.Empty : string.Join(", ", roles);
            people.Add(p);
        }

        PersonsGrid.ItemsSource = people;
    }


    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
        if (!DateTime.TryParse(DobBox.Text, out var dob))
        {
            MessageBox.Show("Invalid date of birth format. Use yyyy-MM-dd");
            return;
        }

        var zip = (ZipBox.Text ?? string.Empty).Trim();
        if (!zip.All(char.IsDigit))
        {
            MessageBox.Show("Zip code must contain only digits.");
            return;
        }

        var phone = (PhoneBox.Text ?? string.Empty).Trim();
        if (!phone.All(char.IsDigit))
        {
            MessageBox.Show("Phone number must contain only digits");
            return;
        }
        if (phone.Length != 10)
        {
            MessageBox.Show("Phone number must be 10 digits long.");
            return;
        }

        var created = personDal.CreatePerson(LastNameBox.Text, FirstNameBox.Text, dob, phone, AddressBox.Text, zip, CityBox.Text, StateBox.Text);

        var role = (RoleCombo.SelectedItem as ComboBoxItem)?.Content?.ToString();
        if (!string.IsNullOrEmpty(role))
        {
            try
            {
                switch (role)
                {
                    case "Nurse":
                        nurseDal.CreateNurse(created.PersonId);
                        break;
                    case "Doctor":
                        doctorDal.CreateDoctor(created.PersonId);
                        break;
                    case "Administrator":
                        administratorDal.CreateAdministrator(created.PersonId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Person created but failed to assign role '{role}': {ex.Message}");
            }
        }

        RefreshPersons();
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO imlement update functionality
        MessageBox.Show("Update is not implemented yet.");
    }

    #endregion

}