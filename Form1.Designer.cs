namespace Iris
{
    partial class Form1
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
            this.loadBtn = new System.Windows.Forms.Button();
            this.FirstStageBtn = new System.Windows.Forms.Button();
            this.originalImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.firstStageImage = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.secondStageImage = new System.Windows.Forms.PictureBox();
            this.SecondStageBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.fourthStageImg = new System.Windows.Forms.PictureBox();
            this.FourthStageBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstStageImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondStageImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fourthStageImg)).BeginInit();
            this.SuspendLayout();
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(82, 662);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(99, 33);
            this.loadBtn.TabIndex = 0;
            this.loadBtn.Text = "Load Image";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // FirstStageBtn
            // 
            this.FirstStageBtn.Location = new System.Drawing.Point(200, 662);
            this.FirstStageBtn.Name = "FirstStageBtn";
            this.FirstStageBtn.Size = new System.Drawing.Size(99, 33);
            this.FirstStageBtn.TabIndex = 1;
            this.FirstStageBtn.Text = "First Stage";
            this.FirstStageBtn.UseVisualStyleBackColor = true;
            this.FirstStageBtn.Click += new System.EventHandler(this.FirstStageBtn_Click);
            // 
            // originalImage
            // 
            this.originalImage.Location = new System.Drawing.Point(12, 65);
            this.originalImage.Name = "originalImage";
            this.originalImage.Size = new System.Drawing.Size(256, 193);
            this.originalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.originalImage.TabIndex = 2;
            this.originalImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Original Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(309, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "First Stage";
            // 
            // firstStageImage
            // 
            this.firstStageImage.Location = new System.Drawing.Point(312, 65);
            this.firstStageImage.Name = "firstStageImage";
            this.firstStageImage.Size = new System.Drawing.Size(256, 193);
            this.firstStageImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.firstStageImage.TabIndex = 4;
            this.firstStageImage.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(605, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Third Stage";
            // 
            // secondStageImage
            // 
            this.secondStageImage.Location = new System.Drawing.Point(608, 65);
            this.secondStageImage.Name = "secondStageImage";
            this.secondStageImage.Size = new System.Drawing.Size(256, 193);
            this.secondStageImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.secondStageImage.TabIndex = 6;
            this.secondStageImage.TabStop = false;
            // 
            // SecondStageBtn
            // 
            this.SecondStageBtn.Location = new System.Drawing.Point(322, 662);
            this.SecondStageBtn.Name = "SecondStageBtn";
            this.SecondStageBtn.Size = new System.Drawing.Size(123, 33);
            this.SecondStageBtn.TabIndex = 8;
            this.SecondStageBtn.Text = "Third Stage";
            this.SecondStageBtn.UseVisualStyleBackColor = true;
            this.SecondStageBtn.Click += new System.EventHandler(this.SecondStageBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(902, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "Fourth Stage";
            // 
            // fourthStageImg
            // 
            this.fourthStageImg.Location = new System.Drawing.Point(905, 65);
            this.fourthStageImg.Name = "fourthStageImg";
            this.fourthStageImg.Size = new System.Drawing.Size(256, 193);
            this.fourthStageImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fourthStageImg.TabIndex = 9;
            this.fourthStageImg.TabStop = false;
            // 
            // FourthStageBtn
            // 
            this.FourthStageBtn.Location = new System.Drawing.Point(467, 662);
            this.FourthStageBtn.Name = "FourthStageBtn";
            this.FourthStageBtn.Size = new System.Drawing.Size(123, 33);
            this.FourthStageBtn.TabIndex = 11;
            this.FourthStageBtn.Text = "Fourth Stage";
            this.FourthStageBtn.UseVisualStyleBackColor = true;
            this.FourthStageBtn.Click += new System.EventHandler(this.FourthStageBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 732);
            this.Controls.Add(this.FourthStageBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.fourthStageImg);
            this.Controls.Add(this.SecondStageBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.secondStageImage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.firstStageImage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.originalImage);
            this.Controls.Add(this.FirstStageBtn);
            this.Controls.Add(this.loadBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.originalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.firstStageImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondStageImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fourthStageImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.Button FirstStageBtn;
        private System.Windows.Forms.PictureBox originalImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox firstStageImage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox secondStageImage;
        private System.Windows.Forms.Button SecondStageBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox fourthStageImg;
        private System.Windows.Forms.Button FourthStageBtn;
    }
}

