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

        public static void roberts_cross_edge_detection_dma(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            //int r, g, b, rx, gx, bx, ry, gy, by;
            int pixelSize = 3;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDest = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);


            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int r, g, b, rx, gx, bx, ry, gy, by, r1, g1, b1, r2, g2, b2, r3, g3, b3, r4, g4, b4;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        //current
                        src = (byte*)bitmapDataSrc.Scan0 + y * bitmapDataSrc.Stride + x * pixelSize;
                        b1 = *(src++);
                        g1 = *(src++);
                        r1 = *(src++);

                        //right
                        src = (byte*)bitmapDataSrc.Scan0 + (y+1) * bitmapDataSrc.Stride + x * pixelSize;
                        b2 = *(src++);
                        g2 = *(src++);
                        r2 = *(src++);

                        //bottom
                        src = (byte*)bitmapDataSrc.Scan0 + y * bitmapDataSrc.Stride + (x+1) * pixelSize;
                        b3 = *(src++);
                        g3 = *(src++);
                        r3 = *(src++);

                        //right bottom
                        src = (byte*)bitmapDataSrc.Scan0 + (y+1) * bitmapDataSrc.Stride + (x+1) * pixelSize;
                        b4 = *(src++);
                        g4 = *(src++);
                        r4 = *(src++);

                        //obtendo a cor do pixel
                        //Color currentPixel = imageBitmapSrc.GetPixel(x, y);
                        //Color rightPixel = imageBitmapSrc.GetPixel(x, y + 1);
                        //Color bottomPixel = imageBitmapSrc.GetPixel(x + 1, y);
                        //Color rightBottomPixel = imageBitmapSrc.GetPixel(x + 1, y + 1);

                        // valores dos pixeis de 0 a 255, nunca negativos
                        // portanto é como se tivesse feito currentPixel * 1 =  dá ele mesmo... rigthBottomPixel * -1 = valor negativo do mesmo;
                        rx = Math.Abs(r1 - r4);
                        gx = Math.Abs(g1 - g4);
                        bx = Math.Abs(b1 - b4);

                        ry = Math.Abs(r2 - r3);
                        gy = Math.Abs(g2 - g3);
                        by = Math.Abs(b2 - b3);

                        r = Math.Abs(rx + ry);
                        g = Math.Abs(gx + gy);
                        b = Math.Abs(bx + by);

                        r = r > 255 ? 255 : r < 0 ? 0 : r;
                        g = g > 255 ? 255 : g < 0 ? 0 : g;
                        b = b > 255 ? 255 : b < 0 ? 0 : b;

                        //imageBitmapDest.SetPixel(x, y, Color.FromArgb(r, g, b));
                        *(dest++) = (byte)b;
                        *(dest++) = (byte)g;
                        *(dest++) = (byte)r;
                    }
                    //src += padding;
                    dest += padding;
                }
            }
            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDest);

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

        public static void prewitt_edge_detection_dma(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            //int r, g, b, rx, gx, bx, ry, gy, by;
            int[,] vmask = new int[,] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
            int[,] hmask = new int[,] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };

            int pixelSize = 3;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDest = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer() + pixelSize;
                int r, g, b, rx, gx, bx, ry, gy, by, r1, g1, b1;
                for (int y = 1; y < height; y++)
                {
                    for (int x = 1; x < width; x++)
                    {

                        rx = gx = bx = ry = gy = by = 0;

                        // realiza um somatório dos valores rgb multiplicado separadamente pelas mascaras horizontais e verticais
                        for (int j = -1; j <= 1; j++) // vertical mask
                        {
                            for (int i = -1; i <= 1; i++) // horizontal mask
                            {
                                //Color pixel = imageBitmapSrc.GetPixel(x + i, y + j);
                                src = (byte*)bitmapDataSrc.Scan0 + (y+j) * bitmapDataSrc.Stride + (x+i) * pixelSize;
                                b1 = *(src++);
                                g1 = *(src++);
                                r1 = *(src++);

                                rx += r1 * hmask[j + 1, i + 1];
                                gx += g1 * hmask[j + 1, i + 1];
                                bx += b1 * hmask[j + 1, i + 1];

                                ry += r1 * vmask[j + 1, i + 1];
                                gy += g1 * vmask[j + 1, i + 1];
                                by += b1 * vmask[j + 1, i + 1];
                            }
                        }

                        r = Math.Abs(rx) + Math.Abs(ry);
                        g = Math.Abs(gx) + Math.Abs(gy);
                        b = Math.Abs(bx) + Math.Abs(by);

                        r = r > 255 ? 255 : r < 0 ? 0 : r;
                        g = g > 255 ? 255 : g < 0 ? 0 : g;
                        b = b > 255 ? 255 : b < 0 ? 0 : b;

                        //imageBitmapDest.SetPixel(x, y, Color.FromArgb(r, g, b));
                        *(dest++) = (byte)b;
                        *(dest++) = (byte)g;
                        *(dest++) = (byte)r;
                    }
                    dest += padding;
                }
            }
            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDest);


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

        public static void sobel_edge_detection_dma(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            //int r, g, b, rx, gx, bx, ry, gy, by;
            int[,] vmask = new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
            int[,] hmask = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };

            int pixelSize = 3;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDest = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dest = (byte*)bitmapDataDest.Scan0.ToPointer();
                int r, g, b, rx, gx, bx, ry, gy, by, r1, g1, b1, r2, g2, b2, r3, g3, b3, r4, g4, b4;
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
                                //Color pixel = imageBitmapSrc.GetPixel(x + i, y + j);
                                src = (byte*)bitmapDataSrc.Scan0 + (y + j) * bitmapDataSrc.Stride + (x + i) * pixelSize;
                                b1 = *(src++);
                                g1 = *(src++);
                                r1 = *(src++);

                                rx += r1 * hmask[j + 1, i + 1];
                                gx += g1 * hmask[j + 1, i + 1];
                                bx += b1 * hmask[j + 1, i + 1];

                                ry += r1 * vmask[j + 1, i + 1];
                                gy += g1 * vmask[j + 1, i + 1];
                                by += b1 * vmask[j + 1, i + 1];
                            }
                        }

                        r = Math.Abs(rx) + Math.Abs(ry);
                        g = Math.Abs(gx) + Math.Abs(gy);
                        b = Math.Abs(bx) + Math.Abs(by);

                        r = r > 255 ? 255 : r < 0 ? 0 : r;
                        g = g > 255 ? 255 : g < 0 ? 0 : g;
                        b = b > 255 ? 255 : b < 0 ? 0 : b;

                        //imageBitmapDest.SetPixel(x, y, Color.FromArgb(r, g, b));
                        *(dest++) = (byte)b;
                        *(dest++) = (byte)g;
                        *(dest++) = (byte)r;
                    }
                    dest += padding;
                }
            }
            //unlock imagem origem 
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDest);


        }

        private static int background(Color color)
        {
            int c = color.ToArgb() & 1;
            Console.WriteLine("background = " + c);
            return c;
        }

        public static void zhangsuen(Bitmap src, Bitmap dest)
        {
            int width = dest.Width;
            int heigth = dest.Height;

            bool hasThinned = true;
            while (hasThinned)
            {
                hasThinned = false;

                // first interaction
                for (int y = 1; y < heigth - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        // current pixel must be black, to apply the algorith
                        Color currentPixel = dest.GetPixel(x, y);
                        int p = currentPixel.ToArgb() & 1;

                        if (p == 1) // black
                        {
                            // first condition is that the connectivity(white to black) must be only 1

                            // P2 x P3
                            int conn = background(dest.GetPixel(x - 1, y)) == 0 && background(dest.GetPixel(x - 1, y + 1)) == 1 ? 1 : 0;
                            // P3 x P4
                            conn += background(dest.GetPixel(x - 1, y + 1)) == 0 && background(dest.GetPixel(x, y + 1)) == 1 ? 1 : 0;
                            // P4 x P5
                            conn += background(dest.GetPixel(x, y + 1)) == 0 && background(dest.GetPixel(x + 1, y + 1)) == 1 ? 1 : 0;
                            // P5 x P6
                            conn += background(dest.GetPixel(x + 1, y + 1)) == 0 && background(dest.GetPixel(x + 1, y)) == 1 ? 1 : 0;
                            // P6 x P7
                            conn += background(dest.GetPixel(x + 1, y)) == 0 && background(dest.GetPixel(x + 1, y - 1)) == 1 ? 1 : 0;
                            // P7 x P8
                            conn += background(dest.GetPixel(x + 1, y - 1)) == 0 && background(dest.GetPixel(x, y - 1)) == 1 ? 1 : 0;
                            // P8 x P9
                            conn += background(dest.GetPixel(x, y - 1)) == 0 && background(dest.GetPixel(x - 1, y - 1)) == 1 ? 1 : 0;
                            // P9 x P2
                            conn += background(dest.GetPixel(x - 1, y - 1)) == 0 && background(dest.GetPixel(x - 1, y)) == 1 ? 1 : 0;

                            Console.WriteLine("conn " + conn);
                            // condition not true, skip
                            if (conn != 1)
                            {
                                break;
                            }
                            // second condition: at least 2 neighbor is black, no more than 6

                            //P2
                            int neightbors = background(dest.GetPixel(x - 1, y)) == 1 ? 1 : 0;
                            //P3
                            neightbors += background(dest.GetPixel(x - 1, y + 1)) == 1 ? 1 : 0;
                            //P4
                            neightbors += background(dest.GetPixel(x, y + 1)) == 1 ? 1 : 0;
                            //P5
                            neightbors += background(dest.GetPixel(x + 1, y + 1)) == 1 ? 1 : 0;
                            //P6
                            neightbors += background(dest.GetPixel(x + 1, y)) == 1 ? 1 : 0;
                            //P7
                            neightbors += background(dest.GetPixel(x + 1, y - 1)) == 1 ? 1 : 0;
                            //P8
                            neightbors += background(dest.GetPixel(x, y - 1)) == 1 ? 1 : 0;
                            //P9
                            neightbors += background(dest.GetPixel(x - 1, y - 1)) == 1 ? 1 : 0;

                            // condition not true, skip
                            if (neightbors < 2 || neightbors > 6) { 
                                break;
                             }
                            // third condition: at least 1 of the P2, P4 and P8 must be white
                            int multiplier = background(dest.GetPixel(x - 1, y)) * background(dest.GetPixel(x, y + 1)) * background(dest.GetPixel(x, y - 1));

                            // condition not true, skip
                            if (multiplier != 0)
                            {
                                break;
                            }
                            // fourth condition: at least 1 of the P2, P6 and P8 must be white
                            multiplier = background(dest.GetPixel(x - 1, y)) * background(dest.GetPixel(x + 1, y)) * background(dest.GetPixel(x, y - 1));

                            // condition not true, skip
                            if (multiplier != 0) {
                                break;
                            }
                            // if the process arrived here, it means that all four conditions were successfull
                            // then the current pixel is wiped (set to white)

                            hasThinned = true;
                            dest.SetPixel(x, y, Color.White);
                        }
                    }
                }



                // second interaction
                for (int y = 1; y < heigth - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        // current pixel must be black, to apply the algorith
                        Color currentPixel = dest.GetPixel(x, y);
                        int p = currentPixel.ToArgb() & 1;

                        if (p == 1) // black
                        {
                            // first condition is that the connectivity(white to black) must be only 1

                            // P2 x P3
                            int conn = background(dest.GetPixel(x - 1, y)) == 0 && background(dest.GetPixel(x - 1, y + 1)) == 1 ? 1 : 0;
                            // P3 x P4
                            conn += background(dest.GetPixel(x - 1, y + 1)) == 0 && background(dest.GetPixel(x, y + 1)) == 1 ? 1 : 0;
                            // P4 x P5
                            conn += background(dest.GetPixel(x, y + 1)) == 0 && background(dest.GetPixel(x + 1, y + 1)) == 1 ? 1 : 0;
                            // P5 x P6
                            conn += background(dest.GetPixel(x + 1, y + 1)) == 0 && background(dest.GetPixel(x + 1, y)) == 1 ? 1 : 0;
                            // P6 x P7
                            conn += background(dest.GetPixel(x + 1, y)) == 0 && background(dest.GetPixel(x + 1, y - 1)) == 1 ? 1 : 0;
                            // P7 x P8
                            conn += background(dest.GetPixel(x + 1, y - 1)) == 0 && background(dest.GetPixel(x, y - 1)) == 1 ? 1 : 0;
                            // P8 x P9
                            conn += background(dest.GetPixel(x, y - 1)) == 0 && background(dest.GetPixel(x - 1, y - 1)) == 1 ? 1 : 0;
                            // P9 x P2
                            conn += background(dest.GetPixel(x - 1, y - 1)) == 0 && background(dest.GetPixel(x - 1, y)) == 1 ? 1 : 0;

                            // condition not true, skip
                            if (conn != 1)
                            {
                                break;
                            }
                            // second condition: at least 2 neighbor is black, no more than 6

                            //P2
                            int neightbors = background(dest.GetPixel(x - 1, y)) == 1 ? 1 : 0;
                            //P3
                            neightbors += background(dest.GetPixel(x - 1, y + 1)) == 1 ? 1 : 0;
                            //P4
                            neightbors += background(dest.GetPixel(x, y + 1)) == 1 ? 1 : 0;
                            //P5
                            neightbors += background(dest.GetPixel(x + 1, y + 1)) == 1 ? 1 : 0;
                            //P6
                            neightbors += background(dest.GetPixel(x + 1, y)) == 1 ? 1 : 0;
                            //P7
                            neightbors += background(dest.GetPixel(x + 1, y - 1)) == 1 ? 1 : 0;
                            //P8
                            neightbors += background(dest.GetPixel(x, y - 1)) == 1 ? 1 : 0;
                            //P9
                            neightbors += background(dest.GetPixel(x - 1, y - 1)) == 1 ? 1 : 0;

                            // condition not true, skip
                            if (neightbors < 2 || neightbors > 6)
                            {
                                break;
                            }
                            // third condition: at least 1 of the P2, P4 and P6 must be white
                            int multiplier = background(dest.GetPixel(x - 1, y)) * background(dest.GetPixel(x, y + 1)) * background(dest.GetPixel(x + 1, y));

                            // condition not true, skip
                            if (multiplier != 0)
                            { 
                                break;
                            }
                            // fourth condition: at least 1 of the P4, P6 and P8 must be white
                            multiplier = background(dest.GetPixel(x, y + 1)) * background(dest.GetPixel(x + 1, y)) * background(dest.GetPixel(x, y - 1));

                            // condition not true, skip
                            if (multiplier != 0)
                            {
                                break;
                            }

                            // if the process arrived here, it means that all four conditions were successfull
                            // then the current pixel is wiped (set to white)

                            hasThinned = true;
                            dest.SetPixel(x, y, Color.White);
                        }
                    }
                }
            }

            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = dest.GetPixel(x, y);
                    int p1 = pixel.ToArgb() & 1; // Valor do pixel (0 ou 1)

                    if (p1 == 1)
                    {
                        dest.SetPixel(x, y, Color.White); // Remova o pixel
                        //hasChanged = true;
                    }
                }
            }
        }

        public static Bitmap ApplyZhangSuenThinning(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap result = new Bitmap(width, height);

            // Copie a imagem original para a imagem de resultado
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    result.SetPixel(x, y, pixel);
                }
            }

            bool hasChanged = true;
            int iteration = 0;

            while (hasChanged)
            {
                hasChanged = false;

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        Color pixel = result.GetPixel(x, y);
                        int p1 = pixel.ToArgb() & 1; // Valor do pixel (0 ou 1)
                        int[] neighborPixels = new int[8];

                        // Obtenha os valores dos 8 pixels vizinhos
                        neighborPixels[0] = (result.GetPixel(x, y - 1).ToArgb() & 1);
                        neighborPixels[1] = (result.GetPixel(x + 1, y - 1).ToArgb() & 1);
                        neighborPixels[2] = (result.GetPixel(x + 1, y).ToArgb() & 1);
                        neighborPixels[3] = (result.GetPixel(x + 1, y + 1).ToArgb() & 1);
                        neighborPixels[4] = (result.GetPixel(x, y + 1).ToArgb() & 1);
                        neighborPixels[5] = (result.GetPixel(x - 1, y + 1).ToArgb() & 1);
                        neighborPixels[6] = (result.GetPixel(x - 1, y).ToArgb() & 1);
                        neighborPixels[7] = (result.GetPixel(x - 1, y - 1).ToArgb() & 1);

                        int n1 = 0;

                        // Condições para verificar se o pixel pode ser removido
                        if (p1 == 1)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (neighborPixels[i] == 0 && neighborPixels[i + 1] == 1)
                                {
                                    n1++;
                                }
                            }
                            if (neighborPixels[7] == 0 && neighborPixels[0] == 1)
                            {
                                n1++;
                            }

                            int totalTransitions = 0;
                            for (int i = 0; i < 8; i++)
                            {
                                if (neighborPixels[i] == 0 && neighborPixels[i + 1] == 1)
                                {
                                    totalTransitions++;
                                }
                            }

                            if (n1 == 1 && (totalTransitions == 1 || totalTransitions == 2) && !(neighborPixels[0] == 1 && neighborPixels[2] == 1 && neighborPixels[4] == 1))
                            {
                                result.SetPixel(x, y, Color.White); // Remova o pixel
                                hasChanged = true;
                            }
                        }
                    }
                }

                // Segunda iteração
                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        Color pixel = result.GetPixel(x, y);
                        int p1 = pixel.ToArgb() & 1; // Valor do pixel (0 ou 1)
                        int[] neighborPixels = new int[8];

                        // Obtenha os valores dos 8 pixels vizinhos
                        neighborPixels[0] = (result.GetPixel(x, y - 1).ToArgb() & 1);
                        neighborPixels[1] = (result.GetPixel(x + 1, y - 1).ToArgb() & 1);
                        neighborPixels[2] = (result.GetPixel(x + 1, y).ToArgb() & 1);
                        neighborPixels[3] = (result.GetPixel(x + 1, y + 1).ToArgb() & 1);
                        neighborPixels[4] = (result.GetPixel(x, y + 1).ToArgb() & 1);
                        neighborPixels[5] = (result.GetPixel(x - 1, y + 1).ToArgb() & 1);
                        neighborPixels[6] = (result.GetPixel(x - 1, y).ToArgb() & 1);
                        neighborPixels[7] = (result.GetPixel(x - 1, y - 1).ToArgb() & 1);

                        int n1 = 0;

                        // Condições para verificar se o pixel pode ser removido
                        if (p1 == 1)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                if (neighborPixels[i] == 0 && neighborPixels[i + 1] == 1)
                                {
                                    n1++;
                                }
                            }
                            if (neighborPixels[7] == 0 && neighborPixels[0] == 1)
                            {
                                n1++;
                            }

                            int totalTransitions = 0;
                            for (int i = 0; i < 8; i++)
                            {
                                if (neighborPixels[i] == 0 && neighborPixels[i + 1] == 1)
                                {
                                    totalTransitions++;
                                }
                            }

                            if (n1 == 1 && (totalTransitions == 1 || totalTransitions == 2) && !(neighborPixels[0] == 1 && neighborPixels[2] == 1 && neighborPixels[6] == 1))
                            {
                                result.SetPixel(x, y, Color.White); // Remova o pixel
                                hasChanged = true;
                            }
                        }
                    }
                }

                iteration++;

                // Aplique a regra de alternância das iterações
                if (iteration % 2 == 0)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Color pixel = result.GetPixel(x, y);
                            int p1 = pixel.ToArgb() & 1; // Valor do pixel (0 ou 1)

                            if (p1 == 1)
                            {
                                result.SetPixel(x, y, Color.White); // Remova o pixel
                                hasChanged = true;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
