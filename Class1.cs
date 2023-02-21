using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace jump_pathak_jump
{
    public class intersectionObserver
    {

       public  bool observe( Rectangle rect1 , Rectangle rect2 )
        {
          

            Point r1 = new Point(Canvas.GetLeft(rect1) + rect1.Width, Canvas.GetTop(rect1) + rect1.Height);
            Point r2 = new Point(Canvas.GetLeft(rect2) + rect2.Width, Canvas.GetTop(rect2) + rect2.Height);
            Point l1 = new Point(Canvas.GetLeft(rect1), Canvas.GetTop(rect1));
            Point l2 = new Point(Canvas.GetLeft(rect2), Canvas.GetTop(rect2));



            if (l1.X < r2.X && r1.X > l2.X && l1.Y < r2.Y && r1.Y > l2.Y)
            {
                return true;
            }
            return false;


        }
    }
}
