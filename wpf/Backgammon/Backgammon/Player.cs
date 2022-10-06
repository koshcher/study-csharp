using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace Backgammon
{
    public class Player
    {
        public SolidColorBrush color;
        Dictionary<int, List<Ellipse>> chips = new Dictionary<int, List<Ellipse>> { }; // int - position
        int currentPosition = 0;
        public bool isStackChipTaken = false;

        public Player(SolidColorBrush solidColorBrush)
        {
            color = solidColorBrush;
        }
        public void Restart()
        {
            foreach(var pair in chips)
            {
                foreach(var chip in pair.Value)
                {
                    if(MainWindow.boards[0].Children.Contains(chip))
                    {
                        MainWindow.boards[0].Children.Remove(chip);
                    }
                    else if(MainWindow.boards[1].Children.Contains(chip))
                    {
                        MainWindow.boards[1].Children.Remove(chip);
                    }
                }
                pair.Value.Clear();
            }
            chips.Clear();

            for (int i = 0; i < 24; i++)
            {
                chips.Add((i + 1), new List<Ellipse> { });
            }

            for (int i = 0; i < 15; i++)
            {
                Ellipse newChip = new Ellipse();
                newChip.Fill = color;
                newChip.MouseLeftButtonDown += OnEllipseMouseLeftButtonDown;

                UpdatePosition(newChip, 1);
                UpdateMargin(newChip, 1);

                chips[1].Add(newChip);
            }
        }
        public bool Move(int steps)
        {
            if(currentPosition != 0)
            {
                isStackChipTaken = currentPosition == 1 ? true : false;
                int newPosition = currentPosition + steps;
                List<Ellipse> currentPositionChips = chips[currentPosition]; // for economing time

                if (newPosition > 24)
                {
                    if(color == Brushes.Black)
                    {
                        MainWindow.boards[(currentPosition < 7 || currentPosition > 18) ? 0 : 1].Children.Remove(currentPositionChips[currentPositionChips.Count - 1]);
                    }
                    else
                    {
                        MainWindow.boards[(currentPosition < 7 || currentPosition > 18) ? 1 : 0].Children.Remove(currentPositionChips[currentPositionChips.Count - 1]);
                    }
                    currentPositionChips.RemoveAt(currentPositionChips.Count - 1);
                }
                else
                {
                    UpdatePosition(currentPositionChips[currentPositionChips.Count - 1], newPosition);
                    UpdateMargin(currentPositionChips[currentPositionChips.Count - 1], newPosition);
                    currentPositionChips[currentPositionChips.Count - 1].Fill = color;

                    chips[newPosition].Add(currentPositionChips[currentPositionChips.Count - 1]);
                    currentPositionChips.RemoveAt(currentPositionChips.Count - 1);
                }

                currentPosition = 0;
                return true;
            }
            return false;
        }
        void OnEllipseMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(MainWindow.players[MainWindow.currentPlayer].color == color)
            {
                if (currentPosition != 0) { chips[currentPosition][chips[currentPosition].Count - 1].Fill = color; }

                foreach (var pair in chips)
                {
                    if (pair.Value.Contains((Ellipse)sender))
                    {
                        currentPosition = pair.Key;
                        break;
                    }
                }

                if(currentPosition == 1 && isStackChipTaken)
                {
                    currentPosition = 0;
                }
                else
                {
                    chips[currentPosition][chips[currentPosition].Count - 1].Fill = Brushes.LightGreen;

                    CheckMoveAviable();
                }
            }
        }
        public void CheckMoveAviable()
        {
            bool isAllAtHome = IsAllAtHome();
            for (int i = 0; i < MainWindow.dices.Count; i++)
            {
                if (!MainWindow.players[1 - MainWindow.currentPlayer].IsPositionEmpty(ToOponentPosition(currentPosition + Convert.ToInt32(MainWindow.dices[i].Content))))
                {
                    MainWindow.dices[i].IsEnabled = false;
                }
                else
                {
                    MainWindow.dices[i].IsEnabled = true;
                }

                if(!isAllAtHome && currentPosition + Convert.ToInt32(MainWindow.dices[i].Content) > 24)
                {
                    MainWindow.dices[i].IsEnabled = false;
                }
            }
        }
        public bool CanMove()
        {
            foreach(var pair in chips)
            {
                if(pair.Key == 1 && isStackChipTaken == true)
                {
                    continue;
                }
                if(pair.Value.Count > 0)
                {
                    for (int i = 0; i < MainWindow.dices.Count; i++)
                    {
                        if (MainWindow.players[1 - MainWindow.currentPlayer].IsPositionEmpty(ToOponentPosition(pair.Key + Convert.ToInt32(MainWindow.dices[i].Content))))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        bool IsAllAtHome()
        {
            int count = 0;
            for (int i = 1; i < 19; i++)
            {
                count += chips[i].Count;
            }
            return count == 0;
        }
        public bool IsEnd()
        {
            int count = 0;
            for (int i = 1; i < 25; i++)
            {
                count += chips[i].Count;
            }
            return count == 0;
        }
        void UpdatePosition(Ellipse chip, int position)
        {
            if(color == Brushes.Black)
            {
                chip.SetValue(Grid.RowProperty, position < 13 ? 2 : 0); // set row

                // set column
                if (position < 7)
                {
                    chip.SetValue(Grid.ColumnProperty, position - 1);
                }
                else if (position < 13)
                {
                    chip.SetValue(Grid.ColumnProperty, position - 7);
                }
                else if (position < 19)
                {
                    chip.SetValue(Grid.ColumnProperty, 18 - position);
                }
                else
                {
                    chip.SetValue(Grid.ColumnProperty, 24 - position);
                }

                // set board
                MainWindow.boards[(currentPosition < 7 || currentPosition > 18) ? 0 : 1].Children.Remove(chip);
                MainWindow.boards[(position < 7 || position > 18) ? 0 : 1].Children.Add(chip);
            }
            else
            {
                chip.SetValue(Grid.RowProperty, position < 13 ? 0 : 2); // set row

                // set column
                if(position < 7)
                {
                    chip.SetValue(Grid.ColumnProperty, 6 - position);
                } 
                else if(position < 13)
                {
                    chip.SetValue(Grid.ColumnProperty, 12 - position);
                } 
                else if (position < 19)
                {
                    chip.SetValue(Grid.ColumnProperty, position - 13);
                }
                else
                {
                    chip.SetValue(Grid.ColumnProperty, position - 19);
                }

                // set board
                MainWindow.boards[(currentPosition < 7 || currentPosition > 18) ? 1 : 0].Children.Remove(chip);
                MainWindow.boards[(position < 7 || position > 18) ? 1 : 0].Children.Add(chip);
            }
        }
        void UpdateMargin(Ellipse chip, int position)
        {
            if(color == Brushes.Black)
            {
                if(position < 13)
                {
                    chip.VerticalAlignment = VerticalAlignment.Bottom;
                    chip.Margin = new Thickness(0, 0, 0, chips[position].Count * 15);
                } 
                else
                {
                    chip.VerticalAlignment = VerticalAlignment.Top;
                    chip.Margin = new Thickness(0, chips[position].Count * 15, 0, 0);
                }
            } 
            else
            {
                if (position < 13)
                {
                    chip.VerticalAlignment = VerticalAlignment.Top;
                    chip.Margin = new Thickness(0, chips[position].Count * 15, 0, 0);
                }
                else
                {
                    chip.VerticalAlignment = VerticalAlignment.Bottom;
                    chip.Margin = new Thickness(0, 0, 0, chips[position].Count * 15);
                }
            }
        }
        public bool IsPositionEmpty(int position)
        {
            return chips[position].Count == 0;
        }
        static int ToOponentPosition(int position)
        {
            if(position < 13)
            {
                return position + 12;
            }
            return position - 12;
        }
    }
}
