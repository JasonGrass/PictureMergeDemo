using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PicProcessTest.Properties;

namespace PicProcessTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picBoxP.SizeMode = PictureBoxSizeMode.StretchImage;
            picBoxL.SizeMode = PictureBoxSizeMode.StretchImage;
            picBoxS.SizeMode = PictureBoxSizeMode.StretchImage;
            picBoxR.SizeMode = PictureBoxSizeMode.StretchImage;
            picBoxShow.SizeMode = PictureBoxSizeMode.StretchImage;

            picBoxP.Image = GetTrafficImage(TrafficArrowType.E_Pedestrian);
            picBoxL.Image = GetTrafficImage(TrafficArrowType.E_Left);
            picBoxS.Image = GetTrafficImage(TrafficArrowType.E_Straight);
            picBoxR.Image = GetTrafficImage(TrafficArrowType.E_Right);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            picBoxShow.Image = ImagesMerge(false, picBoxP.Image, picBoxL.Image, picBoxS.Image,picBoxR.Image);

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);  


        }

        /// <summary>
        /// 将两张图片的颜色比较叠加
        /// </summary>
        /// <param name="img1">Image1</param>
        /// <param name="img2">Image2</param>
        /// <param name="dark">ture:取深色，false:取浅色</param>
        /// <returns></returns>
        public static Image ImagesMerge(Image img1, Image img2, bool dark)
        {
            int height = img1.Height < img2.Height ? img1.Height : img2.Height;
            int width = img1.Width < img2.Width ? img1.Width : img2.Width;

            Bitmap bmp1 = (Bitmap)img1;
            Bitmap bmp2 = (Bitmap)img2;
            Bitmap bmpMerge = new Bitmap(img1,width,height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color c1 = bmp1.GetPixel(i, j);
                    Color c2 = bmp2.GetPixel(i, j);
                    if (dark)
                    {
                        //值越小，颜色越深
                        bmpMerge.SetPixel(i, j, (c1.R + c1.G + c1.B) < (c1.R + c2.G + c2.B) ? c1 : c2);
                    }
                    else
                    {
                        bmpMerge.SetPixel(i, j, (c1.R + c1.G + c1.B) > (c1.R + c2.G + c2.B) ? c1 : c2);
                    }
                }
            }
            return bmpMerge;
        }

        /// <summary>
        /// 将多张图片的颜色比较叠加
        /// </summary>
        /// <param name="dark">ture:取深色，false:取浅色</param>
        /// <param name="imgs">Image</param>
        /// <returns></returns>
        public static Image ImagesMerge(bool dark, params Image[] imgs)
        {
            if (imgs.Length < 2)
            {
                throw new ArgumentException("参数数量错误");
            }
            List<Image> imgls = imgs.ToList();
            while (imgls.Count > 1)
            {
                Image img = ImagesMerge(imgls[0], imgls[1], dark);
                imgls.RemoveAt(0);
                imgls.RemoveAt(0);
                imgls.Add(img);
            }
            return imgls[0];
        }

        public static Image GetTrafficImage(TrafficArrowType arrowType)
        {
            Image s_we_p = Resources.line;
            Image s_left = Resources.left;
            Image s_straight = Resources.straight;
            Image s_right = Resources.right;
            Image tmp;
            switch (arrowType)
            {
                case TrafficArrowType.S_Pedestrian:
                    return s_we_p;

                case TrafficArrowType.W_Pedestrian:
                    tmp = s_we_p;
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return tmp;

                case TrafficArrowType.N_Pedestrian:
                    tmp = s_we_p;
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return tmp;

                case TrafficArrowType.E_Pedestrian:
                    tmp = s_we_p;
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    return tmp;

                case TrafficArrowType.S_Left:
                    return s_left;

                case TrafficArrowType.S_Straight:
                    return s_straight;

                case TrafficArrowType.S_Right:
                    return s_right;

                case TrafficArrowType.W_Left:
                    tmp = s_left;
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return tmp;

                case TrafficArrowType.W_Straight:
                    tmp = s_straight;
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return tmp;

                case TrafficArrowType.W_Right:
                    tmp = s_right;
                    tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return tmp;

                case TrafficArrowType.N_Left:
                    tmp = s_left;
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return tmp;

                case TrafficArrowType.N_Straight:
                    tmp = s_straight;
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return tmp;

                case TrafficArrowType.N_Right:
                    tmp = s_right;
                    tmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return tmp;

                case TrafficArrowType.E_Left:
                    tmp = s_left;
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    return tmp;

                case TrafficArrowType.E_Straight:
                    tmp = s_straight;
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    return tmp;

                case TrafficArrowType.E_Right:
                    tmp = s_right;
                    tmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    return tmp;

                case TrafficArrowType.Empty:
                case TrafficArrowType.Other:
                    return Resources.Background;
            }
            return Resources.Background;
        }


    }

    public enum TrafficArrowType
    {
        // N S W E : 北 南 西 东
        
        /// <summary>
        /// 南侧东西行人
        /// </summary>
        S_Pedestrian,
        /// <summary>
        /// 北侧东西行人
        /// </summary>
        N_Pedestrian,
        /// <summary>
        /// 西侧南北行人
        /// </summary>
        W_Pedestrian,
        /// <summary>
        /// 东侧南北行人
        /// </summary>
        E_Pedestrian,

        /// <summary>
        /// 南侧左转
        /// </summary>
        S_Left,
        /// <summary>
        /// 南侧直行
        /// </summary>
        S_Straight,
        /// <summary>
        /// 南侧右转
        /// </summary>
        S_Right,

        /// <summary>
        /// 北侧左转
        /// </summary>
        N_Left,
        /// <summary>
        /// 北侧直行
        /// </summary>
        N_Straight,
        /// <summary>
        /// 北侧右转
        /// </summary>
        N_Right,

        /// <summary>
        /// 西侧左转
        /// </summary>
        W_Left,
        /// <summary>
        /// 西侧直行
        /// </summary>
        W_Straight,
        /// <summary>
        /// 西侧右转
        /// </summary>
        W_Right,

        /// <summary>
        /// 东侧左转
        /// </summary>
        E_Left,
        /// <summary>
        /// 东侧直行
        /// </summary>
        E_Straight,
        /// <summary>
        /// 东侧右转
        /// </summary>
        E_Right,

        /// <summary>
        /// 空
        /// </summary>
        Empty,

        /// <summary>
        /// 其它
        /// </summary>
        Other,
    }

}
