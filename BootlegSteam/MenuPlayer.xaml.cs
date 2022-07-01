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
using System.Data.SqlClient;

namespace BootlegSteam
{
    /// <summary>
    /// Interaction logic for MenuPlayer.xaml
    /// </summary>
    public partial class MenuPlayer : Window
    {
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

        private void addplayer_Click(object sender, RoutedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();
            stat sobj = new stat()
            {
                timespent = Convert.ToInt64(valtime.Text),
                perfectgame = Convert.ToInt64(valperfect.Text),
                acclevel = Convert.ToInt64(vallevel.Value)
            };
            db.stats.Add(sobj);

            List<stat> statlst = db.stats.ToList();
            var laststatlst = statlst.Last();

            List<icon> iconlst = db.icons.ToList();
            var lasticonlst = statlst.First();

            player pobj = new player()
            {
                title = valtitle.Text,
                creation = Convert.ToDateTime(valcreation.Text),
                statid = laststatlst.id,
                iconid = lasticonlst.id
            };
            db.players.Add(pobj);

            db.SaveChanges();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            bindcombo();
        }

        private long updateplayerid;

        private void updateplayer_Click(object sender, RoutedEventArgs e)
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
            }
            db.SaveChanges();
        }

        private void comboplayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            player p = (player)this.comboplayers.SelectedItem;
            if (p != null)
            {
                valtitle.Text = p.title;
                valcreation.Text = Convert.ToString(p.creation);
                valtime.Text = Convert.ToString(p.stat.timespent);
                valperfect.Text = Convert.ToString(p.stat.perfectgame);
                vallevel.Value = p.stat.acclevel;
                this.updateplayerid = p.id;
            }
        }
    }
}
