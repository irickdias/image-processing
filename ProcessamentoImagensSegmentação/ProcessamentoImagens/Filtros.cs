using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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

        // OPERADOR GRADIENTE-CRUZADO DE ROBERTS...
        public static void roberts_cross_edge_detection(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b, rx, gx, bx, ry, gy, by;

            for (int y = 0; y < height-1; y++)
            {
                for (int x = 0; x < width-1; x++)
                {
                    //obtendo a cor do pixel
                    Color currentPixel = imageBitmapSrc.GetPixel(x, y);
                    Color rightPixel = imageBitmapSrc.GetPixel(x, y + 1);
                    Color bottomPixel = imageBitmapSrc.GetPixel(x + 1, y);
                    Color rightBottomPixel = imageBitmapSrc.GetPixel(x + 1, y + 1);

                    // valores dos pixeis de 0 a 255, nunca negativos
                    // portanto é como se tivesse feito currentPixel * 1 =  dá ele mesmo... rigthBottomPixel * -1 = valor negativo do mesmo;
                    rx = Math.Abs(currentPixel.R - rightBottomPixel.R);
                    gx = Math.Abs(currentPixel.G - rightBottomPixel.G);
                    bx = Math.Abs(currentPixel.B - rightBottomPixel.B);

                    ry = Math.Abs(rightPixel.R - bottomPixel.R);
                    gy = Math.Abs(rightPixel.G - bottomPixel.G);
                    by = Math.Abs(rightPixel.B - bottomPixel.B);

                    r = Math.Abs(rx +  ry);
                    g = Math.Abs(gx + gy);
                    b = Math.Abs(bx + by);

                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;

                    imageBitmapDest.SetPixel(x, y, Color.FromArgb( r, g, b));
                }
            }
        }

        public static void prewitt_edge_detection(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b, rx, gx, bx, ry, gy, by;
            int[,] vmask = new int[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
            int[,] hmask = new int[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {

                    rx = gx = bx = ry = gy = by = 0;

                    // realiza um somatório dos valores rgb multiplicado separadamente pelas mascaras horizontais e verticais
                    for (int j = -1; j <= 1; j++) // vertical mask
                    {
                        for (int i = -1; i <= 1; i++) // horizontal mask
                        {
                            Color pixel = imageBitmapSrc.GetPixel(x + i, y + j);

                            rx += pixel.R * hmask[j + 1, i + 1];
                            gx += pixel.G * hmask[j + 1, i + 1];
                            bx += pixel.B * hmask[j + 1, i + 1];

                            ry += pixel.R * vmask[j + 1, i + 1];
                            gy += pixel.G * vmask[j + 1, i + 1];
                            by += pixel.B * vmask[j + 1, i + 1];
                        }
                    }

                    r = Math.Abs(rx) + Math.Abs(ry);
                    g = Math.Abs(gx) + Math.Abs(gy);
                    b = Math.Abs(bx) + Math.Abs(by);

                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;

                    imageBitmapDest.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
        }

        public static void sobel_edge_detection(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b, rx, gx, bx, ry, gy, by;
            int[,] vmask = new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            int[,] hmask = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {

                    rx = gx = bx = ry = gy = by = 0;

                    // realiza um somatório dos valores rgb multiplicado separadamente pelas mascaras horizontais e verticais
                    for (int j = -1; j <= 1; j++) // vertical mask
                    {
                        for (int i = -1; i <= 1; i++) // horizontal mask
                        {
                            Color pixel = imageBitmapSrc.GetPixel(x + i, y + j);

                            rx += pixel.R * hmask[j + 1, i + 1];
                            gx += pixel.G * hmask[j + 1, i + 1];
                            bx += pixel.B * hmask[j + 1, i + 1];

                            ry += pixel.R * vmask[j + 1, i + 1];
                            gy += pixel.G * vmask[j + 1, i + 1];
                            by += pixel.B * vmask[j + 1, i + 1];
                        }
                    }

                    r = Math.Abs(rx) + Math.Abs(ry);
                    g = Math.Abs(gx) + Math.Abs(gy);
                    b = Math.Abs(bx) + Math.Abs(by);

                    r = r > 255 ? 255 : r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b < 0 ? 0 : b;

                    imageBitmapDest.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
        }
    }
}
