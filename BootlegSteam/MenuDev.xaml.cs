using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BootlegSteam
{
    /// <summary>
    /// Interaction logic for MenuGenre.xaml
    /// </summary>
    public partial class MenuDev : Window
    {
        // Class accesible photo upload path
        private string temppath;
        // Class accesible currently chosen player id
        private long updatedevid;

        /// <summary>
        /// WPF Elements initialization
        /// </summary>
        public MenuDev()
        {
            InitializeComponent();
            bindcombo();
        }

        /// <summary>
        /// Updates the combobox with a list of players from database
        /// </summary>
        private void bindcombo()
        {
            steamdbEntities db = new steamdbEntities();
            List<dev> lst = db.devs.ToList();
            combodevs.ItemsSource = lst;
        }

        /// <summary>
        /// Logic for closing the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkclose(object sender, RoutedEventArgs e)
        {
            Close();
            App.Current.Shutdown();
        }

        /// <summary>
        /// Logic for maximizing/windowed the application window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkmaximize(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// Logic for minimizing the application window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkminimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Logic allowing dragging the application window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dragwindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// Logic for going back to the Main Menu window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkback(object sender, RoutedEventArgs e)
        {
            MenuMain open = new MenuMain();
            open.Visibility = Visibility.Visible;
            Close();
        }

        /// <summary>
        /// Logic for refreshing the combobox players list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            bindcombo();
        }

        /// <summary>
        /// Updates the WPF image with an image chosen by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploaddevpic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                steamdbEntities db = new steamdbEntities();

                dev images = new dev();
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog();
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
                openFileDialog1.DefaultExt = ".jpeg";
                temppath = openFileDialog1.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(temppath));
                valdevpicture.Source = imageSource;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new developer in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adddev_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(valtitle.Text) && !string.IsNullOrWhiteSpace(valcreation.Text) && !string.IsNullOrWhiteSpace(valcountry.Text) && !string.IsNullOrWhiteSpace(valage.Text) && valdevpicture.Source != null)
            {
                steamdbEntities db = new steamdbEntities();

                dev dobj = new dev()
                {
                    title = valtitle.Text,
                    creation = Convert.ToDateTime(valcreation.Text),
                    country = valcountry.Text,
                    popularity = Convert.ToInt64(valpopularity.Value),
                    age = Convert.ToInt64(valage.Text),
                    picture = File.ReadAllBytes(this.temppath)
                };
                db.devs.Add(dobj);

                db.SaveChanges();
                bindcombo();
            }
            else
            {
                MenuWarning open = new MenuWarning();
                open.Show();
            }
        }

        /// <summary>
        /// Updates a chosen developer from combobox in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatedev_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(valtitle.Text) && !string.IsNullOrWhiteSpace(valcreation.Text) && !string.IsNullOrWhiteSpace(valcountry.Text) && !string.IsNullOrWhiteSpace(valage.Text) && valdevpicture.Source != null)
            {
                steamdbEntities db = new steamdbEntities();

                var r = from d in db.devs
                        where d.id == this.updatedevid
                        select d;

                dev obj = r.SingleOrDefault();

                if (obj != null)
                {
                    obj.title = valtitle.Text;
                    obj.creation = Convert.ToDateTime(valcreation.Text);
                    obj.country = valcountry.Text;
                    obj.popularity = Convert.ToInt64(valpopularity.Value);
                    obj.age = Convert.ToInt64(valage.Text);
                    if (this.temppath != null)
                    {
                        obj.picture = File.ReadAllBytes(this.temppath);
                    }
                }
                db.SaveChanges();
                bindcombo();
            }
            else
            {
                MenuWarning open = new MenuWarning();
                open.Show();
            }
        }

        /// <summary>
        /// Deletes a chosen developer from combobox from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deletedev_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                steamdbEntities db = new steamdbEntities();

                var r = from d in db.devs
                        where d.id == this.updatedevid
                        select d;

                dev obj = r.SingleOrDefault();

                if (obj != null)
                {
                    db.devs.Remove(obj);
                    db.SaveChanges();
                    bindcombo();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Logic behind filling the UI with information from the database based on the combobox item selected by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combodevs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();

            dev d = (dev)this.combodevs.SelectedItem;
            if (d != null)
            {
                valtitle.Text = d.title;
                valcreation.Text = Convert.ToString(d.creation);
                valcountry.Text = d.country;
                valpopularity.Value = d.popularity;
                valage.Text = Convert.ToString(d.age);
                this.updatedevid = d.id;

                dev image = new dev();
                var result = (from i in db.devs
                              where i.id == this.updatedevid
                              select i.picture).FirstOrDefault();

                Stream stream = new MemoryStream(result);
                BitmapImage bitobj = new BitmapImage();
                bitobj.BeginInit();
                bitobj.StreamSource = stream;
                bitobj.EndInit();

                this.valdevpicture.Source = bitobj;
            }

            if (d == null)
            {
                valtitle.Text = null;
                valcreation.Text = null;
                valcountry.Text = null;
                valpopularity.Value = 1;
                valage.Text = null;
                this.valdevpicture.Source = null;
            }
        }

        /// <summary>
        /// Logic that allows only numbers in certain WPF textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acceptNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
