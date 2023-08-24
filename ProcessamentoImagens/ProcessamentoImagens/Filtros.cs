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

        public static void flip_horizontal(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;
            //int r, g, b;

            int half = width / 2;

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < half; x++)
                {
                    Color color = src.GetPixel(x, y);
                    Color inverse = src.GetPixel(width - 1  - x, y);

                    dest.SetPixel(x, y, inverse);
                    dest.SetPixel(width - 1  - x, y, color);
                }
            }
        }

        public static void flip_vertical(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;
            //int r, g, b;

            int half = height / 2;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < half; y++)
                {
                    Color color = src.GetPixel(x, y);
                    Color inverse = src.GetPixel(x, height-1 - y);

                    dest.SetPixel(x, y, inverse);
                    dest.SetPixel(x, height-1 - y, color);
                }
            }
        }

        public static void separate_channels(Bitmap src, Bitmap chnr, Bitmap chng, Bitmap chnb)
        {
            int width = src.Width;
            int height = src.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = src.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    Color channelR = Color.FromArgb(r, 0, 0);
                    Color channelG = Color.FromArgb(0, g, 0);
                    Color channelB = Color.FromArgb(0, 0, b);

                    chnr.SetPixel(x, y, channelR);
                    chng.SetPixel(x, y, channelG);
                    chnb.SetPixel(x, y, channelB);
                }
            }
        }

        public static void invert_red_blue(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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
                    Color newcolor = Color.FromArgb(b, g, r);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        public static void black_white(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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

                    if(gs < 128)
                        imageBitmapDest.SetPixel(x, y, Color.Black);
                    else
                        imageBitmapDest.SetPixel(x, y, Color.White);

                    //nova cor
                    //Color newcolor = Color.FromArgb(gs, gs, gs);


                }
            }
        }

        public static void flip_diagonal(Bitmap src, Bitmap dest)
        {
            Bitmap aux = new Bitmap(src);
            flip_horizontal(src, aux);
            flip_vertical(aux, dest);
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

        public static void flip_horizontal_dma(Bitmap imgSrc, Bitmap imgDest)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int pixelSize = 3;
            //int r, g, b;

            int half = width / 2;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imgSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDest = imgDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int r, g, b;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < half; x++)
                    {
                        b = *(src++);
                        g = *(src++);
                        r = *(src++);


                        //Color inverse = src.GetPixel(width - 1 - x, y);

                        //dest.SetPixel(x, y, inverse);
                        //dest.SetPixel(width - 1 - x, y, color);
                    }
                }
            }

            
        }

        public static void separate_channels_dma(Bitmap imgSrc, Bitmap imgDestR, Bitmap imgDestG, Bitmap imgDestB)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int pixelSize = 3;

            
            BitmapData bitmapDataSrc = imgSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            
            BitmapData bitmapDataDestR = imgDestR.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            
            BitmapData bitmapDataDestG = imgDestG.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            
            BitmapData bitmapDataDestB = imgDestB.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* destR = (byte*)bitmapDataDestR.Scan0.ToPointer();
                byte* destG = (byte*)bitmapDataDestG.Scan0.ToPointer();
                byte* destB = (byte*)bitmapDataDestB.Scan0.ToPointer();
                int r, g, b;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        
                        b = *(src++);
                        g = *(src++);
                        r = *(src++);

                        *(destR++) = (byte)0;
                        *(destR++) = (byte)0;
                        *(destR++) = (byte)r;

                        *(destG++) = (byte)0;
                        *(destG++) = (byte)g;
                        *(destG++) = (byte)0;

                        *(destB++) = (byte)b;
                        *(destB++) = (byte)0;
                        *(destB++) = (byte)0;
                    }
                    src += padding;
                    destR += padding;
                    destG += padding;
                    destB += padding;
                }
            }

            imgSrc.UnlockBits(bitmapDataSrc);
            imgDestR.UnlockBits(bitmapDataDestR);
            imgDestG.UnlockBits(bitmapDataDestG);
            imgDestB.UnlockBits(bitmapDataDestB);
        }

        public static void black_white_dma(Bitmap imgSrc, Bitmap imgDest)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int pixelSize = 3;
            Int32 gs;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imgSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDest = imgDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int r, g, b;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //obtendo a cor do pixel
                        //Color cor = imageBitmapSrc.GetPixel(x, y);

                        b = *(src++);
                        g = *(src++);
                        r = *(src++);
                        gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                        if (gs < 128)
                        {
                            *(dest++) = (byte)0;
                            *(dest++) = (byte)0;
                            *(dest++) = (byte)0;
                        }
                        else
                        {
                            *(dest++) = (byte)255;
                            *(dest++) = (byte)255;
                            *(dest++) = (byte)255;
                        }
                    }

                    src += padding;
                    dest += padding;
                }
            }

            //unlock imagem origem 
            imgSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imgDest.UnlockBits(bitmapDataDest);
        }
    }
}
