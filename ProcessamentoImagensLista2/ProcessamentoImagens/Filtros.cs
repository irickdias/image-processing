using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
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

        public static void spacial_resolution(Bitmap img, Bitmap dest)
        {
            int width = img.Width;
            int height = img.Height;
            //int inverse_width = height;
            Color cor1, cor2, cor3, cor4;
            //int halfx = width / 2;
            //int halfy = height / 2;

            int novox = 0, novoy = 0;
            for (int y = 0; y < height; y += 2)
            {
                novox = 0;
                for (int x = 0; x < width; x += 2)
                {
                    cor1 = img.GetPixel(x, y);
                    cor2 = img.GetPixel(x, y + 1);
                    cor3 = img.GetPixel(x + 1, y);
                    cor4 = img.GetPixel(x + 1, y + 1);

                    int media_r = (cor1.R + cor2.R + cor3.R + cor4.R) / 4;
                    int media_g = (cor1.G + cor2.G + cor3.G + cor4.G) / 4;
                    int media_b = (cor1.B + cor2.B + cor3.B + cor4.B) / 4;

                    Color novo = Color.FromArgb(media_r, media_g, media_b);

                    dest.SetPixel(novox, novoy, novo);
                    novox++;
                }
                novoy++;
            }
        }

        public static void spacial_resolution_dma(Bitmap srcImg, Bitmap destImg)
        {
            int width = srcImg.Width;
            int height = srcImg.Height;
            int pixelSize = 3;

            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = srcImg.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = destImg.LockBits(new Rectangle(0, 0, width/2, height/2),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dest = (byte*)bitmapDataDst.Scan0.ToPointer();
                byte* aux, startline;
                int r1, g1, b1, r2, g2, b2, r3, g3, b3, r4, g4, b4, media_r, media_g, media_b;
                int novox, novoy;
                for(int y = 0; y < height; y += 2)
                {
                    //startline = (byte*)bitmapDataSrc.Scan0 + y * bitmapDataSrc.Stride;
                    //src = startline;
                    for(int x = 0; x < width; x += 2)
                    {
                        //src += x * pixelSize;
                        b1 = (*src++);
                        g1 = (*src++);
                        r1 = (*src++);

                        aux = src;

                        src = (byte*)bitmapDataSrc.Scan0 + y * bitmapDataSrc.Stride + (x + 1) * pixelSize;
                        b2 = (*src++);
                        g2 = (*src++);
                        r2 = (*src++);

                        src = (byte*)bitmapDataSrc.Scan0 + (y + 1) * bitmapDataSrc.Stride + x * pixelSize;
                        b3 = (*src++);
                        g3 = (*src++);
                        r3 = (*src++);

                        src = (byte*)bitmapDataSrc.Scan0 + (y + 1) * bitmapDataSrc.Stride + (x + 1) * pixelSize;
                        b4 = (*src++);
                        g4 = (*src++);
                        r4 = (*src++);

                        media_r = (r1 + r2 + r3 + r4) / 4;
                        media_g = (g1 + g2 + g3 + g4) / 4;
                        media_b = (b1 + b2 + b3 + b4) / 4;

                        (*dest++) = (byte)media_b;
                        (*dest++) = (byte)media_g;
                        (*dest++) = (byte)media_r;

                        src = aux + 3;

                    }
                    src += padding + bitmapDataSrc.Stride;
                    dest += padding;
                }
            }
            // unlock imagem origem
            srcImg.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            destImg.UnlockBits(bitmapDataDst);
        }

        public static void segmento4(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;
            //int inverse_width = height;
            Color pixel;
            //int halfx = width / 2;
            //int halfy = height / 2;
            List<Segment> segments = new List<Segment>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixel = src.GetPixel(x, y);

                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        Ponto newPoint = new Ponto(x, y);
                        (int pos, bool found) = isInSegment(segments, newPoint);
                        if (!found) // não encontrou o ponto, não faz parte de nenhum segmento
                        {
                            //Console.WriteLine("passou pela criação do segmento");
                            Segment newSegment = new Segment();
                            newSegment.segment.Add(new Ponto(x, y));
                            segments.Add(newSegment);
                            //pos = 0;
                        }

                        checkLeftPixel(src, segments, x, y - 1, width, height, pos);
                        checkRightPixel(src, segments, x, y + 1, width, height, pos);
                        checkTopPixel(src, segments, x - 1, y, width, height, pos);
                        checkBottomPixel(src, segments, x + 1, y, width, height, pos);


                    }

                }
            }

            paintSegments(dest, segments);
        }

        public static void segmento4_dma(Bitmap srcImg, Bitmap destImg)
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
                Byte* aux, aux2;
                int r, g, b, r2, g2, b2, coord, colorR, colorG, colorB;
                Color color = new Color();
                Random rand = new Random();
                List<Segment> segments = new List<Segment>();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        // ir para uma coordenada em dma
                        // coord = (y*bitmapDataSrc.Stride) + (x*pixelSize)
                        b = (*src++);
                        g = (*src++);
                        r = (*src++);

                        aux = src; // guarda onde ele está após ler pixel atual

                        b2 = (*dest++);
                        g2 = (*dest++);
                        r2 = (*dest++);

                        aux2 = dest;

                        if (r == 0 && g == 0 && b == 0)
                        {
                            
                            Ponto newPoint = new Ponto(x, y);
                            (int pos, bool found) = isInSegment(segments, newPoint);
                            if(!found)
                            {
                                colorR = rand.Next(256);
                                colorG = rand.Next(256);
                                colorB = rand.Next(256);
                                color = Color.FromArgb(colorR, colorG, colorB);

                                Segment newSegment = new Segment();
                                newSegment.segment.Add(newPoint);
                                segments.Add(newSegment);
                            }

                            if (r2 == 0 && g2 == 0 && b2 == 0) // precisa pintar o pixel atual de outra cor
                            {
                                dest = dest - 3;
                                (*dest++) = (byte)color.B;
                                (*dest++) = (byte)color.G;
                                (*dest++) = (byte)color.R;
                            }

                            checkLeftPixelDMA(src, dest, bitmapDataSrc, bitmapDataDest, segments, x - 1, y, width, height, pos, bitmapDataSrc.Stride, pixelSize, color);
                            checkRightPixelDMA(src, dest, bitmapDataSrc, bitmapDataDest, segments, x + 1, y, width, height, pos, bitmapDataSrc.Stride, pixelSize, color);
                            checkTopPixelDMA(src, dest, bitmapDataSrc, bitmapDataDest, segments, x, y - 1, width, height, pos, bitmapDataSrc.Stride, pixelSize, color);
                            checkBottomPixelDMA(src, dest, bitmapDataSrc, bitmapDataDest, segments, x, y + 1, width, height, pos, bitmapDataSrc.Stride, pixelSize, color);

                            src = aux;
                            dest = aux2;
                        }

                    }
                    src += padding;
                    dest += padding;
                }
            }
            //unlock imagem origem 
            srcImg.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            destImg.UnlockBits(bitmapDataDest);
        }

        public static void segmento8(Bitmap src, Bitmap dest)
        {
            int width = src.Width;
            int height = src.Height;
            //int inverse_width = height;
            Color pixel;
            //int halfx = width / 2;
            //int halfy = height / 2;
            List<Segment> segments = new List<Segment>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixel = src.GetPixel(x, y);

                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        Ponto newPoint = new Ponto(x, y);
                        (int pos, bool found) = isInSegment(segments, newPoint);
                        if (!found) // não encontrou o ponto, não faz parte de nenhum segmento
                        {
                            //Console.WriteLine("passou pela criação do segmento");
                            Segment newSegment = new Segment();
                            newSegment.segment.Add(new Ponto(x, y));
                            segments.Add(newSegment);
                            //pos = 0;
                        }

                        checkLeftPixel(src, segments, x, y - 1, width, height, pos);
                        checkRightPixel(src, segments, x, y + 1, width, height, pos);
                        checkTopPixel(src, segments, x - 1, y, width, height, pos);
                        checkBottomPixel(src, segments, x + 1, y, width, height, pos);
                        checkTopLeftPixel(src, segments, x - 1, y - 1, width, height, pos);
                        checkTopRightPixel(src, segments, x - 1, y + 1, width, height, pos);
                        checkBottomLeftPixel(src, segments, x + 1, y - 1, width, height, pos);
                        checkBottomRightPixel(src, segments, x + 1, y + 1, width, height, pos);


                    }

                }
            }

            paintSegments(dest, segments);
        }



        private static void paintSegments(Bitmap img, List<Segment> segments)
        {
            int r, g, b;
            Random rand = new Random();
            int pos = 0;
            foreach (var segment in segments)
            {
                //Console.WriteLine("segmento: " + pos);
                r = rand.Next(256);
                g = rand.Next(256);
                b = rand.Next(256);
                Color color = Color.FromArgb(r, g, b);
                foreach (var point in segment.segment)
                {
                    img.SetPixel(point.x, point.y, color);
                }
                pos++;
            }

        }

        private static unsafe void checkLeftPixelDMA(Byte* src, Byte* dest, BitmapData bitmapDataSrc, BitmapData bitmapDataDest, List<Segment> segments, int x, int y, int width, int height, int pos, int stride, int pixelSize, Color color)
        {
            if (x > 0)
            {
                
                int r, g, b;
                //int coord = (y * stride) + (x * pixelSize);
                src = (byte*)bitmapDataSrc.Scan0 + y * stride + x * pixelSize;
                //if(*src == null)
                b = (*src++);
                g = (*src++);
                r = (*src++);

                if (r == 0 && g == 0 && b == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                        //dest = (byte*)coord;
                        dest = (byte*)bitmapDataDest.Scan0 + y * stride + x * pixelSize;
                        * dest++ = (byte)color.B;
                        *dest++ = (byte)color.G;
                        *dest++ = (byte)color.R;
                    }

                }

            }

        }

        private static unsafe void checkRightPixelDMA(Byte* src, Byte* dest, BitmapData bitmapDataSrc, BitmapData bitmapDataDest, List<Segment> segments, int x, int y, int width, int height, int pos, int stride, int pixelSize, Color color)
        {
            if (y < height)
            {

                int r, g, b;
                //int coord = (y * stride) + (x * pixelSize);
                //src = (byte*)coord;
                src = (byte*)bitmapDataSrc.Scan0 + y * stride + x * pixelSize;
                b = (*src++);
                g = (*src++);
                r = (*src++);

                if (r == 0 && g == 0 && b == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                        //dest = (byte*)coord;
                        dest = (byte*)bitmapDataDest.Scan0 + y * stride + x * pixelSize;
                        *dest++ = (byte)color.B;
                        *dest++ = (byte)color.G;
                        *dest++ = (byte)color.R;
                    }

                }

            }

        }

        private static unsafe void checkTopPixelDMA(Byte* src, Byte* dest, BitmapData bitmapDataSrc, BitmapData bitmapDataDest, List<Segment> segments, int x, int y, int width, int height, int pos, int stride, int pixelSize, Color color)
        {
            if (x > 0)
            {

                int r, g, b;
                //int coord = (y * stride) + (x * pixelSize);
                //src = (byte*)coord;
                src = (byte*)bitmapDataSrc.Scan0 + y * stride + x * pixelSize;
                b = (*src++);
                g = (*src++);
                r = (*src++);

                if (r == 0 && g == 0 && b == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                        //dest = (byte*)coord;
                        dest = (byte*)bitmapDataDest.Scan0 + y * stride + x * pixelSize;
                        *dest++ = (byte)color.B;
                        *dest++ = (byte)color.G;
                        *dest++ = (byte)color.R;
                    }

                }

            }

        }

        private static unsafe void checkBottomPixelDMA(Byte* src, Byte* dest, BitmapData bitmapDataSrc, BitmapData bitmapDataDest, List<Segment> segments, int x, int y, int width, int height, int pos, int stride, int pixelSize, Color color)
        {
            if (x < width)
            {

                int r, g, b;
                //int coord = (y * stride) + (x * pixelSize);
                //src = (byte*)coord;
                src = (byte*)bitmapDataSrc.Scan0 + y * stride + x * pixelSize;
                b = (*src++);
                g = (*src++);
                r = (*src++);

                if (r == 0 && g == 0 && b == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                        //dest = (byte*)coord;
                        dest = (byte*)bitmapDataDest.Scan0 + y * stride + x * pixelSize;
                        *dest++ = (byte)color.B;
                        *dest++ = (byte)color.G;
                        *dest++ = (byte)color.R;
                    }

                }

            }

        }

        private static void checkLeftPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (y > 0)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static void checkRightPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (y < height)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static void checkTopPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (x > 0)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static void checkBottomPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (x < width)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static void checkTopLeftPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (x > 0 && y > 0)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static void checkTopRightPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (x > 0 && y < height)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static void checkBottomLeftPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (x < width && y > 0)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static void checkBottomRightPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if (x < width && y < height)
            {
                Color pixel = img.GetPixel(x, y);

                if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                {
                    Ponto newPoint = new Ponto(x, y);
                    bool found = false;
                    foreach (var ponto in segments[pos].segment)
                    {
                        if (ponto.x == x && ponto.y == y)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        segments[pos].segment.Add(newPoint);
                    }
                }
            }

        }

        private static (int, bool) isInSegment(List<Segment> segments, Ponto point)
        {
            bool found = false;
            int segment_pos = 0;

            foreach (var segment in segments)
            {
                foreach (var ponto in segment.segment)
                {
                    if (ponto.x == point.x && ponto.y == point.y)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }

                segment_pos++;
            }

            return (segment_pos, found);
        }
    }
}
