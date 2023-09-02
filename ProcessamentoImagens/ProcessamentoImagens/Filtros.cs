﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;

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

        public static void divide_center(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;

            int half_x = width / 2;
            int half_y = height / 2;

            Bitmap topleft = new Bitmap(half_x, half_y);
            Bitmap topright = new Bitmap(half_x, half_y);
            Bitmap bottomleft = new Bitmap(half_x, half_y);
            Bitmap bottomright = new Bitmap(half_x, half_y);

            for(int y = 0; y < half_y; y++)
            {
                for(int x = 0; x < half_x; x++)
                {
                    //obtendo a cor do pixel
                    Color cor1 = src.GetPixel(x, y);
                    Color cor2 = src.GetPixel(x + half_x, y);
                    Color cor3 = src.GetPixel(x, y + half_y);
                    Color cor4 = src.GetPixel(x + half_x, y + half_y);




                    dest.SetPixel(x, y, cor4);
                    dest.SetPixel(x + half_x , y, cor3);
                    dest.SetPixel(x, y + half_y, cor2);
                    dest.SetPixel(x + half_x, y + half_y, cor1);
                }
            }

        }

        public static void rotate_90(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;
            int inverse_width = height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = src.GetPixel(x, y);


                    dest.SetPixel(inverse_width - 1 - y, x, cor);
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

        public static void flip_horizontal_dma(Bitmap imgSrc, Bitmap imgDest)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int pixelSize = 3;
            //int r, g, b;

            //int half = width / 2;

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
                //byte* srcFinal = src + (width * height);
                byte* srcFinal, startLine;
                byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                byte* destFinal;
                int r, g, b, r2, g2, b2;

                for (int y = 0; y < height; y++)
                {
                    //srcFinal = destFinal = src + width*3 - 1;
                    startLine = src;
                    for (int x = 0; x < width; x++)
                    {

                        // pega as cores do lado esquerdos
                        b = *(src++);
                        g = *(src++);
                        r = *(src++);

                        // coloca ponteiro no lado direito
                        byte* aux = src;
                        src = startLine + width * 3 - 1 - x * 3;

                        //r2 = *(srcFinal--);
                        //g2 = *(srcFinal--);
                        //b2 = *(srcFinal--);


                        // pega as cores do lado direito
                        r2 = *(src--);
                        g2 = *(src--);
                        b2 = *(src--);

                        // volta o ponteiro pro lado esquerdo
                        src = aux;

                        // coloca sas cores pegas do lado direito de src, no lado esquerdo de dest
                        *(dest++) = (byte)b2;
                        *(dest++) = (byte)g2;
                        *(dest++) = (byte)r2;

                        // coloca o ponteiro no lado direito
                        aux = dest;
                        //dest = destFinal;
                        dest = startLine + width * 3 - 1 - x * 3;

                        // coloca sas cores pegas do lado esquerdo de src, no lado direito de dest
                        *(dest--) = (byte)r; //destFinal--;
                        *(dest--) = (byte)g; //destFinal--;
                        *(dest--) = (byte)b; //destFinal--;

                        // volta o ponteiro pro lado esquerdo
                        dest = aux;





                        //Color inverse = src.GetPixel(width - 1 - x, y);

                        //dest.SetPixel(x, y, inverse);
                        //dest.SetPixel(width - 1 - x, y, color);
                    }
                    src +=  padding;
                    dest += padding;
                }
            }
            //unlock imagem origem 
            imgSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imgDest.UnlockBits(bitmapDataDest);

        }

        public static void flip_vertical_dma(Bitmap imgSrc, Bitmap imgDest)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int pixelSize = 3;
            //int r, g, b;

            int half = height / 2;

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
                //byte* srcFinal = src + (width * height);
                byte* srcFinal;
                byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                byte* destFinal;
                int r, g, b, r2, g2, b2;

                for (int y = 0; y < height; y++)
                {
                    srcFinal = destFinal = (byte*)bitmapDataSrc.Scan0.ToPointer() + bitmapDataSrc.Stride * (height - 1 - y);
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src++);
                        g = *(src++);
                        r = *(src++);

                        b2 = *(srcFinal++);
                        g2 = *(srcFinal++);
                        r2 = *(srcFinal++);

                        *(dest++) = (byte)b2;
                        *(dest++) = (byte)g2;
                        *(dest++) = (byte)r2;

                        *(destFinal++) = (byte)b;
                        *(destFinal++) = (byte)g;
                        *(destFinal++) = (byte)r;
                        //byte* aux = dest;
                        //dest = destFinal;

                        //*(dest--) = (byte)r; destFinal--;
                        //*(dest--) = (byte)g; destFinal--;
                        //*(dest--) = (byte)b; destFinal--;

                        //dest = aux;

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

        public static void flip_diagonal_dma(Bitmap imgSrc, Bitmap imgDest)
        {
            Bitmap imgAux = new Bitmap(imgSrc);

            flip_horizontal_dma(imgSrc, imgAux);
            flip_vertical_dma(imgAux, imgDest);
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

        public static void invert_red_blue_dma(Bitmap imgSrc, Bitmap imgDest)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int pixelSize = 3;
            int r, g, b;

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

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        
                        b = *(src++);
                        g = *(src++);
                        r = *(src++);

                        *(dest++) = (byte)r;
                        *(dest++) = (byte)g;
                        *(dest++) = (byte)b;

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

        public static void divide_center_dma(Bitmap imgSrc, Bitmap imgDest)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int pixelSize = 3;

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
                byte* halfPointer = (byte*)(half * bitmapDataSrc.Stride);
                byte* auxSup, auxInf, srcSupHalf, srcInfHalf;
                int r, g, b, r2, g2, b2, r3, g3, b3, r4, g4, b4;

                int halfLine = (bitmapDataSrc.Stride - padding) / 2;
                //srcSupHalf = (byte*)0;
                for (int y = 0; y < height; y++)
                {
                    //int halfLine = (int)src + (bitmapDataSrc.Stride - padding) / 2;
                    for (int x = 0; x < half; x++)
                    {
                        srcSupHalf = src + halfLine;
                        srcInfHalf = halfPointer + halfLine;
                        for (int k=0; k < halfLine; k++)
                        {
                            //obtendo a cor do pixel
                            //Color cor = imageBitmapSrc.GetPixel(x, y);

                            // canto superior esquerdo
                            b = *(src++);
                            g = *(src++);
                            r = *(src++);

                            //; ; aux = (byte*)src + halfLine + k * 3;

                            // canto superior direito
                            b2 = *(srcSupHalf++);
                            g2 = *(srcSupHalf++);
                            r2 = *(srcSupHalf++);

                            // canto inferior esquerdo
                            b3 = *(halfPointer++);
                            g3 = *(halfPointer++);
                            r3 = *(halfPointer++);

                            //aux = (byte*)halfLine + k * 3;

                            // canto inferior direito
                            b4 = *(srcInfHalf++);
                            g4 = *(srcInfHalf++);
                            r4 = *(srcInfHalf++);


                            *(dest++) = (byte)b4;
                            *(dest++) = (byte)g4;
                            *(dest++) = (byte)r4;
                        }
                        
                    }

                    src += halfLine + padding;
                    halfPointer += halfLine + padding;
                    //srcHalf += padding;
                    dest += padding;
                }

            }
        }
    }
}
