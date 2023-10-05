using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ProcessamentoImagens
{
    public partial class frmPrincipal : Form
    {
        private Image image;
        private Bitmap imageBitmap;
        private string fileName;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnAbrirImagem_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                pictBoxImg1.Image = image;
                pictBoxImg1.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pictBoxImg1.Image = null;
            pictBoxImg2.Image = null;
        }

        private void btnLuminanciaSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_gray(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnLuminanciaComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_grayDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoSemDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativo(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoComDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativoDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnFatiarBits_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo("../../../Images/Slicing");
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            //Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.bitplane_slicing(imageBitmap, fileName);
            //pictBoxImg2.Image = imgDest;
            //label1.Text = "Fatiamento do plano de bits realizados com sucesso";
        }

        private void btnFatiarBitsDMA_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo("../../../Images/Slicing");
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            //Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.bitplane_slicing_dma(imageBitmap, fileName);
            //pictBoxImg2.Image = imgDest;
            //label1.Text = "Fatiamento do plano de bits realizados com sucesso";
        }

        private void btnEqualizar_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            int[] h = Filtros.histogram(imageBitmap);
            Filtros.equalization(imageBitmap, imgDest, h);
            pictBoxImg2.Image = imgDest;
        }

        private void btnEqualizarDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            int[] h = Filtros.histogram_dma(imageBitmap);
            Filtros.equalization_dma(imageBitmap, imgDest, h);
            pictBoxImg2.Image = imgDest;
        }

        private void btnSuavizar5x5_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.smoothing5x5(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnSuavizar5x5DMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.smoothing5x5_dma(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnSuavizar5x5Mediana_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.smoothingMean5x5(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnSuavizar5x5MedianaDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.smoothingMean5x5_dma(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnSuavizar5x5KMedia_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.smoothing5x5KMean(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnSuavizar5x5KMediaDMA_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.smoothing5x5KMean_dma(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }
    }
}
