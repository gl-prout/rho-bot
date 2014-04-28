using Emgu.CV;
using Emgu.CV.Structure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace r_bots
{
    class Webcam
    {
        private Capture capture;
        private HaarCascade haarCascade;
        private PictureBox image1;
        private PictureBox image2;
        Timer timer;

        public Webcam(ref PictureBox imageA, ref PictureBox imageB)
        {
            this.image1 = imageA;
            this.image2 = imageB;
        }

        public void start()
        {
            capture = new Capture();
            haarCascade = new HaarCascade(@"H:\Downloads\trollz\libemgucv-windows-x64-gpu-2.2.1.1150\libemgucv-windows-x64-gpu-2.2.1.1150\bin\haarcascade_frontalface_alt_tree.xml");
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 100;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Image<Bgr, Byte> currentFrame = capture.QueryFrame();
            if (currentFrame != null)
            {
                Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();
                var detectedFaces = grayFrame.DetectHaarCascade(haarCascade)[0];
                foreach (var face in detectedFaces)
                    currentFrame.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
                image1.Image = currentFrame.Bitmap;
            }
        }

        /*[DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap();
                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(ptr);
                return bs;
            }
        }*/
    }
}
