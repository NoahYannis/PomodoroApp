using System;
using System.Data;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PomodoroApp
{
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        DataTable table = new DataTable();
        Notiz notiz = new Notiz();
        int sekunden, minuten;
        bool fokussierungsphase = true, willNotiz = false;

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
            ResizeMode = ResizeMode.NoResize;
            var currentindex = datagridNotiz.Items.IndexOf(datagridNotiz.CurrentCell);
            table.Columns.Add("Titel", typeof(String));
            table.Columns.Add("Notiz", typeof(String));
            Notizfenster();                  
        }

        public class Notiz
        {
            public string titel { get; set; }
            public string datum { get; set; }
            public string notiz { get; set; }
        }
  
        public void timerTick(object sender, EventArgs e)
        {
            lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");

            if (minuten >= 0 | sekunden > 0)
            {
                sekunden -= 1;

                if (minuten > 0 && sekunden < 0)
                {                   
                    sekunden = 59;
                    minuten -= 1;
                }
                else if (minuten == 0 && sekunden < 0)
                {
                    if (fokussierungsphase == true)
                    {
                        MessageBox.Show("Sehr gut! Es wird Zeit, eine kleine Pause einzulegen!");
                        timer.Stop();
                        lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
                    }
                    else if (fokussierungsphase == false)
                    {
                        MessageBox.Show("Hoffentlich konntest du dich entspannen! Es wird Zeit, weiterzumachen :)");
                        timer.Stop();
                        lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
                    }
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (minuten > 0 | sekunden > 0)
            {
                timer.Start();
                Knopffarbe();

                if (fokussierungsphase == true)
                {
                    pbPhase.Source = new BitmapImage(new Uri(@"/PomodoroSonne.png", UriKind.Relative));
                }
                else if (fokussierungsphase == false)
                {
                    pbPhase.Source = new BitmapImage(new Uri(@"/PomodoroMondbildPNG.png", UriKind.Relative));
                }
            }

            else
            {
                MessageBox.Show("Bitte gebe eine gültige Zeit ein.");
            }
        }

        private void btnPhasenwechsel_Click(object sender, RoutedEventArgs e)
        {
            if (fokussierungsphase == true)
            {
                minuten = 5;
                sekunden = 0;
                timer.Start();
                lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
                btnPhasenwechsel.Content = "Fokussierung starten";
            }

            if (fokussierungsphase == false)
            {
                btnPhasenwechsel.Content = "Pause starten";
                minuten = 25;
                sekunden = 0;
                timer.Start();
                lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
            }

            if (fokussierungsphase == true)
            {
                fokussierungsphase = false;
                pbPhase.Source = new BitmapImage(new Uri(@"/PomodoroMondbildPNG.png", UriKind.Relative));               

            }
            else if (fokussierungsphase == false)
            {
                fokussierungsphase = true;
                pbPhase.Source = new BitmapImage(new Uri(@"/PomodoroSonne.png", UriKind.Relative));
            }

            Knopffarbe();
        }

        private void btnMinuten_Click(object sender, RoutedEventArgs e)
        {
            minuten++;
            lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
        }

        private void btnSekunden_Click(object sender, RoutedEventArgs e)
        {
            sekunden += 30; 

            if (sekunden >= 60)
            {
                minuten++;
                sekunden = sekunden - 60;
            }

            lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
        }

        private void lblFarbwechsel(object sender, MouseButtonEventArgs e)
        {
            Random farbe = new Random();
            int labelFarbe = farbe.Next(1, 7);

            switch (labelFarbe)
            {
                case 1:

                    lblTimer.Foreground = Brushes.Yellow;
                    break;

                case 2:

                    lblTimer.Foreground = Brushes.Blue;
                    break;

                case 3:

                    lblTimer.Foreground = Brushes.Red;
                    break;

                case 4:

                    lblTimer.Foreground = Brushes.Green;
                    break;

                case 5:

                    lblTimer.Foreground = Brushes.Purple;
                    break;

                case 6:

                    lblTimer.Foreground = Brushes.Black;
                    break;

                default:
                    lblTimer.Foreground = Brushes.Black;
                    break;
            }

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void btnNeu_Click(object sender, RoutedEventArgs e)
        {
            lblTimer.Content = String.Format($"{minuten:d2}:{sekunden:d2}");
            minuten = 0;
            sekunden = 0;
            timer.Stop();
        }

        private void Knopffarbe()
        {
            if (fokussierungsphase == true)
            {
   
               btnMinuten.Background = Brushes.Yellow;
               btnMinuten.Foreground = Brushes.Black;

               btnSekunden.Background = Brushes.Yellow;
               btnSekunden.Foreground = Brushes.Black;

               btnNeu.Background = Brushes.Yellow;
               btnNeu.Foreground = Brushes.Black;

               btnStop.Background = Brushes.Yellow;
               btnStop.Foreground = Brushes.Black;

               btnStart.Background = Brushes.Yellow;
               btnStart.Foreground = Brushes.Black;

               btnPhasenwechsel.Background = Brushes.Yellow;
               btnPhasenwechsel.Foreground = Brushes.Black;

               btnNotiz.Background = Brushes.Yellow;
               btnNotiz.Foreground = Brushes.Black;  

            }
            else if (fokussierungsphase == false)
            {
                btnMinuten.Background = Brushes.Black;
                btnMinuten.Foreground = Brushes.White;

                btnSekunden.Background = Brushes.Black;
                btnSekunden.Foreground = Brushes.White;

                btnNeu.Background = Brushes.Black;
                btnNeu.Foreground = Brushes.White;

                btnStop.Background = Brushes.Black;
                btnStop.Foreground = Brushes.White;

                btnStart.Background = Brushes.Black;
                btnStart.Foreground = Brushes.White;

                btnPhasenwechsel.Background = Brushes.Black;
                btnPhasenwechsel.Foreground = Brushes.White;

                btnNotiz.Background = Brushes.Black;
                btnNotiz.Foreground = Brushes.White;
            }
        }

        private void btnNotiz_Click(object sender, RoutedEventArgs e)
        {
            willNotiz = true;
            Notizfenster();
        }
        
        private void btnSchließen_Click(object sender, RoutedEventArgs e)
        {
            willNotiz = false;
            Notizfenster();
            timer.Start();
        }

        private void btnLesen_Click(object sender, RoutedEventArgs e)
        {          
            int currentindex = datagridNotiz.SelectedIndex;
            datagridNotiz.Items.Refresh();

            if (currentindex > -1)
            {
                txtTitel.Text = table.Rows[currentindex]["Titel"].ToString();
                txtNotiz.Text = table.Rows[currentindex]["Notiz"].ToString();
            }
        }
        
        private void btnLöschen_Click(object sender, RoutedEventArgs e)
        {
            int currentindex = datagridNotiz.SelectedIndex;

            MessageBoxResult result = MessageBox.Show("Bist du sicher, dass du diesen Eintrag löschen möchtest?", "Pomodoro App", MessageBoxButton.YesNo);
          
            if (result == MessageBoxResult.Yes)
            {
                if (currentindex > 0)
                {
                    datagridNotiz.Items.RemoveAt(currentindex);
                    table.Rows.RemoveAt(currentindex);

                    txtTitel.Clear();
                    txtNotiz.Clear();
                }
                else
                {
                    MessageBox.Show("Es gibt keinen Eintrag, den du löschen könntest.", "Pomodoro App", MessageBoxButton.OK);
                }
            }                   
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            if (txtTitel.Text.Length > 0)
            {
                Notiz notiz = new Notiz();

                notiz.titel = txtTitel.Text;
                notiz.notiz = txtNotiz.Text;
                notiz.datum = Convert.ToString(DateTime.Now);

                table.Rows.Add(notiz.titel, notiz.notiz);
                datagridNotiz.Items.Add(notiz);
                
                txtTitel.Clear();
                txtNotiz.Clear();
            }
            else if (txtTitel.Text.Length == 0)
            {
                MessageBox.Show("Bitte gib einen Titel ein.", "Pomodoro App");
            }         
        }

        private void Notizfenster()
        {
            if (willNotiz)
            {
                lblTitel.Visibility = Visibility.Visible;
                lblNotiz.Visibility = Visibility.Visible;
                txtTitel.Visibility = Visibility.Visible;
                txtNotiz.Visibility = Visibility.Visible;
                datagridNotiz.Visibility = Visibility.Visible;
                btnLöschen.Visibility = Visibility.Visible;
                btnSpeichern.Visibility = Visibility.Visible;
                btnLesen.Visibility = Visibility.Visible;
                btnSchließen.Visibility = Visibility.Visible;
               

                timer.Stop();
                lblTimer.Visibility = Visibility.Hidden;
                btnStart.Visibility = Visibility.Hidden;
                btnStop.Visibility = Visibility.Hidden;
                btnMinuten.Visibility = Visibility.Hidden;
                btnSekunden.Visibility = Visibility.Hidden;
                btnPhasenwechsel.Visibility = Visibility.Hidden;
                btnNotiz.Visibility = Visibility.Hidden;
                pbPhase.Visibility = Visibility.Hidden;
            }
            else
            {
                lblTitel.Visibility = Visibility.Hidden;
                lblNotiz.Visibility = Visibility.Hidden;
                txtTitel.Visibility = Visibility.Hidden;
                txtNotiz.Visibility = Visibility.Hidden;
                datagridNotiz.Visibility = Visibility.Hidden;
                btnLöschen.Visibility = Visibility.Hidden;
                btnSpeichern.Visibility = Visibility.Hidden;
                btnLesen.Visibility = Visibility.Hidden;
                btnSchließen.Visibility = Visibility.Hidden;

                lblTimer.Visibility = Visibility.Visible;
                btnStart.Visibility = Visibility.Visible;
                btnStop.Visibility = Visibility.Visible;
                btnMinuten.Visibility = Visibility.Visible;
                btnSekunden.Visibility = Visibility.Visible;
                btnPhasenwechsel.Visibility = Visibility.Visible;
                btnNotiz.Visibility = Visibility.Visible;
                pbPhase.Visibility = Visibility.Visible;
            }
        }    
    }
}
