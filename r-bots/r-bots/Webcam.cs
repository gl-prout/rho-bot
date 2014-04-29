using Emgu.CV;
using Emgu.CV.Structure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace r_bots
{
    class Webcam
    {
        private Capture captureGauche;
        private Capture captureDroite;
        private HaarCascade haarCascade;
        private PictureBox image1;
        private PictureBox image2;
        private RichTextBox console;
        Timer timer;

        public Webcam(ref PictureBox imageA, ref PictureBox imageB, ref RichTextBox msg)
        {
            this.image1 = imageA;
            this.image2 = imageB;
            this.console = msg;
        }

        public void start()
        {
            captureGauche = new Capture(0);
            captureDroite = new Capture(1);
            if(System.Environment.Is64BitProcess) haarCascade = new HaarCascade(@"H:\Downloads\trollz\libemgucv-windows-x64-gpu-2.2.1.1150\libemgucv-windows-x64-gpu-2.2.1.1150\bin\haarcascade_frontalface_alt_tree.xml");
            else haarCascade = new HaarCascade(@"haarcascade_frontalface_alt_tree.xml");
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 75;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            capturer(ref image1,captureGauche);
            capturer(ref image2,captureDroite);
        }

        private void capturer(ref PictureBox image, Capture captures)
        {
            Image<Bgr, Byte> currentFrame = captures.QueryFrame();
            Image<Gray, Byte> currentFrameGray = captures.QueryGrayFrame();
            if (currentFrame != null)
            {
                var detectedFaces = currentFrameGray.DetectHaarCascade(haarCascade)[0];
                foreach (var face in detectedFaces)
                    currentFrame.Draw(face.rect, new Bgr(double.MaxValue, double.MaxValue, double.MaxValue), 1);
                image.BackgroundImage = currentFrame.Bitmap;
            }
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        /*public static BitmapSource ToBitmapSource(IImage image)
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

