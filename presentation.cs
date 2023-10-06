namespace jumping_pathak
{
        public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            gameTimer.Tick += GameEngine;
            gameTimer.Interval =  TimeSpan.FromMilliseconds(20);

            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/back.png"));
            Background1.Fill = backgroundSprite;
            Background2.Fill = backgroundSprite;


            // for making the initial welcome screen

            initialBack.Fill = new SolidColorBrush(Colors.Black);
            initialBack.Width = 1000;
            initialBack.Height = 1000;

            Canvas.Children.Add(initialBack);

            initialBack.Opacity= 0.7;

            Player.Opacity= 0;
            Obstacle.Opacity= 0;
            jerry.Opacity= 0;
            ScoreTxt.Opacity= 0;

        }
     }
}
