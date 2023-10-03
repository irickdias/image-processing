using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

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

                    if(pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        Ponto newPoint = new Ponto(x, y);
                        (int pos, bool found) = isInSegment(segments, newPoint);
                        if ( !found ) // não encontrou o ponto, não faz parte de nenhum segmento
                        {
                            //Console.WriteLine("passou pela criação do segmento");
                            Segment newSegment = new Segment();
                            newSegment.segment.Add(new Ponto(x, y));
                            segments.Add(newSegment);
                            //pos = 0;
                        }

                        checkLeftPixel(src, segments, x, y-1, width, height, pos);
                        checkRightPixel(src, segments, x, y+1, width, height, pos);
                        checkTopPixel(src, segments, x - 1, y, width, height, pos);
                        checkBottomPixel(src, segments, x + 1, y, width, height, pos);


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
            foreach(var segment in segments)
            {
                //Console.WriteLine("segmento: " + pos);
                r = rand.Next(256);
                g = rand.Next(256);
                b = rand.Next(256);
                Color color = Color.FromArgb(r, g, b);
                foreach(var point in segment.segment)
                {
                    img.SetPixel(point.x, point.y, color);
                }
                pos++;
            }

        }

        private static void checkLeftPixel(Bitmap img, List<Segment> segments, int x, int y, int width, int height, int pos)
        {
            if(y > 0)
            {
                Color pixel = img.GetPixel(x, y);

                if(pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
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

                    if(!found)
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

        private static (int, bool) isInSegment(List<Segment> segments, Ponto point)
        {
            bool found = false;
            int segment_pos = 0;

            foreach(var segment in segments)
            {
                foreach(var ponto in segment.segment)
                {
                    if (ponto.x == point.x && ponto.y == point.y)
                    { 
                        found = true;
                        break;
                    }
                }

                if(found)
                {
                    break;
                }

                segment_pos++;
            }

            return (segment_pos, found);
        }
    }
}
