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

        public frmPrincipal()
        {
            InitializeComponent();
            pictureBoxNormal.Visible = false;
            pictureBoxRed.Visible = false;
            pictureBoxGreen.Visible = false;
            pictureBoxBlue.Visible = false;
        }

        private void btnAbrirImagem_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
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
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_gray(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnLuminanciaComDMA_Click(object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.convert_to_grayDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoSemDMA_Click(object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativo(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnNegativoComDMA_Click(object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.negativoDMA(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnFlipHorizontalWithoutDMA_Click(Object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.flip_horizontal(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnFlipVerticalWithoutDMA_Click(Object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.flip_vertical(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnSeparateChannelsWithoutDMA_Click(Object sender, EventArgs e)
        {
            //Bitmap imgDestN = new Bitmap(image);
            Bitmap imgDestR = new Bitmap(image);
            Bitmap imgDestG = new Bitmap(image);
            Bitmap imgDestB = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.separate_channels(imageBitmap, imgDestR, imgDestG, imgDestB);
            
            pictBoxImg2.Visible = false;
            pictureBoxNormal.Visible = true;
            pictureBoxRed.Visible = true;
            pictureBoxGreen.Visible = true;
            pictureBoxBlue.Visible = true;

            pictureBoxNormal.Image = imageBitmap;
            pictureBoxRed.Image = imgDestR;
            pictureBoxGreen.Image = imgDestG;
            pictureBoxBlue.Image = imgDestB;
        }

        private void btnInvertRedBlueWithoutDMA_Click(object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.invert_red_blue(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnBlackWhiteWithoutDMA_Click(object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.black_white(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void btnFlipDiagonalWithoutDMA_Click(Object sender, EventArgs e)
        {
            if (!pictBoxImg2.Visible)
                changePictureBoxesVisibility();

            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.flip_diagonal(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
        }

        private void changePictureBoxesVisibility()
        {
            pictBoxImg2.Visible = true;
            pictureBoxNormal.Visible = false;
            pictureBoxRed.Visible = false;
            pictureBoxGreen.Visible = false;
            pictureBoxBlue.Visible = false;
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
