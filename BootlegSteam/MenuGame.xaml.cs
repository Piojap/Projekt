using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Forms;

namespace BootlegSteam
{
    /// <summary>
    /// Interaction logic for MenuGame.xaml
    /// </summary>
    public partial class MenuGame : Window
    {
        private string temppath;
        private long updategameid;
        private long combodevid;

        public MenuGame()
        {
            InitializeComponent();
            bindcombo();
            binddevcombo();
        }

        private void bindcombo()
        {
            steamdbEntities db = new steamdbEntities();
            List<game> lst = db.games.ToList();
            combogames.ItemsSource = lst;
        }

        private void binddevcombo()
        {
            steamdbEntities db = new steamdbEntities();
            List<dev> lst = db.devs.ToList();
            valdev.ItemsSource = lst;
        }

        private void clkclose(object sender, RoutedEventArgs e)
        {
            Close();
            App.Current.Shutdown();
        }

        private void clkmaximize(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
        }

        private void clkminimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void dragwindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void clkback(object sender, RoutedEventArgs e)
        {
            MenuMain open = new MenuMain();
            open.Visibility = Visibility.Visible;
            Close();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            bindcombo();
        }

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

        private void addgame_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void updategame_Click(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

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
                            Title = p.title,
                            Score = p.score
                        };

                foreach (var item in r)
                {
                    Console.WriteLine(item.Title);
                    Console.WriteLine(item.Score);
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
    }
}
