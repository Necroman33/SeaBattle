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
        public int[,] myMap = new int[10, 10];
        public int[,] enemyMap = new int[10, 10];
        public bool isPlaying = false;
        public Button[,] myButtons = new Button[10, 10];
        public Button[,] enemyButtons = new Button[10, 10];
        public Bot bot;
        public Game(string user)
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    myMap[i, j] = 0;
                    enemyMap[i, j] = 0;
                }
            }
            username_label.Content += user;
            addEventClicCell(playerShips,1);
            addEventClicCell(enemyShips,2);
            isPlaying = false;
            bot = new Bot(enemyMap, myMap, enemyButtons, myButtons);
            enemyMap = bot.ConfigureShips();

        }


        private void addEventClicCell(Grid grid, int type)
        {
            int i = 0;
            int j = 0;
            foreach (Grid grid_item in grid.Children)
            {
                if (j >= 10)
                {
                    i++;
                    j = 0;
                }
                Button b = new Button();
                if(type == 1)
                {
                    b.Click += B_Click;
                }
                else
                {
                    b.Click += PlayerShoot;
                }
                b.Tag = new Point(i, j);
                grid_item.Children.Add(b);
                if(type == 1)
                {
                    myButtons[i, j] = b;
                }
                else
                {
                    enemyButtons[i, j] = b;
                }
                j++;
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Point point = (Point)button.Tag;
            if (!isPlaying)
            {
                if (myMap[(int)point.X, (int)point.Y] == 0)
                {
                    button.Background = Brushes.Green;
                    myMap[(int)point.X, (int)point.Y] = 1;
                }
                else
                {
                    button.Background = null;
                    myMap[(int)point.X, (int)point.Y] = 0;
                }
            }
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

        public int CheckIfMapIsNotEmpty()
        {
            int result = 0;
            int test = 0;
            int test2 = 0;
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    if (myMap[i, j] != 0&& myMap[i, j] != 3)
                        test = 1;
                    if (enemyMap[i, j] != 0 && enemyMap[i, j] != 3)
                        test2 = 1;
                }
            }
            if(test == 0)
            {
                result = 1;
            }
            else
            {
                if (test2 == 0)
                {
                    result = 2;
                }
            }
            return result;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = true;
        }

        public void restart()
        {
            isPlaying = false;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {

                    myMap[i, j] = 0;
                    enemyMap[i, j] = 0;
                }
            }
            addEventClicCell(playerShips, 1);
            addEventClicCell(enemyShips, 2);
            enemyMap = bot.ConfigureShips();

        }

        public void PlayerShoot(object sender, EventArgs e)
        {
            var scores = context.Scores.ToList();
            var user_scores = scores.Where(u => u.Username == (string)username_label.Content).FirstOrDefault();
            if (isPlaying)
            {
                Button pressedButton = sender as Button;
                bool playerTurn = Shoot(enemyMap, pressedButton);
                if (!playerTurn)
                    bot.Shoot();
                if (CheckIfMapIsNotEmpty() == 1)
                {
                    MessageBox.Show("Вы проиграли!");
                    restart();
                    if ((string)username_label.Content != "Неизвестный")
                    {
                        user_scores.Defeats++;
                        context.SaveChanges();
                    }
                }
                else
                {
                    if (CheckIfMapIsNotEmpty() == 2)
                    {
                        MessageBox.Show("Вы победили!");
                        restart();
                        if ((string)username_label.Content != "Неизвестный")
                        {
                            user_scores.Wins++;
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
      /* public void Destroyed(Button b)
        {
            Button button = b;
            Point point = (Point)button.Tag;
            int status = 0;
            if (enemyMap[])
            {

            }
            enemyButtons[(int)point.X-1,(int)point.Y-1].Background = Brushes.Red
        }*/
       
        public bool Shoot(int[,] map, Button pressedButton)
        {
            Point point = (Point)pressedButton.Tag;
            bool hit = false;
            if (isPlaying)
            {
                if (map[(int)point.X, (int)point.Y] != 0)
                {
                    hit = true;
                    map[(int)point.X, (int)point.Y] = 3;
                    pressedButton.Background = Brushes.Red;
                }
                else
                {
                    hit = false;

                    pressedButton.Background = Brushes.Blue;
                }
            }
            return hit;
        }

        private void give_up_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying) { 
                var scores = context.Scores.ToList();
                var user_scores = scores.Where(u => u.Username == (string)username_label.Content).FirstOrDefault();
                MessageBox.Show("Вы проиграли!");
                restart();
            if ((string)username_label.Content != "Неизвестный")
            {
                user_scores.Defeats++;
                context.SaveChanges();
            }
            }
        }
    }
}
