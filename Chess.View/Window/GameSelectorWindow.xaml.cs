namespace Chess.View.Window
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for GameSelectorWindow.xaml
    /// </summary>
    public partial class GameSelectorWindow : Window
    {
        /// <summary>
        /// Interaction logic for <see cref="GameSelectorWindow"/> window.
        /// </summary>
        public GameSelectorWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dependent on the name of the button that was chosen
        /// go to main window using the corseponding constructor
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Additional information about the event.</param>
        private void selectorClick(object sender, RoutedEventArgs e)
        {
            Button selector = sender as Button;
            switch(selector.Name)
            {
                case "regular":
                    MainWindow regular = new MainWindow();
                    regular.Show();
                    this.Close();
                    break;
                case "ninesixty":
                    MainWindow nineSixty = new MainWindow("960VM");
                    nineSixty.Show();
                    this.Close();
                    break;
                default:
                    MainWindow defaultWindow = new MainWindow("default");
                    defaultWindow.Show();
                    this.Close();
                    break;
            }
        }
    }
}
