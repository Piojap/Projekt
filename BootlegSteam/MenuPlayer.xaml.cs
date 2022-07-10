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
    /// Interaction logic for MenuPlayer.xaml
    /// </summary>
    public partial class MenuPlayer : Window
    {
        private string temppath;
        private long updateplayerid;

        public MenuPlayer()
        {
            InitializeComponent();
            bindcombo();
        }

        private void bindcombo()
        {
            steamdbEntities db = new steamdbEntities();
            List<player> lst = db.players.ToList();
            comboplayers.ItemsSource = lst;
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

                player images = new player();
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog();
                openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
                openFileDialog1.DefaultExt = ".jpeg";
                temppath = openFileDialog1.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(temppath));
                valplayerpicture.Source = imageSource;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void addplayer_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(valtitle.Text) && !string.IsNullOrWhiteSpace(valcreation.Text) && !string.IsNullOrWhiteSpace(valtime.Text) && !string.IsNullOrWhiteSpace(valperfect.Text) && valplayerpicture.Source != null)
            {
                steamdbEntities db = new steamdbEntities();
                stat sobj = new stat()
                {
                    timespent = Convert.ToInt64(valtime.Text),
                    perfectgame = Convert.ToInt64(valperfect.Text),
                    acclevel = Convert.ToInt64(vallevel.Value)
                };
                db.stats.Add(sobj);

                db.SaveChanges();
                steamdbEntities db2 = new steamdbEntities();

                List<stat> statlst = db2.stats.ToList();
                var laststatlst = statlst.Last();

                player pobj = new player()
                {
                    title = valtitle.Text,
                    creation = Convert.ToDateTime(valcreation.Text),
                    picture = File.ReadAllBytes(this.temppath),
                    statid = laststatlst.id,
                };
                db2.players.Add(pobj);

                db2.SaveChanges();
                bindcombo();
            }
            else
            {
                MenuWarning open = new MenuWarning();
                open.Show();
            }
        }

        private void updateplayer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(valtitle.Text) && !string.IsNullOrWhiteSpace(valcreation.Text) && !string.IsNullOrWhiteSpace(valtime.Text) && !string.IsNullOrWhiteSpace(valperfect.Text) && valplayerpicture.Source != null)
            {
                steamdbEntities db = new steamdbEntities();

                var r = from p in db.players
                        where p.id == this.updateplayerid
                        select p;

                player obj = r.SingleOrDefault();

                if (obj != null)
                {
                    obj.title = valtitle.Text;
                    obj.creation = Convert.ToDateTime(valcreation.Text);
                    obj.stat.timespent = Convert.ToInt64(valtime.Text);
                    obj.stat.perfectgame = Convert.ToInt64(valperfect.Text);
                    obj.stat.acclevel = Convert.ToInt64(vallevel.Value);
                    if (this.temppath != null)
                    {
                        obj.picture = File.ReadAllBytes(this.temppath);
                    }
                    db.SaveChanges();
                }
                bindcombo();
            }
            else
            {
                MenuWarning open = new MenuWarning();
                open.Show();
            }
        }

        private void deleteplayer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                steamdbEntities db = new steamdbEntities();

                var r = from p in db.players
                        where p.id == this.updateplayerid
                        select p;

                player obj = r.SingleOrDefault();

                if (obj != null)
                {
                    db.players.Remove(obj);
                    db.SaveChanges();
                }

                var t = from s in db.stats
                        where s.id == obj.statid
                        select s;

                stat obj2 = t.SingleOrDefault();

                if (obj2 != null)
                {
                    db.stats.Remove(obj2);
                    db.SaveChanges();
                }
                bindcombo();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void comboplayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();

            player p = (player)this.comboplayers.SelectedItem;
            if (p != null)
            {
                valtitle.Text = p.title;
                valcreation.Text = Convert.ToString(p.creation);
                valtime.Text = Convert.ToString(p.stat.timespent);
                valperfect.Text = Convert.ToString(p.stat.perfectgame);
                vallevel.Value = p.stat.acclevel;
                this.updateplayerid = p.id;

                player image = new player();
                var result = (from i in db.players
                              where i.id == this.updateplayerid
                              select i.picture).FirstOrDefault();

                Stream stream = new MemoryStream(result);
                BitmapImage bitobj = new BitmapImage();
                bitobj.BeginInit();
                bitobj.StreamSource = stream;
                bitobj.EndInit();

                this.valplayerpicture.Source = bitobj;
            }

            if (p == null)
            {
                valtitle.Text = null;
                valcreation.Text = null;
                valtime.Text = null;
                valperfect.Text = null;
                vallevel.Value = 1;
                this.valplayerpicture.Source = null;
            }
        }
    }
}
