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
using System.Diagnostics;

namespace SeaBattle
{
    /// <summary>
    /// Логика взаимодействия для Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        SeaBattleEntities context = new SeaBattleEntities();
        Logic logic = new Logic();
        public Game(string user)
        {
            InitializeComponent();
            username_label.Content = user;
            addEventClicCell(playerShips);
        }


        private void addEventClicCell(Grid grid)
        {
            int i = 0;
            int j = 0;
            foreach (Grid grid_item in grid.Children)
            {
                if( j>= 10)
                {
                    i++;
                    j = 0;
                }
                Button b = new Button();
                b.Click += B_Click;
                b.Tag = new Point(i, j);
                grid_item.Children.Add(b);
                j++;
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Point point = (Point)button.Tag;
            Debug.WriteLine(point);
        }

        private void To_main_btn_Click(object sender, RoutedEventArgs e)
        {
            string user = username_label.Content.ToString();
            if (user != "Неизвестный")
            {
                User_menu use = new User_menu(user);
                this.Hide();
                use.ShowDialog();
            }
            else
            {
                MainWindow mainw = new MainWindow();
                this.Hide();
                mainw.ShowDialog();
            }
        }

    }
}
