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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                valgamepicture.Source = new BitmapImage(fileUri);
            }
        }

        private void uploaddevpic_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                valdevpicture.Source = new BitmapImage(fileUri);
            }
        }

        private long updategameid;

        private void addgame_Click(object sender, RoutedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();

            game gobj = new game()
            {
                title = valtitle.Text,
                creation = Convert.ToDateTime(valcreation.Text),
                score = Convert.ToInt64(valscore.Value),
                devid = 1
        };
            db.games.Add(gobj);

            db.SaveChanges();
        }

        private void updategame_Click(object sender, RoutedEventArgs e)
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
                obj.devid = 1;
            }
            db.SaveChanges();
        }

        private void deletegame_Click(object sender, RoutedEventArgs e)
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
            }
        }

        private void combogames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            game p = (game)this.combogames.SelectedItem;
            if (p != null)
            {
                valtitle.Text = p.title;
                valcreation.Text = Convert.ToString(p.creation);
                valscore.Value = p.score;
                valdev.Text = p.dev.title;
                this.updategameid = p.id;
            }
        }

        private void valdev_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
