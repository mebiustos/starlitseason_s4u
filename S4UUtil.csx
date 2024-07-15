#r "System.Drawing"
using System.Drawing;

[DllImport("user32.dll")]
static extern IntPtr GetDC(IntPtr hwnd);

[DllImport("user32.dll")]
static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

[DllImport("gdi32.dll")]
static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

static public System.Drawing.Color GetPixelColor(KeyToKey.Plugins.Point point)
{
    return GetPixelColor(point.X, point.Y);
}

static public System.Drawing.Color GetPixelColor(int x, int y)
{
    IntPtr hdc = GetDC(IntPtr.Zero);
    try
    {
        uint pixel = GetPixel(hdc, x, y);
        // ReleaseDC(IntPtr.Zero, hdc);
        Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                    (int)(pixel & 0x0000FF00) >> 8,
                    (int)(pixel & 0x00FF0000) >> 16);
        return color;
    }
    finally
    {
        ReleaseDC(IntPtr.Zero, hdc);
    }
}

static public System.Drawing.Color[] GetPixelColorsA(KeyToKey.Plugins.Point[] points)
{
    System.Drawing.Color[] colors = new System.Drawing.Color[points.Length];
    IntPtr hdc = GetDC(IntPtr.Zero);

    try
    {
        for (int i = 0; i < points.Length; i++)
        {
            uint pixel = GetPixel(hdc, points[i].X, points[i].Y);
            System.Drawing.Color color = System.Drawing.Color.FromArgb(
                (int)(pixel & 0x000000FF),
                (int)(pixel & 0x0000FF00) >> 8,
                (int)(pixel & 0x00FF0000) >> 16);
            colors[i] = color;
        }
    }
    finally
    {
        ReleaseDC(IntPtr.Zero, hdc);
    }

    Console.WriteLine("---");
    for (var i = 0; i < colors.Length; i++)
    {
        Console.WriteLine($"{i}: {colors[i].R} {colors[i].G} {colors[i].B}");
    }
    return colors;
}
