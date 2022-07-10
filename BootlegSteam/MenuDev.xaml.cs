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
    /// Interaction logic for MenuGenre.xaml
    /// </summary>
    public partial class MenuDev : Window
    {
        private string temppath;
        private long updatedevid;

        public MenuDev()
        {
            InitializeComponent();
            bindcombo();
        }

        private void bindcombo()
        {
            steamdbEntities db = new steamdbEntities();
            List<dev> lst = db.devs.ToList();
            combodevs.ItemsSource = lst;
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

        private void adddev_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void updatedev_Click(object sender, RoutedEventArgs e)
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

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
    }
}
