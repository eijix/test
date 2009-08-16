using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BattleConsole
{
    static class ImageUtility
    {
         /// <summary>
        /// 任意角度旋转
        /// </summary>
        /// <param name="bmp">原始图Bitmap</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="bkColor">背景色</param>
        /// <returns>输出Bitmap</returns>
        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 20;
            int h = bmp.Height + 20;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf =  PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        public static Bitmap Rotate1(Bitmap bmp, float angle)
        {
            TextureBrush MyBrush = new TextureBrush(bmp);
            double h = Math.Sin(angle) * (bmp.Height / 2) + Math.Sin(90 - angle) * (bmp.Height / 2);
            MyBrush.RotateTransform(angle);

            Bitmap dst = new Bitmap((int)(h * 2), (int)(h * 2));
            Graphics g = Graphics.FromImage(dst);
            //g.DrawImageUnscaled(MyBrush.Image, 0, 0, bmp.Width, bmp.Height);
            //g.FillRectangle(MyBrush,0, 0, (int)(h * 2), (int)(h*2));
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TranslateTransform(0,0);
            g.RotateTransform(angle);
            
            g.DrawImage(bmp, 0, 0);
            g.Dispose();

            return dst;

        }

        public static Bitmap Rotate2(Bitmap bmp, float angle, Color bkColor)
        {
            Bitmap dst = new Bitmap(bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(dst);
            g.SmoothingMode = SmoothingMode.HighQuality;

            Rectangle r = new Rectangle(1, 1, bmp.Width, bmp.Height);
            PointF center = new PointF(r.Width / 2, r.Height / 2);
            float offsetX = center.X - bmp.Width / 2;
            float offsetY = center.Y - bmp.Height / 2;

            RectangleF picRect = new RectangleF(offsetX, offsetY, bmp.Width, bmp.Height);
            PointF Pcenter = new PointF(picRect.X + picRect.Width / 2, picRect.Y + picRect.Height / 2);
            g.Clear(bkColor);
            g.TranslateTransform(Pcenter.X, Pcenter.Y);     //变换绘图平面
            g.RotateTransform(angle); //执行旋转
            g.TranslateTransform(-Pcenter.X, -Pcenter.Y);     //恢复绘图平面
            g.DrawImage(bmp, picRect);
            g.ResetTransform();
            g.Dispose();

            return dst;
        }
    }
}
