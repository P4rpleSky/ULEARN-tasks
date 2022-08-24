using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace RefactorMe
{
    class Drawer
    {
        static float x, y;
        static Graphics graphics;

        public static void Initialize( Graphics newGraphics )
        {
            graphics = newGraphics;
            graphics.SmoothingMode = SmoothingMode.None;
            graphics.Clear(Color.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            x = x0; 
            y = y0;
        }

        public static void MakeIt(Pen pen, double length, double deg)
        {
            //Делает шаг длиной length в направлении deg и рисует пройденную траекторию
            var x1 = (float)(x + length * Math.Cos(deg));
            var y1 = (float)(y + length * Math.Sin(deg));
            graphics.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void Change(double length, double deg)
        {
            x = (float)(x + length * Math.Cos(deg)); 
            y = (float)(y + length * Math.Sin(deg));
        }
    }

    public class ImpossibleSquare
    {
        const double Big = 0.375;
        const double Small = 0.04;

        public static void DrawSide1 (double sz)
        {
            Drawer.MakeIt(Pens.Yellow, sz * Big, 0);
            Drawer.MakeIt(Pens.Yellow, sz * Small * Math.Sqrt(2), Math.PI / 4);
            Drawer.MakeIt(Pens.Yellow, sz * Big, Math.PI);
            Drawer.MakeIt(Pens.Yellow, sz * Big - sz * Small, Math.PI / 2);

            Drawer.Change(sz * Small, -Math.PI);
            Drawer.Change(sz * Small * Math.Sqrt(2), 3 * Math.PI / 4);
        }

        public static void DrawSide2(double sz)
        {
            Drawer.MakeIt(Pens.Yellow, sz * Big, -Math.PI / 2);
            Drawer.MakeIt(Pens.Yellow, sz * Small * Math.Sqrt(2), -Math.PI / 2 + Math.PI / 4);
            Drawer.MakeIt(Pens.Yellow, sz * Big, -Math.PI / 2 + Math.PI);
            Drawer.MakeIt(Pens.Yellow, sz * Big - sz * Small, -Math.PI / 2 + Math.PI / 2);

            Drawer.Change(sz * Small, -Math.PI / 2 - Math.PI);
            Drawer.Change(sz * Small * Math.Sqrt(2), -Math.PI / 2 + 3 * Math.PI / 4);
        }

        public static void DrawSide3(double sz)
        {
            Drawer.MakeIt(Pens.Yellow, sz * Big, Math.PI);
            Drawer.MakeIt(Pens.Yellow, sz * Small * Math.Sqrt(2), Math.PI + Math.PI / 4);
            Drawer.MakeIt(Pens.Yellow, sz * Big, Math.PI + Math.PI);
            Drawer.MakeIt(Pens.Yellow, sz * Big - sz * Small, Math.PI + Math.PI / 2);

            Drawer.Change(sz * Small, Math.PI - Math.PI);
            Drawer.Change(sz * Small * Math.Sqrt(2), Math.PI + 3 * Math.PI / 4);
        }

        public static void DrawSide4(double sz)
        {
            Drawer.MakeIt(Pens.Yellow, sz * Big, Math.PI / 2);
            Drawer.MakeIt(Pens.Yellow, sz * Small * Math.Sqrt(2), Math.PI / 2 + Math.PI / 4);
            Drawer.MakeIt(Pens.Yellow, sz * Big, Math.PI / 2 + Math.PI);
            Drawer.MakeIt(Pens.Yellow, sz * Big - sz * Small, Math.PI / 2 + Math.PI / 2);

            Drawer.Change(sz * Small, Math.PI / 2 - Math.PI);
            Drawer.Change(sz * Small * Math.Sqrt(2), Math.PI / 2 + 3 * Math.PI / 4);
        }

        public static void Draw(int width, int height, double degOfRotation, Graphics graphics)
        {
            // degOfRotation пока не используется, но будет использоваться в будущем
            Drawer.Initialize(graphics);

            var sz = Math.Min(width, height);

            var diagonalLength = Math.Sqrt(2) * (sz * Big + sz * Small) / 2;
            var x0 = (float)(diagonalLength * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float)(diagonalLength * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

            Drawer.SetPosition(x0, y0);

            //Рисуем 1-ую сторону
            DrawSide1(sz);
            //Рисуем 2-ую сторону
            DrawSide2(sz);
            //Рисуем 3-ю сторону
            DrawSide3(sz);
            //Рисуем 4-ую сторону
            DrawSide4(sz);
        }
    }
}