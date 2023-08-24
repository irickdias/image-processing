namespace ProcessamentoImagens
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictBoxImg1 = new System.Windows.Forms.PictureBox();
            this.pictBoxImg2 = new System.Windows.Forms.PictureBox();
            this.btnAbrirImagem = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnLuminanciaSemDMA = new System.Windows.Forms.Button();
            this.btnLuminanciaComDMA = new System.Windows.Forms.Button();
            this.btnNegativoComDMA = new System.Windows.Forms.Button();
            this.btnNegativoSemDMA = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBoxRed = new System.Windows.Forms.PictureBox();
            this.pictureBoxNormal = new System.Windows.Forms.PictureBox();
            this.pictureBoxGreen = new System.Windows.Forms.PictureBox();
            this.pictureBoxBlue = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBlue)).BeginInit();
            this.SuspendLayout();
            // 
            // pictBoxImg1
            // 
            this.pictBoxImg1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictBoxImg1.Location = new System.Drawing.Point(5, 6);
            this.pictBoxImg1.Name = "pictBoxImg1";
            this.pictBoxImg1.Size = new System.Drawing.Size(600, 500);
            this.pictBoxImg1.TabIndex = 102;
            this.pictBoxImg1.TabStop = false;
            // 
            // pictBoxImg2
            // 
            this.pictBoxImg2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictBoxImg2.Location = new System.Drawing.Point(610, 6);
            this.pictBoxImg2.Name = "pictBoxImg2";
            this.pictBoxImg2.Size = new System.Drawing.Size(600, 500);
            this.pictBoxImg2.TabIndex = 105;
            this.pictBoxImg2.TabStop = false;
            // 
            // btnAbrirImagem
            // 
            this.btnAbrirImagem.Location = new System.Drawing.Point(5, 512);
            this.btnAbrirImagem.Name = "btnAbrirImagem";
            this.btnAbrirImagem.Size = new System.Drawing.Size(101, 23);
            this.btnAbrirImagem.TabIndex = 106;
            this.btnAbrirImagem.Text = "Abrir Imagem";
            this.btnAbrirImagem.UseVisualStyleBackColor = true;
            this.btnAbrirImagem.Click += new System.EventHandler(this.btnAbrirImagem_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Location = new System.Drawing.Point(5, 541);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(101, 23);
            this.btnLimpar.TabIndex = 107;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnLuminanciaSemDMA
            // 
            this.btnLuminanciaSemDMA.Location = new System.Drawing.Point(112, 512);
            this.btnLuminanciaSemDMA.Name = "btnLuminanciaSemDMA";
            this.btnLuminanciaSemDMA.Size = new System.Drawing.Size(143, 23);
            this.btnLuminanciaSemDMA.TabIndex = 108;
            this.btnLuminanciaSemDMA.Text = "Luminância sem DMA";
            this.btnLuminanciaSemDMA.UseVisualStyleBackColor = true;
            this.btnLuminanciaSemDMA.Click += new System.EventHandler(this.btnLuminanciaSemDMA_Click);
            // 
            // btnLuminanciaComDMA
            // 
            this.btnLuminanciaComDMA.Location = new System.Drawing.Point(112, 541);
            this.btnLuminanciaComDMA.Name = "btnLuminanciaComDMA";
            this.btnLuminanciaComDMA.Size = new System.Drawing.Size(143, 23);
            this.btnLuminanciaComDMA.TabIndex = 109;
            this.btnLuminanciaComDMA.Text = "Luminância com DMA";
            this.btnLuminanciaComDMA.UseVisualStyleBackColor = true;
            this.btnLuminanciaComDMA.Click += new System.EventHandler(this.btnLuminanciaComDMA_Click);
            // 
            // btnNegativoComDMA
            // 
            this.btnNegativoComDMA.Location = new System.Drawing.Point(261, 541);
            this.btnNegativoComDMA.Name = "btnNegativoComDMA";
            this.btnNegativoComDMA.Size = new System.Drawing.Size(130, 23);
            this.btnNegativoComDMA.TabIndex = 111;
            this.btnNegativoComDMA.Text = "Negativo com DMA";
            this.btnNegativoComDMA.UseVisualStyleBackColor = true;
            this.btnNegativoComDMA.Click += new System.EventHandler(this.btnNegativoComDMA_Click);
            // 
            // btnNegativoSemDMA
            // 
            this.btnNegativoSemDMA.Location = new System.Drawing.Point(261, 512);
            this.btnNegativoSemDMA.Name = "btnNegativoSemDMA";
            this.btnNegativoSemDMA.Size = new System.Drawing.Size(130, 23);
            this.btnNegativoSemDMA.TabIndex = 110;
            this.btnNegativoSemDMA.Text = "Negativo sem DMA";
            this.btnNegativoSemDMA.UseVisualStyleBackColor = true;
            this.btnNegativoSemDMA.Click += new System.EventHandler(this.btnNegativoSemDMA_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(396, 512);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 23);
            this.button1.TabIndex = 112;
            this.button1.Text = "Flip Horizontal w/o DMA";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnFlipHorizontalWithoutDMA_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(538, 512);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 23);
            this.button2.TabIndex = 113;
            this.button2.Text = "Flip Vertical w/o DMA";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnFlipVerticalWithoutDMA_Click);
            // 
            // pictureBoxRed
            // 
            this.pictureBoxRed.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxRed.Location = new System.Drawing.Point(611, 267);
            this.pictureBoxRed.Name = "pictureBoxRed";
            this.pictureBoxRed.Size = new System.Drawing.Size(302, 238);
            this.pictureBoxRed.TabIndex = 114;
            this.pictureBoxRed.TabStop = false;
            // 
            // pictureBoxNormal
            // 
            this.pictureBoxNormal.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxNormal.Location = new System.Drawing.Point(610, 6);
            this.pictureBoxNormal.Name = "pictureBoxNormal";
            this.pictureBoxNormal.Size = new System.Drawing.Size(303, 255);
            this.pictureBoxNormal.TabIndex = 115;
            this.pictureBoxNormal.TabStop = false;
            // 
            // pictureBoxGreen
            // 
            this.pictureBoxGreen.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxGreen.Location = new System.Drawing.Point(920, 267);
            this.pictureBoxGreen.Name = "pictureBoxGreen";
            this.pictureBoxGreen.Size = new System.Drawing.Size(291, 238);
            this.pictureBoxGreen.TabIndex = 116;
            this.pictureBoxGreen.TabStop = false;
            // 
            // pictureBoxBlue
            // 
            this.pictureBoxBlue.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxBlue.Location = new System.Drawing.Point(920, 6);
            this.pictureBoxBlue.Name = "pictureBoxBlue";
            this.pictureBoxBlue.Size = new System.Drawing.Size(291, 255);
            this.pictureBoxBlue.TabIndex = 117;
            this.pictureBoxBlue.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(866, 512);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 23);
            this.button3.TabIndex = 118;
            this.button3.Text = "Separate RGB Chn w/o DMA";
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnSeparateChannelsWithoutDMA_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1040, 512);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(170, 23);
            this.button4.TabIndex = 119;
            this.button4.Text = "Invert Red N Blue w/o DMA";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnInvertRedBlueWithoutDMA_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(112, 578);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(170, 23);
            this.button5.TabIndex = 120;
            this.button5.Text = "B And W w/o DMA";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.btnBlackWhiteWithoutDMA_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(396, 539);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(138, 23);
            this.button6.TabIndex = 121;
            this.button6.Text = "Flip Horizontal w/ DMA";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(538, 539);
            this.button7.Margin = new System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(150, 23);
            this.button7.TabIndex = 122;
            this.button7.Text = "Flip Vertical w/ DMA";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(866, 539);
            this.button8.Margin = new System.Windows.Forms.Padding(2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(170, 23);
            this.button8.TabIndex = 123;
            this.button8.Text = "Separate RGB Chn w/ DMA";
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.btnSeparateChannelsWithDMA_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(1040, 539);
            this.button9.Margin = new System.Windows.Forms.Padding(2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(170, 23);
            this.button9.TabIndex = 124;
            this.button9.Text = "Invert Red N Blue w/o DMA";
            this.button9.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(112, 605);
            this.button10.Margin = new System.Windows.Forms.Padding(2);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(170, 23);
            this.button10.TabIndex = 125;
            this.button10.Text = "B And W w/ DMA";
            this.button10.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.btnBlackWhiteWithDMA_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(692, 539);
            this.button11.Margin = new System.Windows.Forms.Padding(2);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(170, 23);
            this.button11.TabIndex = 127;
            this.button11.Text = "Flip Main Diagonal w/ DMA";
            this.button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(692, 512);
            this.button12.Margin = new System.Windows.Forms.Padding(2);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(170, 23);
            this.button12.TabIndex = 126;
            this.button12.Text = "Flip Main Diagonal w/o DMA";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.btnFlipDiagonalWithoutDMA_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 652);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pictureBoxBlue);
            this.Controls.Add(this.pictureBoxGreen);
            this.Controls.Add(this.pictureBoxNormal);
            this.Controls.Add(this.pictureBoxRed);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnNegativoComDMA);
            this.Controls.Add(this.btnNegativoSemDMA);
            this.Controls.Add(this.btnLuminanciaComDMA);
            this.Controls.Add(this.btnLuminanciaSemDMA);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.btnAbrirImagem);
            this.Controls.Add(this.pictBoxImg2);
            this.Controls.Add(this.pictBoxImg1);
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Formulário Principal";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBlue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictBoxImg1;
        private System.Windows.Forms.PictureBox pictBoxImg2;
        private System.Windows.Forms.Button btnAbrirImagem;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnLuminanciaSemDMA;
        private System.Windows.Forms.Button btnLuminanciaComDMA;
        private System.Windows.Forms.Button btnNegativoComDMA;
        private System.Windows.Forms.Button btnNegativoSemDMA;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBoxRed;
        private System.Windows.Forms.PictureBox pictureBoxNormal;
        private System.Windows.Forms.PictureBox pictureBoxGreen;
        private System.Windows.Forms.PictureBox pictureBoxBlue;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
    }
}

