using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ProcessamentoImagens
{
    class Filtros
    {
        //sem acesso direto a memoria
        public static void convert_to_gray(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        //sem acesso direito a memoria
        public static void negativo(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    //nova cor
                    Color newcolor = Color.FromArgb(255 - r, 255 - g, 255 - b);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        //com acesso direto a memória
        public static void convert_to_grayDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;
            Int32 gs;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src++); //está armazenado dessa forma: b g r 
                        g = *(src++);
                        r = *(src++);
                        gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);
                        *(dst++) = (byte)gs;
                        *(dst++) = (byte)gs;
                        *(dst++) = (byte)gs;
                    }
                    src += padding;
                    dst += padding;
                }
            }
            //unlock imagem origem
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        //com acesso direito a memoria
        public static void negativoDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src1 = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src1++); //está armazenado dessa forma: b g r 
                        g = *(src1++);
                        r = *(src1++);

                        *(dst++) = (byte)(255 - b);
                        *(dst++) = (byte)(255 - g);
                        *(dst++) = (byte)(255 - r);
                    }
                    src1 += padding;
                    dst += padding;
                }
            }
            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        public static void bitplane_slicing(Bitmap src, string fileName)
        {
            ImageFormat format = src.RawFormat;
            for(int i=0; i < 8; i++)
            {
                Bitmap n = getBitmapPlane(src, i);
                n.Save("../../../Images/Slicing/" + fileName + "_" + i + ".jpg" , ImageFormat.Jpeg);
            }
        }

        public static void bitplane_slicing_dma(Bitmap src, string fileName)
        {
            for(int i=0; i<8; i++)
            {
                Bitmap n = getBitmapPlaneDMA(src, i);
                n.Save("../../../Images/Slicing" + fileName + "-" + i + ".jpg", ImageFormat.Jpeg);
            }
        }

        private static Bitmap getBitmapPlaneDMA(Bitmap srcImg, int bitPlane)
        {
            Bitmap newBitmap = new Bitmap(srcImg);
            int width = srcImg.Width;
            int height = srcImg.Height;
            int pixelSize = 3;

            BitmapData bitmapDataSrc = srcImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDest = newBitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                Byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                Byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int r, g, b;

                for(int y=0; y<height; y++)
                {
                    for(int x=0; x<width; x++)
                    {
                        b = (*src++);
                        g = (*src++);
                        r = (*src++);

                        var bit = getBit((byte)b, bitPlane);

                        (*dest++) = (byte)(255 * bit);
                        (*dest++) = (byte)(255 * bit);
                        (*dest++) = (byte)(255 * bit);
                    }
                    src += padding;
                    dest += padding;
                }
            }
            srcImg.UnlockBits(bitmapDataSrc);
            newBitmap.UnlockBits(bitmapDataDest);

            return newBitmap;
        }

        private static Bitmap getBitmapPlane(Bitmap src, int bitPlane)
        {
            Bitmap newBitmap = new Bitmap(src);
            int width = src.Width;
            int height = src.Height;

            for(int y=0; y<height; y++)
            {
                for(int x=0; x<width; x++)
                {
                    Color color = src.GetPixel(x, y);

                    var bit = getBit(color.R, bitPlane);

                    Color newColor = Color.FromArgb(255 * bit, 255 * bit, 255 * bit);

                    newBitmap.SetPixel(x, y, newColor);
                }
            }

            return newBitmap;
        }

        private static int getBit(byte b, int bitIndex)
        {
            return (b >> bitIndex) & 0x01;
            //return (b & (1 >> bitIndex));
            //return ((b >> bitIndex) & 1) != 0;
        }

        public static int[] histogram(Bitmap srcimg)
        {
            int width = srcimg.Width;
            int height = srcimg.Height;
            int[] hist = new int[256];

            for(int y=0; y<height; y++)
            {
                for(int x = 0; x<width; x++)
                {
                    Color color = srcimg.GetPixel(x, y);

                    hist[color.R]++;


                }
            }

            return hist;
        }

        public static int[] histogram_dma(Bitmap srcimg)
        {
            int width = srcimg.Width;
            int height = srcimg.Height;
            int pixelSize = 3;
            int[] hist = new int[256];

            BitmapData bitmapDataSrc = srcimg.LockBits(new Rectangle(0, 0, width, height), 
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                Byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                int r, g, b;

                for( int y=0; y<height; y++) 
                {
                    for(int x=0; x<width; x++)
                    {
                        b = (*src++);
                        g = (*src++);
                        r = (*src++);

                        hist[b]++;
                    }
                    src += padding;
                }
            }
            srcimg.UnlockBits(bitmapDataSrc);

            return hist;
        }

        public static void equalization(Bitmap src, Bitmap dest, int[] hist)
        {
            int width = src.Width;
            int height = src.Height;
            int[] eq = new int[256];
            int sum = 0;
            int g = graylevels(hist);
            Console.WriteLine(g);
            double l = (src.Width * src.Height) / g;
            Console.WriteLine(l);
            for(int i=0; i<hist.Length; i++)
            {
                if (hist[i] != 0)
                {
                    Console.WriteLine(hist[i]);
                    sum += hist[i];
                    Console.WriteLine(sum);

                    int res = (int)Math.Round((sum / l) - 1);
                    Console.WriteLine(res);
                    if (res < 0)
                        eq[i] = 0;
                    else
                        eq[i] = res;
                }
            }

            for(int y=0; y<height; y++)
            {
                for(int x=0; x<width; x++)
                {
                    Color color = src.GetPixel(x, y);

                    Color c = Color.FromArgb(eq[color.R], eq[color.R], eq[color.R]);
                    dest.SetPixel(x, y, c);
                }
            }
        }

        private static int graylevels(int[] hist)
        {
            int g = 0;

            for(int i=0; i<hist.Length; i++)
            {
                if (hist[i] != 0)
                    g++;
            }

            return g;
        }
    }
}
