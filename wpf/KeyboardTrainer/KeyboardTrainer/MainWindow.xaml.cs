using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;


namespace KeyboardTrainer
{
    internal sealed partial class MainWindow : Window
    {
        Generator generator = new Generator();
        Test test = new Test();

        // for showing click
        Brush previousColor;
        Border previousBorder = new Border();
        Border currentBorder;


        // keys for input
        Dictionary<Key, char> keyCharLower = new Dictionary<Key, char> 
        {
            {Key.Oem3, '`'},{Key.D1, '1'},{Key.D2, '2'},{Key.D3, '3'},{Key.D4, '4'},{Key.D5, '5'},{Key.D6, '6'},{Key.D7, '7'},{Key.D8, '8'},{Key.D9, '9'},{Key.D0, '0'},{Key.OemMinus, '-'},{Key.OemPlus, '='},
            {Key.Q, 'q'},{Key.W, 'w'},{Key.E, 'e'},{Key.R, 'r'},{Key.T, 't'},{Key.Y, 'y'},{Key.U, 'u'},{Key.I, 'i'},{Key.O, 'o'},{Key.P, 'p'},{Key.OemOpenBrackets, '['},{Key.OemCloseBrackets, ']'},{Key.Oem5, '\\'},
            {Key.A, 'a'},{Key.S, 's'},{Key.D, 'd'},{Key.F, 'f'},{Key.G, 'g'},{Key.H, 'h'},{Key.J, 'j'},{Key.K, 'k'},{Key.L, 'l'},{Key.OemSemicolon, ';'},{Key.OemQuotes, '\''},
            {Key.Z, 'z'},{Key.X, 'x'},{Key.C, 'c'},{Key.V, 'v'},{Key.B, 'b'},{Key.N, 'n'},{Key.M, 'm'},{Key.OemComma, ','},{Key.OemPeriod, '.'},{Key.OemQuestion, '/'}
        };
        Dictionary<Key, char> keyCharUpper = new Dictionary<Key, char>
        {
            {Key.Oem3, '~'},{Key.D1, '!'},{Key.D2, '@'},{Key.D3, '#'},{Key.D4, '$'},{Key.D5, '%'},{Key.D6, '^'},{Key.D7, '&'},{Key.D8, '*'},{Key.D9, '('},{Key.D0, ')'},{Key.OemMinus, '_'},{Key.OemPlus, '+'},
            {Key.Q, 'Q'},{Key.W, 'W'},{Key.E, 'E'},{Key.R, 'R'},{Key.T, 'T'},{Key.Y, 'Y'},{Key.U, 'U'},{Key.I, 'I'},{Key.O, 'O'},{Key.P, 'P'},{Key.OemOpenBrackets, '{'},{Key.OemCloseBrackets, '}'},{Key.Oem5, '|'},
            {Key.A, 'A'},{Key.S, 'S'},{Key.D, 'D'},{Key.F, 'F'},{Key.G, 'G'},{Key.H, 'H'},{Key.J, 'J'},{Key.K, 'K'},{Key.L, 'L'},{Key.OemSemicolon, ':'},{Key.OemQuotes, '"'},
            {Key.Z, 'Z'},{Key.X, 'X'},{Key.C, 'C'},{Key.V, 'V'},{Key.B, 'B'},{Key.N, 'N'},{Key.M, 'M'},{Key.OemComma, '<'},{Key.OemPeriod, '>'},{Key.OemQuestion, '?'}
        };

        // keys for showing pressing
        Dictionary<Key, Border> keyBorderPairs;
        
          
        public MainWindow()
        {
            InitializeComponent();
            keyBorderPairs = new Dictionary<Key, Border> 
            {
               {Key.Oem3, Tilda}, {Key.D1, One}, {Key.D2, Two}, {Key.D3, Three}, {Key.D4, Four}, {Key.D5, Five}, {Key.D6, Six}, {Key.D7, Seven},
               {Key.D8, Eight}, {Key.D9, Nine}, {Key.D0, Zero}, {Key.OemMinus, Miinus}, {Key.OemPlus, Plus}, {Key.Back, Backspace}, {Key.Tab, Tab},
               {Key.Q, Q}, {Key.W, W}, {Key.E, E}, {Key.R, R}, {Key.T, T}, {Key.Y, Y}, {Key.U, U}, {Key.I, I}, {Key.O, O}, {Key.P, P}, 
               {Key.OemOpenBrackets, OpenBrackets}, {Key.OemCloseBrackets, CloseBrackets}, {Key.Oem5, Backslash}, {Key.CapsLock, CapsLock}, 
               {Key.A, A}, {Key.S, S}, {Key.D, D}, {Key.F, F}, {Key.G, G}, {Key.H, H}, {Key.J, J}, {Key.K, K}, {Key.L, L},
               {Key.OemSemicolon, Semicolon}, {Key.OemQuotes, Quotes}, {Key.Enter, Enter }, {Key.LeftShift, LeftShift }, {Key.Z, Z}, {Key.X, X},
               {Key.C, C}, {Key.V, V}, {Key.B, B}, {Key.N, N}, {Key.M, M}, {Key.OemComma, Comma}, {Key.OemPeriod, Period}, {Key.OemQuestion, Slash},
               {Key.RightShift, RightShift}, {Key.LeftCtrl, LeftCtrl}, {Key.LWin, LefttWin}, {Key.LeftAlt, LeftAlt}, {Key.Space, Space},
               {Key.RightAlt, RightAlt}, {Key.RWin, RightWin}, {Key.RightCtrl, RightCtrl}
            };
        }

        private void ShowKeyClick(KeyEventArgs e)
        {
            if (keyBorderPairs.TryGetValue(e.Key, out currentBorder))
            {
                previousBorder.Background = previousColor;
                previousColor = currentBorder.Background;
                currentBorder.Background = new SolidColorBrush(Colors.White);
                previousBorder = currentBorder;
            }
        }

        private void KeyClick(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Check when test is started
            if (test.IsStarted)
            {
                // Show key click
                ShowKeyClick(e);

                // For moment when first symbol is typed
                if (!test.IsGoing)
                {
                    test.IsGoing = true;
                    test.startTime = DateTime.Now;
                }

                if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.CapsLock)
                {
                    return;
                }

                // Chack Space
                if (e.Key == Key.Space)
                {
                    InputText.Text = "";
                    TextToType.Text = TextToType.Text.Substring(TextToType.Text.IndexOf(' ') + 1) + " " + generator.Word;
                    InputText.Background = new SolidColorBrush(Colors.LightGreen);
                    return;
                }

                // Chack Backspace
                if (e.Key == Key.Back && InputText.Text.Length > 0)
                {
                    if (InputText.Text.Length <= TextToType.Text.Length && InputText.Text[InputText.Text.Length - 1] == TextToType.Text[InputText.Text.Length - 1])
                    {
                        test.Chars -= 1; // for not spamming one letter
                    }
                    InputText.Text = InputText.Text.Remove(InputText.Text.Length - 1); // delete letter

                    if (InputText.Text.Length <= TextToType.Text.Length && InputText.Text == TextToType.Text.Substring(0, InputText.Text.Length))
                    {
                        InputText.Background = new SolidColorBrush(Colors.LightGreen);
                    }

                    return;
                }

                char letter;
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) || Keyboard.IsKeyToggled(Key.CapsLock))
                {
                    if (keyCharUpper.TryGetValue(e.Key, out letter))
                    {
                        InputText.Text += letter;
                    }
                    foreach (var key in keyBorderPairs)
                    {
                        if (keyCharUpper.TryGetValue(key.Key, out letter))
                        {
                            ((TextBlock)key.Value.Child).Text = letter.ToString();
                        }
                    }
                }
                else
                {
                    if (keyCharLower.TryGetValue(e.Key, out letter))
                    {
                        InputText.Text += letter;
                    }
                    foreach (var key in keyBorderPairs)
                    {
                        if (keyCharLower.TryGetValue(key.Key, out letter))
                        {
                            ((TextBlock)key.Value.Child).Text = letter.ToString();
                        }
                    }
                }

                // check input symbol
                if (InputText.Text.Length <= TextToType.Text.Length && InputText.Text.Length > 0 && InputText.Text[InputText.Text.Length - 1] == TextToType.Text[InputText.Text.Length - 1])
                {
                    test.Chars += 1;
                }
                else
                {
                    test.Fails += 1;
                    InputText.Background = new SolidColorBrush(Colors.Tomato);
                }

                CharsPersMinTextBlock.Text = test.CharsPerMin.ToString();
                FailsTextBlock.Text = test.Fails.ToString();
            }

           
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            test.IsStarted = true;
            CharsPersMinTextBlock.Text = "0";
            FailsTextBlock.Text = "0";
            TextToType.Text = generator.Word + " " + generator.Word + " " + generator.Word;
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            test.Stop();
            TextToType.Text = "";
            InputText.Text = "";
            previousBorder.Background = previousColor;
        }

        private void DifficultyChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DifficultyTextBlock.Text = DifficultySlider.Value.ToString();
            generator.Level = (int)DifficultySlider.Value - 1;
        }

        private void ToUpper(object sender, RoutedEventArgs e)
        {
            generator.isLower = false;
        }

        private void ToLower(object sender, RoutedEventArgs e)
        {
            generator.isLower = true;
        }
    }
}
