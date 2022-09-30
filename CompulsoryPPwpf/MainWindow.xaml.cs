using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace CompulsoryPPwpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<long> primes; 
        public event PropertyChangedEventHandler PropertyChanged;
        public string timeElapsed { get; set; }

        private DispatcherTimer timer;
        private Stopwatch stopWatch;
        PrimeGenerator primegen;
        public MainWindow()
        {
            InitializeComponent();

            // CmbScenario.Items
            CmbScenario.Items.Add("Parallel");
            CmbScenario.Items.Add("Sequential");
            CmbScenario.SelectedIndex = 0;
            RangeFrom.Text = "5";
            RangeTo.Text = "100";

            primegen = new PrimeGenerator();

        }

        public void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += dispatcherTimerTick_;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            stopWatch = new Stopwatch();
            stopWatch.Start();
            timer.Start();
        }

        private void dispatcherTimerTick_(object sender, EventArgs e)
        {
            timeElapsed = stopWatch.Elapsed.TotalSeconds.ToString(); // Format as you wish
            //PropertyChanged(this, new PropertyChangedEventArgs("timeElapsed"));

            ElapsedTime.Text = timeElapsed;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            long first;
            long last;

            if(RangeFrom.Text.Length != 0 && RangeTo.Text.Length != 0)
            {
                StartTimer();
                first = (long)Convert.ToDouble(RangeFrom.Text);
                last = (long)Convert.ToDouble(RangeTo.Text);
                
                primeListBox.ItemsSource = CmbScenario.SelectedIndex == 0 ? new ObservableCollection<long>(await primegen.GetPrimeNumbersParallelAsync(first, last)) : new ObservableCollection<long>(await primegen.GetPrimeNumbersSequentialAsync(first, last));

                stopWatch.Stop();
            }
        }
    }
}
