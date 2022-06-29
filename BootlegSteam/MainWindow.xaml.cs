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

namespace BootlegSteam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            steamdbEntities db = new steamdbEntities();
            var play = from p in db.players
                       select new
                       {
                           PlayerName = p.title,
                           CreationDate = p.creation
                       };
            
            foreach (var item in play)
            {
                Console.WriteLine(item.PlayerName);
                Console.WriteLine(item.CreationDate);
            }

            this.gridPlayers.ItemsSource = play.ToList();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();

            player playerObject = new player()
            {
                title = txtName.Text,
                creation = Convert.ToDateTime(txtCrea.Text),
                iconid = Convert.ToInt64(txtIcon.Text),
                statid = Convert.ToInt64(txtStat.Text)
            };

            db.players.Add(playerObject);
            db.SaveChanges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            steamdbEntities db = new steamdbEntities();

            this.gridPlayers.ItemsSource = db.players.ToList();
        }
    }
}
