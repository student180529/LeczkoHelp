using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LiniaProdukcyjna
{
    /// <summary>
    /// Logika interakcji dla klasy Linia.xaml
    /// </summary>
    public partial class Linia : Window
    {
        public static Linia singletonLinia;
        static public bool alarm = false;
        static public int ilosclodow;
        static Timer timer3s = new Timer();
        static Timer timer2s = new Timer();
        static int licznik3s;
        static int licznik2s;
        static public bool obrot;
        static public int wysokosclini1 = 152;
        static public int wysokosclini2 = 207;
        static public int srodekkolox = 65;
        static public int srodekkoloy = 179;
        static public int promienkola = 28;
        static public int odstep = 100;
        static public int wysokoscprzegrody = 30;
        static public double kat;
        static public int ilosckubkow;
        static public Line[] liniegora= new Line[7];
        static public Polygon[] kubki = new Polygon[7];
        static public Polygon drzwikubki = new Polygon();
        static public Line[] liniedol = new Line[7];
        static public Ellipse[] galki = new Ellipse[7];
        static public Line liniekolo = new Line();
        static public double temperatura = -20;
        static public int ilosc = 0;
        public Linia()
        {
            if (singletonLinia == null) singletonLinia = this;
            else return;

            InitializeComponent();
            licznik2s = 0;
            licznik3s = 0;
            ilosclodow = 0;
            kat = 0;
            ilosckubkow = 0;
            timer3s.Elapsed += new ElapsedEventHandler(Timer3s);
            timer3s.Interval = 30;
            timer3s.Enabled = false;
            timer2s.Elapsed += new ElapsedEventHandler(Timer2s);
            timer2s.Interval = 50;
            timer2s.Enabled = false;
            suwak.Value = -20;
            stan.Text = "bezczynność";
            temp.Text = temperatura.ToString()+"*C";
            gotowe.Text = ilosc.ToString() + " sztuk";
            for (int i = 0; i < 7; i++) {
                //gora
                liniegora[i] = new Line();
                liniegora[i].Visibility = System.Windows.Visibility.Visible;
                liniegora[i].Stroke = System.Windows.Media.Brushes.DarkViolet;
                liniegora[i].StrokeThickness = 5;
                liniegora[i].X1 = 90+i* odstep;
                liniegora[i].X2 = 90+i* odstep;
                liniegora[i].Y1 = wysokosclini1- wysokoscprzegrody;
                liniegora[i].Y2 = wysokosclini1;
                Okno.Children.Add(liniegora[i]);
                //dol
                liniedol[i] = new Line();
                liniedol[i].Visibility = System.Windows.Visibility.Visible;
                liniedol[i].Stroke = System.Windows.Media.Brushes.DarkViolet;
                liniedol[i].StrokeThickness = 5;
                liniedol[i].X1 = 90 + i * odstep;
                liniedol[i].X2 = 90 + i * odstep;
                liniedol[i].Y1 = wysokosclini2;
                liniedol[i].Y2 = wysokosclini2+ wysokoscprzegrody;
                Okno.Children.Add(liniedol[i]);
                //kubki
                kubki[i] = new Polygon();
                kubki[i].Stroke = System.Windows.Media.Brushes.Black;
                kubki[i].Fill = System.Windows.Media.Brushes.LightSeaGreen;
                kubki[i].StrokeThickness = 2;
                System.Windows.Point goralewo = new System.Windows.Point(90+35+odstep*i,wysokosclini1 -30);
                System.Windows.Point goraprawo = new System.Windows.Point(90 + 65 + odstep * i, wysokosclini1 -30);
                System.Windows.Point dollewo = new System.Windows.Point(90 + 40 + odstep * i, wysokosclini1);
                System.Windows.Point dolprawo = new System.Windows.Point(90 + 60 + odstep * i, wysokosclini1);
                PointCollection punkty = new PointCollection();
                punkty.Add(goralewo);
                punkty.Add(goraprawo);
                punkty.Add(dolprawo);
                punkty.Add(dollewo);
                kubki[i].Points = punkty;
                kubki[i].Visibility = System.Windows.Visibility.Hidden;
                Okno.Children.Add(kubki[i]);
                //galki
                galki[i] = new Ellipse();
                Oknogalki.Children.Add(galki[i]);
                galki[i].Stroke = System.Windows.Media.Brushes.Black;
                galki[i].StrokeThickness = 1;
                galki[i].Fill = System.Windows.Media.Brushes.DarkBlue;
                galki[i].Width = 25;
                galki[i].Height = 25;
                galki[i].Visibility = System.Windows.Visibility.Hidden;
                Canvas.SetLeft(galki[i], 127+100*i);
                Canvas.SetTop(galki[i], wysokosclini1-42.5);
            }
            //kolo
            liniekolo.Visibility = System.Windows.Visibility.Hidden;
            liniekolo.Stroke = System.Windows.Media.Brushes.DarkViolet;
            liniekolo.StrokeThickness = 5;
            liniekolo.X1 = srodekkolox;
            liniekolo.X2 = srodekkolox;
            liniekolo.Y1 = srodekkoloy+promienkola;
            liniekolo.Y2 = srodekkoloy+wysokoscprzegrody+promienkola;
            Okno.Children.Add(liniekolo);
            //drzwikubki
            System.Windows.Point gl = new System.Windows.Point(116,90);
            System.Windows.Point gp = new System.Windows.Point(116+46,90);
            System.Windows.Point dl = new System.Windows.Point(116,87);
            System.Windows.Point dp = new System.Windows.Point(116+46,87);
            PointCollection pkt = new PointCollection();
            pkt.Add(gl);
            pkt.Add(gp);
            pkt.Add(dp);
            pkt.Add(dl);
            drzwikubki.Points = pkt;
            drzwikubki.Stroke = System.Windows.Media.Brushes.Black;
            drzwikubki.Fill = System.Windows.Media.Brushes.LightGray;
            drzwikubki.StrokeThickness = 2;
            drzwikubki.Visibility = System.Windows.Visibility.Visible;
            Okno.Children.Add(drzwikubki);
        }
        private static void Timer3s(object source, ElapsedEventArgs e)//animacja lini
        {
            licznik3s++;
            if (licznik3s < odstep)
            {
                for (int i = 0; i < 7; i++)
                {
                    liniegora[i].Dispatcher.Invoke(() => { liniegora[i].X1 = liniegora[i].X1 + 1; });
                    liniegora[i].Dispatcher.Invoke(() => { liniegora[i].X2 = liniegora[i].X2 + 1; });
                    liniedol[i].Dispatcher.Invoke(() => { liniedol[i].X1 = liniedol[i].X1 - 1; });
                    liniedol[i].Dispatcher.Invoke(() => { liniedol[i].X2 = liniedol[i].X2 - 1; });
                    PointCollection punkty = new PointCollection();
                    kubki[i].Dispatcher.Invoke(() => { punkty = kubki[i].Points; });
                    punkty.Dispatcher.Invoke(() => { punkty[0]= new System.Windows.Point(punkty[0].X+1, punkty[0].Y); });
                    punkty.Dispatcher.Invoke(() => { punkty[1] = new System.Windows.Point(punkty[1].X + 1, punkty[1].Y); });
                    punkty.Dispatcher.Invoke(() => { punkty[2] = new System.Windows.Point(punkty[2].X + 1, punkty[2].Y); });
                    punkty.Dispatcher.Invoke(() => { punkty[3] = new System.Windows.Point(punkty[3].X + 1, punkty[3].Y); });
                    kubki[i].Dispatcher.Invoke(() => { kubki[i].Points = punkty; });
                    galki[i].Dispatcher.Invoke(() => {double pozycja= Canvas.GetLeft(galki[i]); Canvas.SetLeft(galki[i], pozycja + 1); });

                }
                if (licznik3s == 20)
                {
                    liniegora[6].Dispatcher.Invoke(() =>{liniegora[6].Visibility = System.Windows.Visibility.Hidden;});
                    liniegora[0].Dispatcher.Invoke(() =>{liniedol[0].Visibility = System.Windows.Visibility.Hidden;});
                    liniekolo.Dispatcher.Invoke(() => { liniekolo.Visibility = System.Windows.Visibility.Visible; });
                    kubki[5].Dispatcher.Invoke(() => { kubki[5].Visibility = System.Windows.Visibility.Hidden; });
                    galki[5].Dispatcher.Invoke(() => { galki[5].Visibility = System.Windows.Visibility.Hidden; });
                    obrot = true;
                    if (ilosckubkow > 5) {
                        ilosc++;
                        WypiszIlosc(singletonLinia.gotowe);
                        //Thread thread = new Thread(WypiszIlosc());
                        //thread.Start();
                        //Linia.singletonLinia.gotowe.Dispatcher.Invoke(() =>
                        //{
                        //    gotowe.Text = ilosc.ToString() + " sztuk";
                        //    WypiszIlosc(gotowe);
                        //});
                    }
                }
                //kolo
                if (obrot)
                {
                    liniekolo.Dispatcher.Invoke(() => { liniekolo.X1 = srodekkolox - promienkola * Math.Sin(kat); });
                    liniekolo.Dispatcher.Invoke(() => { liniekolo.Y1 = srodekkoloy + promienkola * Math.Cos(kat); });
                    liniekolo.Dispatcher.Invoke(() => { liniekolo.X2 = srodekkolox - (promienkola + wysokoscprzegrody) * Math.Sin(kat); });
                    liniekolo.Dispatcher.Invoke(() => { liniekolo.Y2 = srodekkoloy + (promienkola + wysokoscprzegrody) * Math.Cos(kat); });
                    kat += Math.PI / 75;
                }
            }
            else {
                licznik3s = 0;
                kat = 0;
                obrot = false;
                liniekolo.Dispatcher.Invoke(() => { liniekolo.Visibility = System.Windows.Visibility.Hidden; });
                for (int i = 0; i < 7; i++)
                {
                    liniegora[i].Dispatcher.Invoke(() => { liniegora[i].X1 = liniegora[i].X1 - odstep; });
                    liniegora[i].Dispatcher.Invoke(() => { liniegora[i].X2 = liniegora[i].X2 - odstep; });
                    liniedol[i].Dispatcher.Invoke(() => { liniedol[i].X1 = liniedol[i].X1 + odstep; });
                    liniedol[i].Dispatcher.Invoke(() => { liniedol[i].X2 = liniedol[i].X2 + odstep; });
                    PointCollection punkty = new PointCollection();
                    kubki[i].Dispatcher.Invoke(() => { punkty = kubki[i].Points; });
                    punkty.Dispatcher.Invoke(() => { punkty[0] = new System.Windows.Point(90 + 35 + odstep * i, wysokosclini1 - 30); });
                    punkty.Dispatcher.Invoke(() => { punkty[1] = new System.Windows.Point(90 + 65 + odstep * i, wysokosclini1 - 30); });
                    punkty.Dispatcher.Invoke(() => { punkty[3] = new System.Windows.Point(90 + 40 + odstep * i, wysokosclini1); });
                    punkty.Dispatcher.Invoke(() => { punkty[2] = new System.Windows.Point(90 + 60 + odstep * i, wysokosclini1); });
                    kubki[i].Dispatcher.Invoke(() => { kubki[i].Points = punkty; });
                    galki[i].Dispatcher.Invoke(() => { Canvas.SetLeft(galki[i], 127 + 100 * i); });
                    galki[i].Dispatcher.Invoke(() => { Canvas.SetTop(galki[i], wysokosclini1 - 42.5); });
                    }
                liniegora[0].Dispatcher.Invoke(() =>{liniedol[0].Visibility = System.Windows.Visibility.Hidden;});
                kubki[6].Dispatcher.Invoke(() => { kubki[6].Visibility = System.Windows.Visibility.Hidden; });
                if (ilosckubkow > 5)
                {
                    kubki[5].Dispatcher.Invoke(() => { kubki[5].Visibility = System.Windows.Visibility.Visible; });
                    galki[5].Dispatcher.Invoke(() => { galki[5].Visibility = System.Windows.Visibility.Visible; });
                }
                timer3s.Enabled = false;
                timer2s.Enabled = true;
            }
        }
        private static void Timer2s(object source, ElapsedEventArgs e)//animacja stanowisk
        {
            PointCollection punkty = new PointCollection();
            if (ilosckubkow > 0 && ilosckubkow<6) {
                kubki[(ilosckubkow+1)].Dispatcher.Invoke(() => { kubki[ilosckubkow].Visibility = System.Windows.Visibility.Visible; });
            }
            if (licznik2s < 20)
            {
                drzwikubki.Dispatcher.Invoke(() => { punkty = drzwikubki.Points; });
                punkty.Dispatcher.Invoke(() => { punkty[0] = new System.Windows.Point(punkty[0].X, punkty[0].Y + 2.5); });
                punkty.Dispatcher.Invoke(() => { punkty[1] = new System.Windows.Point(punkty[1].X, punkty[1].Y + 2.5); });
                drzwikubki.Dispatcher.Invoke(() => { drzwikubki.Points = punkty; });
            }
            else if (licznik2s < 40)
            {
                if (ilosckubkow > 1 && ilosckubkow < 6)
                {
                    galki[ilosckubkow].Dispatcher.Invoke(() => { galki[ilosckubkow].Visibility = System.Windows.Visibility.Visible; });
                }
            }
            else if (licznik2s == 40) {
                if (ilosckubkow < 6)
                {
                    kubki[ilosckubkow].Dispatcher.Invoke(() => { kubki[ilosckubkow].Visibility = System.Windows.Visibility.Visible; });
                }
            }
            else if (licznik2s < 60)
            {
                drzwikubki.Dispatcher.Invoke(() => { punkty = drzwikubki.Points; });
                punkty.Dispatcher.Invoke(() => { punkty[0] = new System.Windows.Point(punkty[0].X, punkty[0].Y - 2); });
                punkty.Dispatcher.Invoke(() => { punkty[1] = new System.Windows.Point(punkty[1].X, punkty[1].Y - 2); });
                drzwikubki.Dispatcher.Invoke(() => { drzwikubki.Points = punkty; });
            }
            else
            {
                drzwikubki.Dispatcher.Invoke(() => { punkty = drzwikubki.Points; });
                punkty.Dispatcher.Invoke(() => { punkty[0] = new System.Windows.Point(116, 90); });
                punkty.Dispatcher.Invoke(() => { punkty[1] = new System.Windows.Point(116 + 46, 90); });
                drzwikubki.Dispatcher.Invoke(() => { drzwikubki.Points = punkty; });
                licznik2s = 0;
                ilosckubkow++;
                timer3s.Enabled = true;
                timer2s.Enabled = false;
            }
            licznik2s++;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer3s.Enabled = false;
            timer2s.Enabled = false;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stan.Text = "działanie";
            if (licznik3s > 0)
            {
                timer3s.Enabled = true;
            }
            else {
                timer2s.Enabled = true;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            stan.Text = "bezczynność";
            timer3s.Enabled = false;
            timer2s.Enabled = false;
        }

        private void suwak_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            temperatura = suwak.Value;
            temp.Text = temperatura.ToString("0.##") + "*C";
            if (!alarm)
            {
                if (temperatura > -10 || temperatura < -25)
                {
                    Alarm("Zła temperatura!!!!");
                    alarm = true;
                }
            }
            else
            {
                if (temperatura < -10 && temperatura > -25)
                {
                    KoniecAlarmu();
                    alarm = false;
                }
            }
        }
        private void Alarm(string alarm) {
            timer3s.Enabled = false;
            timer2s.Enabled = false;
            stan.Text = "alarm";
            MessageBox.Show(alarm);
            Background =Brushes.Red;
        }
        private void KoniecAlarmu() {
            stan.Text = "działanie";
            if (licznik3s > 0)
            {
                timer3s.Enabled = true;
            }
            else
            {
                timer2s.Enabled = true;
            }
            Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDBCA79");
        }

        private static void WypiszIlosc(TextBox textBox) {
            textBox.Text = ilosc.ToString() + " sztuk";
        }
    }
}
