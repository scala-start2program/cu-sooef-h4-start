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
using Scala.ClassesPart2.Core;

namespace Scala.ClassesPart2.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PersoonService persoonService;
        bool isNieuw;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartSituatie();
            ClearControls();
            persoonService = new PersoonService();
            VulListbox();
        }
        private void StartSituatie()
        {
            grpPersonen.IsEnabled = true;
            grpDetails.IsEnabled = false;
            btnBewaren.Visibility = Visibility.Hidden;
            btnAnnuleren.Visibility = Visibility.Hidden;
        }
        private void BewerkSituatie()
        {
            grpPersonen.IsEnabled = false;
            grpDetails.IsEnabled = true;
            btnBewaren.Visibility = Visibility.Visible;
            btnAnnuleren.Visibility = Visibility.Visible;
        }
        private void ClearControls()
        {
            txtNaam.Text = "";
            txtVoornaam.Text = "";
            dtpGeboortedatum.SelectedDate = null;
            rdbMan.IsChecked = false;
            rdbVrouw.IsChecked = false;
            lblLeeftijd.Content = "";
        }
        private void VulListbox()
        {
            lstPersonen.ItemsSource = null;
            lstPersonen.ItemsSource = persoonService.Personen;
        }
        private void lstPersonen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            if (lstPersonen.SelectedItem == null)
                return;

            Persoon persoon = (Persoon)lstPersonen.SelectedItem;

            txtNaam.Text = ((Persoon)lstPersonen.SelectedItem).Naam;

            txtNaam.Text = persoon.Naam;
            txtVoornaam.Text = persoon.Voornaam;
            dtpGeboortedatum.SelectedDate = persoon.Geboortedatum;
            lblLeeftijd.Content = persoon.GetLeeftijd();
            if (persoon.IsMan)
                rdbMan.IsChecked = true;
            else
                rdbVrouw.IsChecked = true;

        }
        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            isNieuw = true;
            ClearControls();
            BewerkSituatie();
            txtNaam.Focus();
        }
        private void btnWijzig_Click(object sender, RoutedEventArgs e)
        {
            if (lstPersonen.SelectedItem == null)
                return;

            isNieuw = false;
            BewerkSituatie();
            txtNaam.Focus();
        }
        private void btnWis_Click(object sender, RoutedEventArgs e)
        {
            if (lstPersonen.SelectedItem == null)
                return;

            Persoon persoon = (Persoon)lstPersonen.SelectedItem;
            persoonService.VerwijderPersoon(persoon);
            VulListbox();
        }
        private void btnBewaren_Click(object sender, RoutedEventArgs e)
        {
            string naam = txtNaam.Text.Trim();
            if (naam.Length == 0)
            {
                MessageBox.Show("Naam invoeren!", "fout");
                txtNaam.Focus();
                return;
            }
            string voornaam = txtVoornaam.Text.Trim();
            if (voornaam.Length == 0)
            {
                MessageBox.Show("Voornaam invoeren !", "fout");
                txtVoornaam.Focus();
                return;
            }
            if (dtpGeboortedatum.SelectedDate == null)
            {
                MessageBox.Show("Geboortedatum invoeren !", "fout");
                dtpGeboortedatum.Focus();
                return;
            }
            DateTime geboortedatum = (DateTime)dtpGeboortedatum.SelectedDate;
            if (rdbMan.IsChecked == false && rdbVrouw.IsChecked == false)
            {
                MessageBox.Show("Selecteer man of vrouw !", "fout");
                rdbMan.Focus();
                return;
            }
            bool isman = false;
            if (rdbMan.IsChecked == true)
            {
                isman = true;
            }
            Persoon persoon;
            if (isNieuw)
            {
                persoon = new Persoon(naam, voornaam, geboortedatum, isman);
                persoonService.VoegPersoonToe(persoon);
            }
            else
            {
                persoon = (Persoon)lstPersonen.SelectedItem;
                persoon.Naam = naam;
                persoon.Voornaam = voornaam;
                persoon.Geboortedatum = geboortedatum;
                persoon.IsMan = isman;
                persoonService.OrderList();
            }
            StartSituatie();
            VulListbox();
            lstPersonen.SelectedItem = persoon;
            lstPersonen_SelectionChanged(null, null);
        }

        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            StartSituatie();
            ClearControls();
            if (lstPersonen.SelectedIndex > -1)
                lstPersonen_SelectionChanged(null, null);
        }
    }
}
