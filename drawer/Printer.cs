using System.Drawing;
using System.Runtime.InteropServices;

namespace drawer;

public class Drawer
{

// shut up about the sun! shut up about the sun!
#pragma warning disable CA1416 // Validate platform compatibility

    // the following imports let us define whatever colors we want in the console
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleMode(IntPtr handle, out int mode);

    // the following imports are used for setting the console size
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int handle);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();
    private static IntPtr ThisConsole = GetConsoleWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    private const int HIDE = 0;
    private const int MAXIMIZE = 3;
    private const int MINIMIZE = 6;
    private const int RESTORE = 9;


    // an array for an image
    // defaults to 60 x 60 (60 rows, 60 columns) in the console
    // can be changed by passing different values into the init method
    private Color[,] ImageColors = new Color[60,60];

    /// <summary>
    /// Initializes the drawer library by setting the console window size and
    /// the dimensions of the ImageColors array
    /// </summary>
    /// <param name="win_dim_x">The number of columns wide</param>
    /// <param name="win_dim_y">The number of rows</param>
    /// <param name="img_dim_x">The width of the image</param>
    /// <param name="img_dim_y">The height of the image</param>
    /// <returns>True if no exception was thrown.</returns>
    public bool Init(int win_dim_x, int win_dim_y, int img_dim_x = 60, int img_dim_y = 60)
    {
        try
        {
            // set the console size
            Console.SetWindowSize(win_dim_x, win_dim_y);
            //ShowWindow(ThisConsole, MAXIMIZE);

            if(img_dim_x != 60 || img_dim_y != 60)
            {
                ImageColors = new Color[img_dim_x, img_dim_y];
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Reads an image into the ImageColor array.
    /// </summary>
    /// <param name="imagePath">The string to an image to read.</param>
    public void ReadImage(string imagePath)
    {
        Bitmap img = new Bitmap(imagePath);

        // for whatever reason the image is read sideways and flipped, so 
        // we can fix that now instead of later
        img.RotateFlip(RotateFlipType.Rotate270FlipY);

        // if the image is too big, we want to smush it
        if (img.Width > ImageColors.GetLength(0) || img.Height > ImageColors.GetLength(1))
        {
            img = SmushImage(img, ImageColors.GetLength(0), ImageColors.GetLength(1));
        }

        for(int i = 0; i < img.Width; i++)
        {
            for(int j = 0; j < img.Height; j++)
            {
                Color pixel = img.GetPixel(i, j);
                ImageColors[i, j] = pixel;
            }
        }
    }

    /// <summary>
    /// Reads an image into an array of Color objects and returns that array
    /// </summary>
    /// <param name="imagePath">The path to th e image to convert</param>
    /// <param name="x">The x dimension</param>
    /// <param name="y">The y dimension</param>
    /// <returns>A 2D array of Color objects.</returns>
    public Color[,] ReadImage(string imagePath, int x, int y)
    {
        Bitmap img = new Bitmap(imagePath);

        Color[,] colors = new Color[x, y];

        img.RotateFlip(RotateFlipType.Rotate270FlipY);

        if(img.Width > x || img.Height > y)
        {
            img = SmushImage(img, x, y);
        }

        for(int i = 0; i < img.Width; i++)
        {
            for (int j = 0; j < img.Height; j++)
            {
                Color pixel = img.GetPixel(i, j);
                colors[i, j] = pixel;
            }
        }

        return colors;
    }

    /// <summary>
    /// Smushes an image down to an appropriate size.
    /// </summary>
    /// <param name="img">The image to smush.</param>
    /// <param name="width">The width to smush the image to.</param>
    /// <param name="height">The height to smush the image to.</param>
    /// <returns>A smushed image.</returns>
    private static Bitmap SmushImage(Bitmap img, int width, int height)
    {
        Bitmap resized = new Bitmap(width, height);

        using (Graphics g = Graphics.FromImage((Image)resized))
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, 0, 0, width, height);
        }

        return resized;
    }


    /// <summary>
    /// Displays the image held in ImageColors on the console.
    /// </summary>
    public void PrintImage()
    {
        Console.Clear();

        for (int i = 0; i < ImageColors.GetLength(0); i++)
        {
            for(int j = 0; j < ImageColors.GetLength(1); j++)
            {
                if(j != ImageColors.GetLength(1) - 1)
                {
                    try
                    {
                        Print(ImageColors[i, j].R, ImageColors[i, j].G, ImageColors[i, j].B, "  ", false, false, true);
                    }
                    catch
                    {
                        break;
                    }
                }
                else
                {
                    try
                    {
                        Print(ImageColors[i, j].R, ImageColors[i, j].G, ImageColors[i, j].B, "  ", true, false, true);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Displays an image held in an array that is NOT ImageColors on the console.
    /// </summary>
    /// <param name="img">A 2D array of Colors.</param>
    public void PrintImage(Color[,] img)
    {
        for(int i = 0; i < img.GetLength(0); i++)
        {
            for(int j = 0; j < img.GetLength(1); j++)
            {
                if(j != 59)
                {
                    try
                    {
                        Print(img[i, j].R, img[i, j].G, img[i, j].B, "  ", false, false, true);
                    }
                    catch
                    {
                        break;
                    }
                }
                else
                {
                    try
                    {
                        Print(img[i, j].R, img[i, j].G, img[i, j].B, "  ", true, false, true);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Prints a "pixel" of a given color.
    /// Note: If both foreground and background are selected, or neither is selected,
    /// then the background prints.
    /// </summary>
    /// <param name="r">The red value.</param>
    /// <param name="g">The green value.</param>
    /// <param name="b">The blue value.</param>
    /// <param name="text">The text to print.</param>
    /// <param name="newline">Whether the console should print a newline after the "pixel" or not.</param>
    /// <param name="foreground">The "pixel" is printed as a foreground color.</param>
    /// <param name="background">The "pixel" is printed as a background color.</param>
    private static void Print(int r, int g, int b, string? text, bool newline = false, bool foreground = false, bool background = false)
    {
        var handle = GetStdHandle(-11);
        GetConsoleMode(handle, out int mode);
        SetConsoleMode(handle, mode | 0x4);

        string colorText = string.Empty;

        if(foreground && !background) 
        {
            colorText = $"\x1b[38;2;{r};{g};{b}m";
        }
        else if((background && !foreground) || (!foreground && !background) || (foreground && background))
        {
            colorText = $"\x1b[48;2;{r};{g};{b}m";
        }

        if(text == null)
        {
            text = string.Empty;
        }

        colorText += text;

        if(newline)
        {
            Console.WriteLine(colorText);
        }
        else
        {
            Console.Write(colorText);
        }
    }

    /// <summary>
    /// Sets the cursor position.
    /// </summary>
    /// <param name="x">The column to set to.</param>
    /// <param name="y">The row to set to.</param>
    public static void SetPosition(int x, int y)
    {
        Console.SetCursorPosition(x, y);
    }

    /// <summary>
    /// Writes some text at a given cursor position.
    /// </summary>
    /// <param name="s">The string to print</param>
    /// <param name="x">The column to set to.</param>
    /// <param name="y">The row to set to.</param>
    public static void WriteAt(string s, int x, int y)
    {
        try
        {
            Console.SetCursorPosition(x, y);
            Console.Write(s);
        }
        catch(ArgumentOutOfRangeException e)
        {
            Console.Clear();
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Prints a colored line of text at a specific location.
    /// </summary>
    /// <param name="s">The string to print.</param>
    /// <param name="r">The red value.</param>
    /// <param name="g">The green value</param>
    /// <param name="b">The blue value</param>
    /// <param name="x">The column.</param>
    /// <param name="y">The row.</param>
    /// <param name="foreground">RGB are foreground color</param>
    /// <param name="background">RGB are background color</param>
    /// <param name="newline">Print a newline</param>
    public static void PrintColorAtPosition(string s, int r, int g, int b, int x, int y, bool foreground = false, bool background = false, bool newline = false)
    {
        try
        {
            Console.SetCursorPosition(x, y);
            Print(r, g, b, s, newline, foreground, background);
        }
        catch(ArgumentOutOfRangeException e)
        {
            Console.Clear();
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Same as previous, but takes a Color object instead of individual RGB.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="c"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="foreground"></param>
    /// <param name="background"></param>
    /// <param name="newline"></param>
    public static void PrintColorAtPosition(string s, Color c, int x, int y, bool foreground = false, bool background = false, bool newline = false)
    {
        try
        {
            Console.SetCursorPosition(x, y);
            Print(c.R, c.G, c.B, s, newline, foreground, background);
        }
        catch(ArgumentOutOfRangeException e)
        {
            Console.Clear();
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Draws a solid color plane at the given dimensions.
    /// </summary>
    /// <param name="bgd">The color of the plane</param>
    /// <param name="dim_x">The number of columns.</param>
    /// <param name="dim_y">The number of rows.</param>
    public void DrawPlane(Color bgd, int dim_x, int dim_y)
    {
        try
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            Color[,] plane = new Color[dim_x, dim_y];

            for (int i = 0; i < dim_x; i++)
            {
                for (int j = 0; j < dim_y; j++)
                {
                    plane[i, j] = bgd;
                }
            }

            PrintImage(plane);

        }
        catch
        {

        }
    }

    /// <summary>
    /// Places a cursor onto a plane.
    /// </summary>
    /// <param name="cursor">The color of the cursor.</param>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    /// <param name="x_lim">The cursor may not exceed this many columns.</param>
    /// <param name="y_lim">The cursor may not exceed this many rows.</param>
    public void AddCursor(Color cursor, int x, int y, int x_lim, int y_lim)
    {
        try
        {
            if (x > x_lim)
            {
                x = x_lim;
            }
            if(x < 0)
            {
                x = 0;
            }

            if(y > y_lim)
            {
                y = y_lim;
            }
            if(y < 0)
            {
                y = 0;
            }

            Console.SetCursorPosition(x, y);
            Print(cursor.R, cursor.G, cursor.B, "  ", false, false, true);
        }
        catch
        {

        }
    }

    /// <summary>
    /// Moves a cursor on a plain background or with no regard to an image.
    /// </summary>
    /// <param name="cursor">The color of the cursor</param>
    /// <param name="bgd">The color of the background.</param>
    /// <param name="x">The column position.</param>
    /// <param name="y">The row position.</param>
    /// <param name="x_prev">The last column position.</param>
    /// <param name="y_prev">The last row position.</param>
    /// <param name="x_lim">The column limit.</param>
    /// <param name="y_lim">The row limit.</param>
    public void MoveCursor(Color cursor, Color bgd, int x, int y, int x_prev, int y_prev, int x_lim, int y_lim)
    {
        Console.SetCursorPosition(x_prev, y_prev);
        Print(bgd.R, bgd.G, bgd.B, "  ", false, false, true);
        AddCursor(cursor, x, y, x_lim, y_lim);
    }

    /// <summary>
    /// Moves a cursor on a background that cares about the state of the image stored in ImageColors.
    /// </summary>
    /// <param name="cursor">The color of the cursor</param>
    /// <param name="x">The column position.</param>
    /// <param name="y">The row position.</param>
    /// <param name="x_prev">The last column position.</param>
    /// <param name="y_prev">The last row position.</param>
    /// <param name="x_lim">The column limit.</param>
    /// <param name="y_lim">The row limit.</param>
    public void MoveCursorOnImage(Color cursor, int x, int y, int x_prev, int y_prev, int x_lim, int y_lim)
    {
        Console.SetCursorPosition(x_prev, y_prev);
        Print(ImageColors[x_prev, y_prev].R, ImageColors[x_prev, y_prev].G, ImageColors[x_prev, y_prev].B, "  ", false, false, true);
        AddCursor(cursor, x, y, x_lim, y_lim);
    }

    /// <summary>
    /// Moves a cursor on a background that cares abouot the state of a provided image.
    /// </summary>
    /// <param name="cursor">The color of the cursor</param>
    /// <param name="image">An array of colors from an image</param>
    /// <param name="x">The column position</param>
    /// <param name="y">The row position</param>
    /// <param name="x_prev">The last column position</param>
    /// <param name="y_prev">The last row position</param>
    /// <param name="x_lim">The column limit</param>
    /// <param name="y_lim">The row limit</param>
    public void MoveCursorOnImage(Color cursor, Color[,] image, int x, int y , int x_prev, int y_prev, int x_lim, int y_lim)
    {
        Console.SetCursorPosition(x_prev, y_prev);
        Print(image[x_prev, y_prev].R, image[x_prev, y_prev].G, image[x_prev, y_prev].B, "  ", false, false, true);
        AddCursor(cursor, x, y, x_lim, y_lim);
    }

    private void ConvertBlackToOtherColor(Color color)
    {
        for(int i = 0; i < ImageColors.GetLength(0); i++)
        {
            for(int j = 0; j < ImageColors.GetLength(1); j++)
            {
                if(ImageColors[i, j] == Color.Black || (ImageColors[i,j].R == 0 && ImageColors[i,j].G == 0 && ImageColors[i,j].B == 0))
                {
                    ImageColors[i, j] = color;
                }
            }
        }
    }

    private Color[,] ConvertBlackToOtherColor(Color[,] colors, Color color)
    {
        for(int i = 0; i < colors.GetLength(0); i++)
        {
            for(int j = 0; j <= colors.GetLength(1); j++)
            {
                if(colors[i, j] == Color.Black || (colors[i,j].R == 0 && colors[i,j].G == 0 && colors[i, j].B == 0))
                {
                    colors[i, j] = color;
                }
            }
        }

        return colors;
    }

    private void ConvertWhiteToOtherColor(Color color)
    {
        for (int i = 0; i < ImageColors.GetLength(0); i++)
        {
            for (int j = 0; j < ImageColors.GetLength(1); j++)
            {
                if (ImageColors[i, j] == Color.White || (ImageColors[i, j].R == 255 && ImageColors[i, j].G == 255 && ImageColors[i, j].B == 255))
                {
                    ImageColors[i, j] = color;
                }
            }
        }
    }

    private Color[,] ConvertWhiteToOtherColor(Color[,] colors, Color color)
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j <= colors.GetLength(1); j++)
            {
                if (colors[i, j] == Color.White || (colors[i, j].R == 255 && colors[i, j].G == 255 && colors[i, j].B == 255))
                {
                    colors[i, j] = color;
                }
            }
        }

        return colors;
    }

    private void ConvertColorToOtherColor(Color colorFrom, Color colorTo)
    {
        for (int i = 0; i < ImageColors.GetLength(0); i++)
        {
            for (int j = 0; j < ImageColors.GetLength(1); j++)
            {
                if (ImageColors[i, j] == colorFrom)
                {
                    ImageColors[i, j] = colorTo;
                }
            }
        }
    }

    private Color[,] ConverColorToOtherColor(Color[,] colors, Color colorFrom, Color colorTo)
    {
        for (int i = 0; i < colors.GetLength(0); i++)
        {
            for (int j = 0; j <= colors.GetLength(1); j++)
            {
                if (colors[i, j] == colorFrom)
                {
                    colors[i, j] = colorTo;
                }
            }
        }

        return colors;
    }

#pragma warning restore CA1416
}