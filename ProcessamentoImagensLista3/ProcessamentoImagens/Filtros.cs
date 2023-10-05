using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting.Messaging;

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
                n.Save("../../../Images/Slicing/" + fileName + "_" + i + ".jpg", ImageFormat.Jpeg);
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

                        var bit = getBit((byte)r, bitPlane);

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

                    //Console.WriteLine("RED:" + color.R);
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

                        hist[r]++;
                        //Console.WriteLine("RED:" + r);

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
            int totalPixels = width * height;
            int[] eq = new int[256];
            int sum = 0;
            //int g = graylevels(hist);
            //int greatest = greatest_gray_level(hist)
            int greatest = 255;
            //Console.WriteLine(g);
            //double l = (src.Width * src.Height) / g;
            //Console.WriteLine(l);
            for(int i=0; i<hist.Length; i++)
            {
                //if (hist[i] != 0)
                //{
                    //Console.WriteLine(hist[i]);
                    sum += hist[i];
                    //Console.WriteLine(sum);

                    //double norm = sum / totalPixels;


                    //int res = (int)Math.Round((sum / l) - 1);
                    //int res = (int)Math.Round(norm * greatest);
                    int res =(int)Math.Round(((double)sum * greatest) / totalPixels);
                    //Console.WriteLine(res);
                    if (res < 0)
                        eq[i] = 0;
                    else
                        eq[i] = res;
                //}
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

        public static void equalization_dma(Bitmap srcImg, Bitmap destImg, int[] hist)
        {
            int width = srcImg.Width;
            int height = srcImg.Height;
            int totalPixels = width * height;
            int pixelSize = 3;

            BitmapData bitmapDataSrc = srcImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDest = destImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                Byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                Byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int r, g, b;
                int[] eq = new int[256];
                int sum = 0;
                int greatest = 255;

                for (int i = 0; i < hist.Length; i++)
                {
                    //if (hist[i] != 0)
                    //{
                        sum += hist[i];
                        int res = (int)Math.Round(((double)sum * greatest) / totalPixels);

                        if (res < 0)
                            eq[i] = 0;
                        else
                            eq[i] = res;
                    //}
                }

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = (*src++);
                        g = (*src++);
                        r = (*src++);

                        
                        (*dest++) = (byte)eq[(byte)r];
                        (*dest++) = (byte)eq[(byte)r];
                        (*dest++) = (byte)eq[(byte)r];
                    }
                    src += padding;
                    dest += padding;
                }
            }
            srcImg.UnlockBits(bitmapDataSrc);
            destImg.UnlockBits(bitmapDataDest);
        }

        private static int greatest_gray_level(int[] hist)
        {
            //int lvl = 0;

            for(int i = hist.Length - 1; i>=0; i--)
            {
                if (hist[i] != 0)
                    return i;
                    
            }

            return 0;
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

        public static void smoothing5x5(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;
            int sum;
            int qtdePixel;
            Color color;
            for (int y = 0; y < height; y++) // variação y=4; y< height-4
            {
                //sum = 0;
                for (int x = 0; x < width; x++) // variação x=4; x< width-4
                {
                    sum = 0;
                    qtdePixel = 0;
                    // 5 pixel of first column
                    /*color = src.GetPixel(x-2, y-2);
                    sum += color.R;
                    color = src.GetPixel(x-1, y-2);
                    sum += color.R;
                    color = src.GetPixel(x, y-2);
                    sum += color.R;
                    color = src.GetPixel(x+1, y-2);
                    sum += color.R;
                    color = src.GetPixel(x+2, y - 2);
                    sum += color.R;


                    color = src.GetPixel(x-2, y-1);
                    sum += color.R;
                    color = src.GetPixel(x-1, y-1);
                    sum += color.R;
                    color = src.GetPixel(x, y-1);
                    sum += color.R;
                    color = src.GetPixel(x+1, y-1);
                    sum += color.R;
                    color = src.GetPixel(x+2, y-1);
                    sum += color.R;


                    color = src.GetPixel(x-2, y); 
                    sum += color.R;
                    color = src.GetPixel(x-1, y);
                    sum += color.R;
                    color = src.GetPixel(x, y);
                    sum += color.R;
                    color = src.GetPixel(x+1, y);
                    sum += color.R;
                    color = src.GetPixel(x+2, y);
                    sum += color.R;


                    color = src.GetPixel(x-2, y+1);
                    sum += color.R;
                    color = src.GetPixel(x-1, y+1);
                    sum += color.R;
                    color = src.GetPixel(x, y+1);
                    sum += color.R;
                    color = src.GetPixel(x+1, y+1);
                    sum += color.R;
                    color = src.GetPixel(x+2, y+1);
                    sum += color.R;


                    color = src.GetPixel(x-2, y+2);
                    sum += color.R;
                    color = src.GetPixel(x-1, y+2);
                    sum += color.R;
                    color = src.GetPixel(x, y+2);
                    sum += color.R;
                    color = src.GetPixel(x+1, y+2);
                    sum += color.R;
                    color = src.GetPixel(x+2, y+2);
                    sum += color.R;*/

                    neighborCumulativeSum(src, x-2, y - 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x-1, y - 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x, y - 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x+1, y - 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x+2, y - 2, ref sum, ref qtdePixel);

                    neighborCumulativeSum(src, x - 2, y - 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x - 1, y - 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x, y - 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 1, y - 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 2, y - 1, ref sum, ref qtdePixel);

                    neighborCumulativeSum(src, x - 2, y, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x - 1, y, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x, y, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 1, y, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 2, y, ref sum, ref qtdePixel);

                    neighborCumulativeSum(src, x - 2, y + 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x - 1, y + 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x, y + 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 1, y + 1, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 2, y + 1, ref sum, ref qtdePixel);

                    neighborCumulativeSum(src, x - 2, y + 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x - 1, y + 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x, y + 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 1, y + 2, ref sum, ref qtdePixel);
                    neighborCumulativeSum(src, x + 2, y + 2, ref sum, ref qtdePixel);

                    int g = sum / qtdePixel;

                   // Console.WriteLine("resultado media: " + g + "sum : " + sum);

                    dest.SetPixel(x, y, Color.FromArgb(g, g, g));

                }
            }

        }

        public static void smoothing5x5_dma(Bitmap srcImg, Bitmap destImg)
        {
            int width = srcImg.Width;
            int height = srcImg.Height;
            int pixelSize = 3;

            BitmapData bitmapDataSrc = srcImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDest = destImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                Byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                Byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int sum;
                int qtdePixel;
                for( int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        sum = 0;
                        qtdePixel = 0;
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 2, y - 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 1, y - 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x, y - 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 1, y - 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 2, y - 2, ref sum, ref qtdePixel);

                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 2, y - 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 1, y - 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x, y - 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 1, y - 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 2, y - 1, ref sum, ref qtdePixel);

                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 2, y, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 1, y, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x, y, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 1, y, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 2, y, ref sum, ref qtdePixel);

                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 2, y + 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 1, y + 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x, y + 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 1, y + 1, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 2, y + 1, ref sum, ref qtdePixel);

                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 2, y + 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x - 1, y + 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x, y + 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 1, y + 2, ref sum, ref qtdePixel);
                        neighborCumulativeSumDMA(src, bitmapDataSrc, pixelSize, x + 2, y + 2, ref sum, ref qtdePixel);

                        int g = sum / qtdePixel;

                        (*dest++) = (byte)g;
                        (*dest++) = (byte)g;
                        (*dest++) = (byte)g;
                    }
                    src += padding;
                    dest += padding;
                }
            }
            srcImg.UnlockBits(bitmapDataSrc);
            destImg.UnlockBits(bitmapDataDest);
        }

        private static void neighborCumulativeSum(Bitmap src, int x, int y, ref int sum, ref int qtdePixel)
        {
            if(x > 0 && x < src.Width && y > 0 && y < src.Height)
            {
                Color color = src.GetPixel(x, y);
                sum += color.R;
                qtdePixel++;
            }
        }

        private static unsafe void neighborCumulativeSumDMA(Byte* src, BitmapData bitmapData, int pixelSize, int x, int y, ref int sum, ref int qtdePixel)
        {
            if (x > 0 && x < bitmapData.Width && y > 0 && y < bitmapData.Height)
            {
                src = (byte*)bitmapData.Scan0 + y * bitmapData.Stride + x * pixelSize;
                int b = (*src++);
                int g = (*src++);
                int r = (*src++);
                sum += r;
                qtdePixel++;
            }
        }

        public static void smoothingMean5x5(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;

            for (int y = 0; y < height; y++) // variação y=4; y< height-4
            {
                
                for (int x = 0; x < width; x++) // variação x=4; x< width-4
                {
                    List<int> values = new List<int>();


                    neighborMean(src, x - 2, y - 2, values);
                    neighborMean(src, x - 1, y - 2, values);
                    neighborMean(src, x, y - 2, values);
                    neighborMean(src, x + 1, y - 2, values);
                    neighborMean(src, x + 2, y - 2, values);

                    neighborMean(src, x - 2, y - 1, values);
                    neighborMean(src, x - 1, y - 1, values);
                    neighborMean(src, x, y - 1, values);
                    neighborMean(src, x + 1, y - 1, values);
                    neighborMean(src, x + 2, y - 1, values);

                    neighborMean(src, x - 2, y, values);
                    neighborMean(src, x - 1, y, values);
                    neighborMean(src, x, y, values);
                    neighborMean(src, x + 1, y, values);
                    neighborMean(src, x + 2, y, values);

                    neighborMean(src, x - 2, y + 1, values);
                    neighborMean(src, x - 1, y + 1, values);
                    neighborMean(src, x, y + 1, values);
                    neighborMean(src, x + 1, y + 1, values);
                    neighborMean(src, x + 2, y + 1, values);

                    neighborMean(src, x - 2, y + 2, values);
                    neighborMean(src, x - 1, y + 2, values);
                    neighborMean(src, x, y + 2, values);
                    neighborMean(src, x + 1, y + 2, values);
                    neighborMean(src, x + 2, y + 2, values);

                    values.Sort();
                    int mean = values[values.Count / 2]; 

                    dest.SetPixel(x, y, Color.FromArgb(mean, mean, mean));

                }
            }
        }

        public static void smoothingMean5x5_dma(Bitmap srcImg, Bitmap destImg)
        {
            int width = srcImg.Width;
            int height = srcImg.Height;
            int pixelSize = 3;

            BitmapData bitmapDataSrc = srcImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDest = destImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                Byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                Byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();

                for (int y = 0; y < height; y++) // variação y=4; y< height-4
                {

                    for (int x = 0; x < width; x++) // variação x=4; x< width-4
                    {
                        List<int> values = new List<int>();


                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y - 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y - 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x, y - 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y - 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y - 2, values);

                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y - 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y - 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x, y - 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y - 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y - 1, values);

                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x, y, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y, values);

                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y + 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y + 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x, y + 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y + 1, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y + 1, values);

                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y + 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y + 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x, y + 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y + 2, values);
                        neighborMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y + 2, values);

                        values.Sort();
                        int mean = values[values.Count / 2];

                        (*dest++) = (byte)mean;
                        (*dest++) = (byte)mean;
                        (*dest++) = (byte)mean;
                    }
                    src += padding;
                    dest += padding;
                }
            }
            srcImg.UnlockBits(bitmapDataSrc);
            destImg.UnlockBits(bitmapDataDest);
        }

        private static void neighborMean(Bitmap src, int x, int y, List<int> values)
        {
            if (x > 0 && x < src.Width && y > 0 && y < src.Height)
            {
                Color color = src.GetPixel(x, y);
                values.Add(color.R);
            }
        }

        private static unsafe void neighborMeanDMA(Byte* src, BitmapData bitmapData, int pixelSize, int x, int y, List<int> values)
        {
            if (x > 0 && x < bitmapData.Width && y > 0 && y < bitmapData.Height)
            {
                src = (byte*)bitmapData.Scan0 + y * bitmapData.Stride + x * pixelSize;
                int b = (*src++);
                int g = (*src++);
                int r = (*src++);
                values.Add(r);
            }
        }

        public static void smoothing5x5KMean(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;
            int k = 9;
            for (int y = 0; y < height; y++) // variação y=4; y< height-4
            {

                for (int x = 0; x < width; x++) // variação x=4; x< width-4
                {
                    List<int> values = new List<int>();


                    neighborKMean(src, x - 2, y - 2, values);
                    neighborKMean(src, x - 1, y - 2, values);
                    neighborKMean(src, x, y - 2, values);
                    neighborKMean(src, x + 1, y - 2, values);
                    neighborKMean(src, x + 2, y - 2, values);

                    neighborKMean(src, x - 2, y - 1, values);
                    neighborKMean(src, x - 1, y - 1, values);
                    neighborKMean(src, x, y - 1, values);
                    neighborKMean(src, x + 1, y - 1, values);
                    neighborKMean(src, x + 2, y - 1, values);

                    neighborKMean(src, x - 2, y, values);
                    neighborKMean(src, x - 1, y, values);
                    neighborKMean(src, x, y, values);
                    neighborKMean(src, x + 1, y, values);
                    neighborKMean(src, x + 2, y, values);

                    neighborKMean(src, x - 2, y + 1, values);
                    neighborKMean(src, x - 1, y + 1, values);
                    neighborKMean(src, x, y + 1, values);
                    neighborKMean(src, x + 1, y + 1, values);
                    neighborKMean(src, x + 2, y + 1, values);

                    neighborKMean(src, x - 2, y + 2, values);
                    neighborKMean(src, x - 1, y + 2, values);
                    neighborKMean(src, x, y + 2, values);
                    neighborKMean(src, x + 1, y + 2, values);
                    neighborKMean(src, x + 2, y + 2, values);

                    values.Sort();

                    int sum = 0;
                    int posMean = values.Count / 2;
                    //int m;

                    
                    for (int i = posMean - k; i < posMean; i++)
                    {
                        sum += values[i];
                        //Console.WriteLine(i);
                    }
                    int m = sum / k;
                    
                    dest.SetPixel(x, y, Color.FromArgb(m, m, m));

                }
            }
        }

        public static void smoothing5x5KMean_dma(Bitmap srcImg, Bitmap destImg)
        {
            int width = srcImg.Width;
            int height = srcImg.Height;
            int pixelSize = 3;

            BitmapData bitmapDataSrc = srcImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bitmapDataDest = destImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                Byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                Byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int k = 9;
                for (int y = 0; y < height; y++) // variação y=4; y< height-4
                {

                    for (int x = 0; x < width; x++) // variação x=4; x< width-4
                    {
                        List<int> values = new List<int>();


                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y - 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y - 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x, y - 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y - 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y - 2, values);

                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y - 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y - 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x, y - 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y - 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y - 1, values);

                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x, y, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y, values);

                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y + 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y + 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x, y + 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y + 1, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y + 1, values);

                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 2, y + 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x - 1, y + 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x, y + 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 1, y + 2, values);
                        neighborKMeanDMA(src, bitmapDataSrc, pixelSize, x + 2, y + 2, values);

                        values.Sort();
                        int mean = values[values.Count / 2];

                        int sum = 0;
                        int posMean = values.Count / 2;
                        //int m;


                        for (int i = posMean - k; i < posMean; i++)
                        {
                            sum += values[i];
                            //Console.WriteLine(i);
                        }
                        int m = sum / k;

                        (*dest++) = (byte)m;
                        (*dest++) = (byte)m;
                        (*dest++) = (byte)m;
                    }
                    src += padding;
                    dest += padding;
                }
            }
            srcImg.UnlockBits(bitmapDataSrc);
            destImg.UnlockBits(bitmapDataDest);
        }

        private static void neighborKMean(Bitmap src, int x, int y, List<int> values)
        {
            if (x > 0 && x < src.Width && y > 0 && y < src.Height)
            {
                Color color = src.GetPixel(x, y);
                values.Add(color.R);
            }
            else
                values.Add(0);
        }

        private static unsafe void neighborKMeanDMA(Byte* src, BitmapData bitmapData, int pixelSize, int x, int y, List<int> values)
        {
            if (x > 0 && x < bitmapData.Width && y > 0 && y < bitmapData.Height)
            {
                src = (byte*)bitmapData.Scan0 + y * bitmapData.Stride + x * pixelSize;
                int b = (*src++);
                int g = (*src++);
                int r = (*src++);
                values.Add(r);
            }
            else
                values.Add(0);
        }

    }
}
