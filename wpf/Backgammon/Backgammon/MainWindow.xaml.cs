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

namespace Backgammon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public List<Grid> boards = new List<Grid> { };
        static public List<Player> players = new List<Player> { };
        static public  List<Button> dices = new List<Button> { };
        static public int currentPlayer;

        Random rnd = new Random();
        int movesCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            //Initialize boards
            boards.Add(leftBoard); // 0
            boards.Add(rightBoard); // 1

            players.Add(new Player(Brushes.White)); // 0
            players.Add(new Player(Brushes.Black)); // 1

            dices.Add(firstDice);
            dices.Add(secondDice);
        }
        private void Start(object sender, RoutedEventArgs e)
        {
            Restart();
        }
        void Restart()
        {
            for(int i = 0; i < players.Count; i++)
            {
                players[i].Restart();
            }
            currentPlayer = rnd.Next(2);
            firstDice.Visibility = Visibility.Visible;
            secondDice.Visibility = Visibility.Visible;
            MoveToNextPlayer();
            ThrowDices();
        }
        void ThrowDices()
        {
            int first = rnd.Next(1, 7);
            int second = rnd.Next(1, 7);
            movesCount = first == second ? 4 : 2;
            firstDice.Content = first;
            secondDice.Content = second;
            if (!players[currentPlayer].CanMove())
            {
                MessageBox.Show("You can not make a move, next player move");
                MoveToNextPlayer();
            }
        }
        void MoveToNextPlayer()
        {
            players[currentPlayer].isStackChipTaken = false;

            if(currentPlayer == 0)
            {
                currentPlayer = 1;

                if(!leftBoard.Children.Contains(firstDice))
                {
                    rightBoard.Children.Remove(firstDice);
                    rightBoard.Children.Remove(secondDice);

                    leftBoard.Children.Add(firstDice);
                    leftBoard.Children.Add(secondDice);
                }

                firstDice.Background = Brushes.Black;
                secondDice.Background = Brushes.Black;
                firstDice.Foreground = Brushes.White;
                secondDice.Foreground = Brushes.White;
            } 
            else
            {
                currentPlayer = 0;

                if(!rightBoard.Children.Contains(firstDice))
                {
                    leftBoard.Children.Remove(firstDice);
                    leftBoard.Children.Remove(secondDice);

                    rightBoard.Children.Add(firstDice);
                    rightBoard.Children.Add(secondDice);
                }

                firstDice.Background = Brushes.White;
                secondDice.Background = Brushes.White;
                firstDice.Foreground = Brushes.Black;
                secondDice.Foreground = Brushes.Black;
            }

            firstDice.IsEnabled = true;
            firstDice.Visibility = Visibility.Visible;
            secondDice.IsEnabled = true;
            secondDice.Visibility = Visibility.Visible;

            ThrowDices();
        }
        private void Move(object sender, RoutedEventArgs e)
        {
            if (players[currentPlayer].Move(Convert.ToInt32(((Button)sender).Content)))
            {
                firstDice.IsEnabled = true;
                secondDice.IsEnabled = true;
                if(movesCount <= 2)
                {
                    ((Button)sender).Visibility = Visibility.Hidden;
                }

                if (players[currentPlayer].IsEnd())
                {
                    if(players[currentPlayer].color == Brushes.Black)
                    {
                        MessageBox.Show("Black is winner");
                    } 
                    else
                    {
                        MessageBox.Show("White is winner");
                    }
                    Restart();
                    return;
                }

                movesCount--;

                if (dices[0].Visibility == Visibility.Hidden && dices[1].Visibility == Visibility.Hidden)
                {
                    MoveToNextPlayer();
                }

                if (!players[currentPlayer].CanMove())
                {
                    MessageBox.Show("You can not make a move, next player move");
                    MoveToNextPlayer();
                }
            }
        }
    }
}
