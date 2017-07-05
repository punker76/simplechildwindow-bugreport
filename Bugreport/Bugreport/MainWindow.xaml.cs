namespace FreezableExceptionDemo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using MahApps.Metro.Controls;
    using MahApps.Metro.SimpleChildWindow;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool StartThreadWhenLoaded { get; set; } = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowTestOverlay()
        {
            var childWindow = new ChildWindow();

            try
            {
                childWindow.Content = new TextBlock() { Text = "Test" };
            }
            catch (InvalidOperationException ex)
            {
                // This exception is thrown when a second ChildWindow is created
                throw;
            }
            childWindow.ShowCloseButton = true;
            childWindow.ShowTitleBar = true;
            childWindow.Title = "Test";

            this.ShowChildWindowAsync(childWindow);
        }

        private void StartWindowThread()
        {
            var ts = new ThreadStart(() =>
                {
                    var window = new MainWindow();
                    window.StartThreadWhenLoaded = false;
                    window.ShowDialog();
                }
            );

            var thread = new Thread(ts);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void ShowOverlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowTestOverlay();
        }

        private void StartThreadButton_OnClick(object sender, RoutedEventArgs e)
        {
            StartWindowThread();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (StartThreadWhenLoaded)
            {
                // sleep a bit to avoid a bug in the XamlInspection tools
                await Task.Delay(1000);
                StartWindowThread();
            }

            //await Task.Delay(2000);
            //ShowOverlay();
        }
    }
}
