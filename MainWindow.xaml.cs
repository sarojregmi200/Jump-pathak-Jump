using jump_pathak_jump;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// this namespace is needed to access the dispatcher timer
using System.Windows.Threading;

namespace jumping_pathak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();
        bool jumping;
        bool gamerOver;

        bool gravity = false;

        bool gameRunning = true;

        Rectangle initialBack = new Rectangle();
        bool eaten = false;

        ImageBrush playerSprite = new ImageBrush();
        ImageBrush backgroundSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();
        ImageBrush coinSprite = new ImageBrush();

        int score = 0;

        double runnerSpriteIndex = 1;
        double dyingIndex = 1;

        intersectionObserver collisionObserver = new intersectionObserver();    


        // this is the main function constructure function that will run when the game loads
        public MainWindow()
        {
            
            // to initialize all the components
            InitializeComponent();

            // when the game loads we want the canvas to be in focus
            Canvas.Focus();

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

        private void GameEngine(object sender, EventArgs e)
        {
            if (gameRunning == true)
            {

                backgroundParalax(5);
                obstacleParalax(8);

                 if (jumping == true)
                {
                    jumpPathak(15);
                }
                else
                {
                     // implementing gravity
                    if (VisualTreeHelper.GetOffset(Player).Y < 50)
                    {
                        gravity = true;
                    }
                    if(gravity == true && VisualTreeHelper.GetOffset(Player).Y < 130)
                    {
                        Canvas.SetTop(Player, VisualTreeHelper.GetOffset(Player).Y + 3);
                    }
                    else if (gravity == true && VisualTreeHelper.GetOffset(Player).Y < 221)
                    {
                        Canvas.SetTop(Player, VisualTreeHelper.GetOffset(Player).Y + 5);
                    }
                    else
                    {
                        gravity = false;
                    }
                }
                
               


                if(jumping == false && gravity == false)
                {
                    // running animation
                    runnerSpriteIndex += 0.5;
                    if (runnerSpriteIndex >= 5)
                    {
                        runnerSpriteIndex = 1;
                    }
                    runSprite(runnerSpriteIndex);
                }


                // checking the Obstacle collision
                checkObstacleCollision();
                // checking for the coin collection
                if(collisionObserver.observe(jerry, Player))
                {
                    score += 1;
                    ScoreTxt.Content = "score : " + score;

                    Canvas.SetLeft(jerry, 1000);

                }
                jerryparalax(8);
            }

            if(gamerOver == true && dyingIndex < 10)
            {
                dyingIndex += 0.5;
                dyingAnimation(dyingIndex);
            }

        }


        private void keyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                startGame();
            }
            
        }

        private void keyUp(object sender , KeyEventArgs e)
        {
            if(e.Key == Key.Space && jumping == false && gravity == false)
            {
                jumping = true;
            }
        }

        // for starting the game
        private void startGame()
        {

            removeInitialScreen();
            obstacleSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/karela.png"));
            Obstacle.Fill = obstacleSprite;
            
            coinSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/jerry.png"));
            jerry.Fill = coinSprite;
            Canvas.SetLeft(jerry, 1000); // initial jerry position

            // default player sprite
            playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/rf4.png"));


            jumping = false;
            gameRunning = true;

            gamerOver = false;

            score= 0;
            ScoreTxt.Content = "Score: " + score;
            Player.Height = 139;
            Player.Width = 87;

            Canvas.SetLeft(Obstacle, 1200); 
            gameTimer.Start();
        }
   
        // for moving the background
        private void backgroundParalax(int speed)
        {
            if (Canvas.GetLeft(Background1) <= -Background1.ActualWidth) Canvas.SetLeft(Background1, Background1.ActualWidth);
            if (Canvas.GetLeft(Background2) <= -Background2.ActualWidth) Canvas.SetLeft(Background2, Background2.ActualWidth);

            Canvas.SetLeft(Background1, Canvas.GetLeft(Background1) - speed);
            Canvas.SetLeft(Background2, Canvas.GetLeft(Background2) - speed);
        }

        // for moving the object
        private void obstacleParalax(int speed)
        {
            if(Canvas.GetLeft(Obstacle) <  - Player.Width)
            {
            Canvas.SetLeft(Obstacle,   1200);
            }

            Canvas.SetLeft(Obstacle, Canvas.GetLeft(Obstacle) - speed);
        }

        private void removeInitialScreen()
        {
            newgame.Opacity= 0;
            secondtext.Opacity= 0;  
            initialBack.Opacity= 0; 

            Player.Opacity= 1;
            Obstacle.Opacity= 1;
            jerry.Opacity= 1;   
            ScoreTxt.Opacity= 1;
        }


        private void setupDeadScreen()
        {
            initialBack.Opacity= 0.7;

            secondtext.Content = "Press enter to play again";
            newgame.Content = "Score: "+ score +""; 

            ScoreTxt.Opacity= 0;
            secondtext.Opacity= 1;  
            newgame.Opacity= 1;
        }
        // contains the jump logic
        private void jumpPathak(int force)
        {

            double playerTopOffset = VisualTreeHelper.GetOffset(Player).Y;
            double maxJump = 50;


            if (playerTopOffset < maxJump)
            {
                jumping = false;
            }
            else
            {
                Canvas.SetTop(Player, playerTopOffset - force);
            }

        }

        // for creating a running animation
        private void runSprite(double i)
        {
           switch (i) {
             case 1:
               playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/rf1.png"));
             break;
             case 2:
               playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/rf2.png"));
             break;
             case 3:
               playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/rf3.png"));
             break;
             case 4:
               playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/rf4.png"));
             break;
                 
           }

            Player.Fill = playerSprite;


        }

        public void dyingAnimation(double i)
        {
            switch (i)
            {
                case 1:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df1.png"));
                    break;

                case 2:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df2.png"));
                    Player.Height = 145;
                    Player.Width = 125;
                    break;
                    
                case 3:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df3.png"));
                    Player.Height = 120;
                    Player.Width = 130;
                    break;
                case 4:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df4.png"));
                    Player.Height = 100;
                    Player.Width = 150;
                    break;
                case 5:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df5.png"));
                    Canvas.SetTop(Player, Canvas.GetTop(Player) + 10);
                    break;
                case 6:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df6.png"));
                    Canvas.SetTop(Player, Canvas.GetTop(Player) + 20);
                    break;
                case 7:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df7.png"));
                    break;
                case 8:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df8.png"));
                    break;
                case 9:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/df9.png"));
                    break;

            }


            Player.Fill = playerSprite;
          

        }

        // contains logic for the player when it hits the obstacle
        private void checkObstacleCollision()
        {
            if (collisionObserver.observe(Player, Obstacle) == true)
            {
                // ending the game
                gamerOver = true;
                gameRunning = false;
                setupDeadScreen();  
            }

        }


        private void jerryparalax(double speed)
        {

            if (Canvas.GetLeft(jerry) < -Player.Width)
            {
                Canvas.SetLeft(jerry, 1000);
            }

            Canvas.SetLeft(jerry, Canvas.GetLeft(jerry) - speed);
        }
    }
}
