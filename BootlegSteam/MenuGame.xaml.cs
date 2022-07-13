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
    /// Interaction logic for MenuGame.xaml
    /// </summary>
    public partial class MenuGame : Window
    {
        // Class accesible photo upload path
        private string temppath;
        // Class accesible currently chosen player id
        private long updategameid;
        // Class accesible developer id from combobox
        private long combodevid;

        /// <summary>
        /// WPF Elements initialization
        /// </summary>
        public MenuGame()
        {
            InitializeComponent();
            bindcombo();
            binddevcombo();
        }

        /// <summary>
        /// Updates the combobox with a list of games from database
        /// </summary>
        private void bindcombo()
        {
            steamdbEntities db = new steamdbEntities();
            List<game> lst = db.games.ToList();
            combogames.ItemsSource = lst;
        }

        /// <summary>
        /// Updates the 2nd combobox with a list of developers from database
        /// </summary>
        private void binddevcombo()
        {
            steamdbEntities db = new steamdbEntities();
            List<dev> lst = db.devs.ToList();
            valdev.ItemsSource = lst;
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
        private void uploadgamepic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                steamdbEntities db = new steamdbEntities();

                game images = new game();
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog();
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
                openFileDialog1.DefaultExt = ".jpeg";
                temppath = openFileDialog1.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(temppath));
                valgamepicture.Source = imageSource;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new game in the database with the linked developer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addgame_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(valtitle.Text) && !string.IsNullOrWhiteSpace(valcreation.Text) && valgamepicture.Source != null)
            {
                steamdbEntities db = new steamdbEntities();

                game gobj = new game()
                {
                    title = valtitle.Text,
                    creation = Convert.ToDateTime(valcreation.Text),
                    score = Convert.ToInt64(valscore.Value),
                    devid = this.combodevid,
                    picture = File.ReadAllBytes(this.temppath)
                };
                db.games.Add(gobj);

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
        /// Updates a chosen game from combobox from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updategame_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(valtitle.Text) && !string.IsNullOrWhiteSpace(valcreation.Text) && valgamepicture.Source != null)
            {
                steamdbEntities db = new steamdbEntities();

                var r = from p in db.games
                        where p.id == this.updategameid
                        select p;

                game obj = r.SingleOrDefault();

                if (obj != null)
                {
                    obj.title = valtitle.Text;
                    obj.creation = Convert.ToDateTime(valcreation.Text);
                    obj.score = Convert.ToInt64(valscore.Value);
                    obj.devid = this.combodevid;
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
        /// Deletes a chosen game from combobox from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deletegame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                steamdbEntities db = new steamdbEntities();

                var r = from p in db.games
                        where p.id == this.updategameid
                        select p;

                game obj = r.SingleOrDefault();

                if (obj != null)
                {
                    db.games.Remove(obj);
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
        /// Logic behind adding, updating and deleting based on the user input in the textboxes, datepicker and slider in WPF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combogames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();

            game p = (game)this.combogames.SelectedItem;
            if (p != null)
            {
                valtitle.Text = p.title;
                valcreation.Text = Convert.ToString(p.creation);
                valscore.Value = p.score;
                valdev.Text = p.dev.title;
                this.updategameid = p.id;

                game image = new game();
                var result = (from i in db.games
                              where i.id == this.updategameid
                              select i.picture).FirstOrDefault();

                Stream stream = new MemoryStream(result);
                BitmapImage bitobj = new BitmapImage();
                bitobj.BeginInit();
                bitobj.StreamSource = stream;
                bitobj.EndInit();

                this.valgamepicture.Source = bitobj;
            }

            if (p == null)
            {
                valtitle.Text = null;
                valcreation.Text = null;
                valscore.Value = 1;
                valdev.Text = null;
                this.valgamepicture.Source = null;
            }
        }

        /// <summary>
        /// Logic behind filling the UI with information from the database based on the combobox item selected by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void valdev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();

            dev d = (dev)this.valdev.SelectedItem;
            if (d != null)
            {
                this.combodevid = d.id;

                var r = from p in db.games
                        where p.devid == this.combodevid
                        select new
                        {
                            Score = p.score,
                            Title = p.title,
                            Creation = p.creation
                        };

                foreach (var item in r)
                {
                    Console.WriteLine(item.Score);
                    Console.WriteLine(item.Title);
                    Console.WriteLine(item.Creation);
                }

                this.valdevgames.ItemsSource = r.ToList();

                dev image = new dev();
                var result = (from i in db.devs
                              where i.id == this.combodevid
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
                this.valdevgames.ItemsSource = null;
                this.valdevpicture.Source = null;
            }
        }

        /// <summary>
        /// Datepicker formatting for DD/MM/YYYY
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void valdevgames_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }
    }
}
