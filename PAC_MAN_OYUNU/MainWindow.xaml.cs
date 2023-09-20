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
using System.Windows.Threading;

namespace PAC_MAN_OYUNU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool goLeft, goRight, goDown, goUp;
        bool noLeft, noRight, noDown, noUp;
        int speed = 8;
        Rect pacmanHitBox;
        int hayaletSpeed = 5;
        int hayaletMoveStep = 200;
        int currentHayaletStep;
        int score = 0;
        public MainWindow()
        {
            InitializeComponent();
            GameSetUp();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goDown = goUp = false;
                noRight = noDown = noUp = false;
                goLeft = true;
                pacman.RenderTransform = new RotateTransform(180, pacman.Width / 2, pacman.Height / 2);
            }
            if (e.Key == Key.Right && noRight == false)
            {
                goLeft = goUp = goDown = false;
                noLeft = noUp = noDown = false;
                goRight = true;
                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }
            if (e.Key == Key.Up && noUp == false)
            {
                goRight = goLeft = goDown = false;
                noRight = noLeft = noDown = false;
                goUp = true;
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }
            if (e.Key == Key.Down && noDown == false)
            {
                goRight = goDown = goLeft = false;
                noRight = noDown = noLeft = false;
                goDown = true;
                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }
        }

        private void GameSetUp()
        {
            MyCanvas.Focus();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            currentHayaletStep = hayaletMoveStep;
            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resimler/1200px-Pac_Man.svg.png"));
            pacman.Fill = pacmanImage;
            ImageBrush redHayalet = new ImageBrush();
            redHayalet.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resimler/580b57fcd9996e24bc43c316.png"));
            redDusman.Fill = redHayalet;
            ImageBrush orangeHayalet = new ImageBrush();
            orangeHayalet.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resimler/enhanced-1559-1603203875-2.png"));
            orangeDusman.Fill = orangeHayalet;
            ImageBrush pinkHayalet = new ImageBrush();
            pinkHayalet.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resimler/f92b5eb0b854c0c2dcf55eadcd2ec0bd.jpg"));
            pinkDusman.Fill = pinkHayalet;

        }

        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }
            if (goDown && Canvas.GetTop(pacman) + 100 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                goDown = false;
            }
            if (goUp && Canvas.GetTop(pacman) < 1)
            {
                noUp = true;
                goUp = false;
            }
            if (goLeft && Canvas.GetLeft(pacman) - 10 < 1)
            {
                noLeft = true;
                goLeft = false;
            }
            if (goRight && Canvas.GetLeft(pacman) + 60 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                goRight = false;
            }
            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                if ((string)x.Tag == "duvar")
                {
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }
                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;
                    }
                }
                if ((string)x.Tag == "hayalet")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {

                        GameOver("Hayaletler seni yakaladı, Tekrar oynamak için tamam'a bas");
                    }
                    if (x.Name.ToString() == "orangeDusman")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - hayaletSpeed);
                    }
                    else
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + hayaletSpeed);
                    }
                    currentHayaletStep--;
                    if (currentHayaletStep < 1)
                    {
                        currentHayaletStep = hayaletMoveStep;
                        hayaletSpeed = -hayaletSpeed;
                    }
                } 
            }
            if (score == 140)
            {
                GameOver("Kazandınız, Bütün coinleri topladınız");
            }
        }

        private void GameOver(string message)
        {
            //140
            gameTimer.Stop();
            MessageBox.Show(message, "PAC MAN GAME");
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
