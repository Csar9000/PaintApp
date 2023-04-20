using System;
using System.Drawing;
using System.Windows.Forms;

namespace PaintApp
{
    public partial class Form1 : Form
    {
        public static double convertValue = 37.7952755905511;
        public static double AD = 2 * convertValue;
        public static double BC = 2 * AD;
        public static int bitmapWidth = 0;
        public static int bitmapHeight = 0;

        public Form1()
        {
            InitializeComponent();

            drawFig(AD, BC, pictureBox1, textBox2);
        }
        public static double Distance(Point a, Point b)
        {
            double dX = a.X - b.X;
            double dY = a.Y - b.Y;
            return Math.Round(Math.Sqrt(dX * dX + dY * dY));
        }

        public static void drawFig(double AD, double BC, PictureBox pictureBox1, TextBox textBox2)
        {
            SolidBrush black = new SolidBrush(Color.Black);
            SolidBrush white = new SolidBrush(Color.White);

            // String[] namesPicks = new String[] { "A", "B", "C", "D", "E" };
            Pen blackPen = new Pen(Color.White, 3);
            Point[] points = new Point[4];// Массив точек трапеции.  
            Point[] triangle = new Point[3];// Массив точек треугольника.


            if (bitmapWidth == 0 && bitmapHeight == 0)
            {
                points[0].X = pictureBox1.Width / 2 - (int)Math.Round(AD, 0); points[0].Y = pictureBox1.Height / 2 - (int)Math.Round(AD, 0) - pictureBox1.Height / 8;
                points[3].X = pictureBox1.Width / 2 + (int)Math.Round(AD, 0); points[3].Y = pictureBox1.Height / 2 - (int)Math.Round(AD, 0) - pictureBox1.Height / 8;

                points[1].X = pictureBox1.Width / 2 - (int)Math.Round(BC, 0); points[1].Y = pictureBox1.Height / 2 + (int)Math.Round(BC, 0) - pictureBox1.Height / 8;
                points[2].X = pictureBox1.Width / 2 + (int)Math.Round(BC, 0); points[2].Y = pictureBox1.Height / 2 + (int)Math.Round(BC, 0) - pictureBox1.Height / 8;
            }
            else
            {
                points[0].X = bitmapWidth / 2 - (int)Math.Round(AD, 0); points[0].Y = bitmapHeight / 2 - (int)Math.Round(AD, 0) - pictureBox1.Height / 8;
                points[3].X = bitmapWidth / 2 + (int)Math.Round(AD, 0); points[3].Y = bitmapHeight / 2 - (int)Math.Round(AD, 0) - pictureBox1.Height / 8;

                points[1].X = bitmapWidth / 2 - (int)Math.Round(BC, 0); points[1].Y = bitmapHeight / 2 + (int)Math.Round(BC, 0) - pictureBox1.Height / 8;
                points[2].X = bitmapWidth / 2 + (int)Math.Round(BC, 0); points[2].Y = bitmapHeight / 2 + (int)Math.Round(BC, 0) - pictureBox1.Height / 8;
            }


            triangle[0].X = points[0].X; triangle[0].Y = points[0].Y;
            triangle[1].X = points[1].X; triangle[1].Y = points[1].Y;
            triangle[2].X = points[0].X; triangle[2].Y = triangle[1].Y;

            // высчитывание заштрихованнной области без эллипса
            double k1 = Distance(triangle[0], triangle[2]) + Distance(triangle[0], triangle[2]) / 1.6;
            double k2 = Distance(triangle[2], triangle[1]) + Distance(triangle[2], triangle[1]) / 1.6;
            double gip = Distance(triangle[0], triangle[1]) + Distance(triangle[0], triangle[1]) / 1.6;

            // параметры заштрихованной области в сантиметрах
            double tempk1 = k1 / convertValue;
            double tempk2 = k2 / convertValue;
            double tempGip = gip / convertValue;

            // площадь треугольника
            double squareTr = Math.Round((tempk1 * tempk2) / 2, 3);
            // нахождение осей эллипса
            double os1 = (tempk1 + tempk2 - tempGip) / 2;
            double os2 = (tempk1 + tempk2 - tempGip);
            // площадь эллипса
            double squareEl = Math.Round(Math.PI * os1 * os2, 3);
            textBox2.Text = Convert.ToString(Math.Round(squareTr - squareEl, 3)) + " см^2";

            // параметры для располдожения эллипса
            int X = triangle[2].X - (int)(k1 + k2 - gip) / 2;
            int Y = triangle[1].Y - (int)(k1 + k2 - gip);
            int width = (int)((k1 + k2 - gip) / 2) - 3;
            int height = (int)(k1 + k2 - gip) - 3;

            // эллипс
            Rectangle rect = new Rectangle(X, Y, width, height);

            // Изображение, которое будем вставлять в PictureBox.
            Bitmap bmp = new Bitmap(pictureBox1.Width / 2 + (int)Distance(points[1], points[3]), pictureBox1.Height / 2 + (int)(Distance(triangle[0], triangle[2]))); // обработать ошибку!!!!

            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                grfx.Clear(Color.White);
                grfx.DrawPolygon(Pens.Black, points);
                grfx.FillPolygon(black, triangle);

                /*for (int i = 0; i < points.Length; i++)
                {
                    grfx.DrawString(namesPicks[i].ToString(), new Font("Arial", 10), Brushes.Red, points[i].X - 20, points[i].Y - 1);
                }*/


                grfx.DrawEllipse(blackPen, rect);
                grfx.FillEllipse(white, rect);
            }

            bitmapWidth = bmp.Width;
            bitmapHeight = bmp.Height;
            // Устанавливаем изображение.
            pictureBox1.Image = bmp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains('.'))
            {
                textBox1.Text = textBox1.Text.Replace('.', ',');
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            if (textBox1.Text.Length != 0)
            {
                if (double.TryParse(textBox1.Text, out double res) == true)
                {
                    label1.Text = "";
                    if (Convert.ToDouble(textBox1.Text) <= 0)
                    {
                        label1.Text = "Необходимо ввести положительное число больше нуля";
                        Graphics g = pictureBox1.CreateGraphics();
                        g.Clear(Color.White);
                        textBox2.Text = "0";
                        AD = 0;
                        BC = 0;
                    }
                    else
                    {
                        label1.Text = "";
                        AD = Convert.ToDouble(textBox1.Text) * convertValue;
                        BC = AD * 2;

                        if (BC > pictureBox1.Width)
                        {
                            label1.Text = "Ширина картинки превышает размер окна.\n" +
                                "Введите значение поменьше";
                        }
                        else
                        {
                            label1.Text = "";
                            drawFig(AD, BC, pictureBox1, textBox2);
                        }

                    }
                }
                else
                {
                    Graphics g = pictureBox1.CreateGraphics();
                    g.Clear(Color.White);
                    textBox2.Text = "0";
                    label1.Text = "Вы ввели нечисловое значение";
                }

            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                if (double.TryParse(textBox1.Text, out double res) == true && double.Parse(textBox1.Text) > 0)
                {
                    if (BC < pictureBox1.Width)
                    {
                        label1.Text = "";
                        drawFig(AD, BC, pictureBox1, textBox2);
                    }
                }
                else
                {
                    textBox2.Text = "0";
                    Graphics g = Graphics.FromImage(pictureBox1.Image);
                    g.Clear(Color.White);
                }
            }

        }
    }
}
