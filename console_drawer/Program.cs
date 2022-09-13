using drawer;
using System.Drawing;

//string imgPath = @".\image\map.png";
//string imgPath = @".\image\Roomba, With Legs.png";
string imgPath = @".\image\blaheeeh.png";

Drawer drawer = new Drawer();

bool success = drawer.Init(120, 60);

if (success)
{
    drawer.ReadImage(imgPath);

    drawer.PrintImage();

    Drawer.WriteAt("blaheeeh", 0, 20);
    Drawer.PrintColorAtPosition("blaheeeh", 255, 0, 0, 105, 20, true, false, false);

    //int dim_x = 60;
    //int dim_y = 60;

    //int cursor_x = 10;
    //int cursor_y = 10;

    //int x_lim = 117;
    //int y_lim = 59;


    ////drawer.DrawPlane(Color.White, dim_x, dim_y);
    //drawer.ReadImage(imgPath);
    //drawer.PrintImage();
    //drawer.AddCursor(Color.Red, cursor_x, cursor_y, x_lim, y_lim);

    //do
    //{
    //    if (Console.ReadKey(true).Key == ConsoleKey.W)
    //    {
    //        //drawer.MoveCursor(Color.Red, Color.White, cursor_x, cursor_y - 1, cursor_x, cursor_y, x_lim, y_lim);
    //        drawer.MoveCursorOnImage(Color.Red, cursor_x, cursor_y - 1, cursor_x, cursor_y, x_lim, y_lim);
    //        cursor_y -= 1;
    //        if (cursor_y < 0)
    //        {
    //            cursor_y = 0;
    //        }
    //    }
    //    if (Console.ReadKey(true).Key == ConsoleKey.S)
    //    {
    //        //drawer.MoveCursor(Color.Red, Color.White, cursor_x, cursor_y + 1, cursor_x, cursor_y, x_lim, y_lim);
    //        drawer.MoveCursorOnImage(Color.Red, cursor_x, cursor_y + 1, cursor_x, cursor_y, x_lim, y_lim);
    //        cursor_y += 1;
    //        if (cursor_y > y_lim)
    //        {
    //            cursor_y = y_lim;
    //        }
    //    }
    //    if (Console.ReadKey(true).Key == ConsoleKey.A)
    //    {
    //        //drawer.MoveCursor(Color.Red, Color.White, cursor_x - 1, cursor_y, cursor_x, cursor_y, x_lim, y_lim);
    //        drawer.MoveCursorOnImage(Color.Red, cursor_x - 1, cursor_y, cursor_x, cursor_y, x_lim, y_lim);
    //        cursor_x -= 1;
    //        if (cursor_x < 0)
    //        {
    //            cursor_x = 0;
    //        }
    //    }
    //    if (Console.ReadKey(true).Key == ConsoleKey.D)
    //    {
    //        //drawer.MoveCursor(Color.Red, Color.White, cursor_x + 1, cursor_y, cursor_x, cursor_y, x_lim, y_lim);
    //        drawer.MoveCursorOnImage(Color.Red, cursor_x + 1, cursor_y, cursor_x, cursor_y, x_lim, y_lim);
    //        cursor_x += 1;
    //        if (cursor_x > x_lim)
    //        {
    //            cursor_x = x_lim;
    //        }
    //    }
    //} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

    Console.ReadLine();
}

