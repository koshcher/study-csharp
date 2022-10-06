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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Strategy actStrategy;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumClick(object sender, RoutedEventArgs e)
        {
            List<string> currentActList = new List<string>(currentAct.Text.Split(' '));
            if (currentActList[0] == "0")
            {
                currentAct.Text += ".";
            }
            else if (currentActList.Count > 1 && currentActList[2] == "0")
            {
                currentAct.Text += ".";
            }
            currentAct.Text += ((Button)sender).Content;
        }
        private void DotClick(object sender, RoutedEventArgs e)
        {
            List<string> currentActList = new List<string>(currentAct.Text.Split(' '));
            if (currentActList.Count == 1)
            {
                if(currentActList[0].Contains('.') == false)
                {
                    currentAct.Text += ".";
                }
            } 
            else if (currentActList[2] != "")
            {
                if (currentActList[2].Contains('.') == false)
                {
                    currentAct.Text += ".";
                }
            }
        }

        // Functional
        private void ClearCurrent(object sender, RoutedEventArgs e)
        {
            currentAct.Text = "";
        }
        private void ClearAll(object sender, RoutedEventArgs e)
        {
            allActs.Text = "";
            currentAct.Text = "";
        }
        private void DeleteLastSymbol(object sender, RoutedEventArgs e)
        {
            if (currentAct.Text.Length > 0)
            {
                if(currentAct.Text[currentAct.Text.Length - 1] == ' ')
                {
                    currentAct.Text = currentAct.Text.Remove(currentAct.Text.Length - 3);
                } 
                else
                {
                    currentAct.Text = currentAct.Text.Remove(currentAct.Text.Length - 1);
                }
            }
        }

        // Operations
        private void ChangeOperation(char newOperation)
        {
            int indexOfSpace = currentAct.Text.IndexOf(' ');
            if (indexOfSpace == -1)
            {
                currentAct.Text += " " + newOperation + " ";
            }
            else
            {
                currentAct.Text = currentAct.Text.Replace(currentAct.Text[indexOfSpace + 1], newOperation); // indexOfSpace + 1 = current operation 
            }
        }
        private void DivisionClick(object sender, RoutedEventArgs e)
        {
            ChangeOperation('/');
            actStrategy = new DivisionStrategy { };
        }
        private void MultiplicationClick(object sender, RoutedEventArgs e)
        {
            ChangeOperation('*');
            actStrategy = new MultiplicationStrategy { };
        }
        private void DifferenceClick(object sender, RoutedEventArgs e)
        {
            ChangeOperation('-');
            actStrategy = new DifferenceStrategy { };
        }
        private void SumClick(object sender, RoutedEventArgs e)
        {
            ChangeOperation('+');
            actStrategy = new SumStrategy { };
        }

        private void Count(object sender, RoutedEventArgs e)
        {
            List<string> currentActList = new List<string>(currentAct.Text.Split(' '));
            if(currentActList.Count == 3 && currentActList[currentActList.Count - 1] != "")
            {
                if(allActs.Text == "")
                {
                    allActs.Text += currentAct.Text;
                }
                else
                {
                    allActs.Text += " " + currentActList[1] + " " + currentActList[2];
                }
                
                currentAct.Text = actStrategy.Count(Convert.ToDouble(currentActList[0]), Convert.ToDouble(currentActList[2])).ToString();
            }

        }
    }
}
