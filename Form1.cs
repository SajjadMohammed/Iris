using System;
using System.Drawing;
using System.Windows.Forms;

namespace Iris
{
    public partial class Form1 : Form
    {

        Bitmap bitmap;


        public Form1()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"Bitmap files(*.bmp)|*.bmp|Jpeg files(*.jpg)|*.jpg|Tiff files(*.tif)|*.tif|Png files(*.png)|*.png|All Files(*.jpg;*.bmp;*.tif;*.png)|*.jpg;*.bmp;*.tif;*.png",
                FilterIndex = 5
            };
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            bitmap = (Bitmap)Image.FromFile(openFileDialog.FileName, false);
            bitmap = ImageLab.Load(bitmap);
            originalImage.Image = bitmap;

        }

        private void FirstStageBtn_Click(object sender, EventArgs e)
        {
            firstStageImage.Image = ImageLab.FirstStage(bitmap);
            bitmap = ImageLab.FirstStage(bitmap);
        }

        private void SecondStageBtn_Click(object sender, EventArgs e)
        {
           secondStageImage.Image = bitmap = ImageLab.SecondStage(bitmap);
            
        }

        private void FourthStageBtn_Click(object sender, EventArgs e)
        {
            fourthStageImg.Image = ImageLab.FourthStage(bitmap);
        }
    }
}
