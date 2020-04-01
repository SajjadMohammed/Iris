using System;
using System.Drawing;
using System.Windows.Forms;

namespace Iris
{
    public partial class Form1 : Form
    {

        Bitmap bitmap, bitmap2, bitmap3;


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
            bitmap = ImageLab.FirstStage(bitmap);
            // bitmap = ConvertTo8Bpp.CopyToBpp(bitmap, 8);
            firstStageImage.Image = bitmap;
        }

        private void FifthStageBtn_Click(object sender, EventArgs e)
        {
            fifthPictureBox.Image = bitmap = ImageLab.FourthStage(bitmap);
        }

        private void SixthStageBtn_Click(object sender, EventArgs e)
        {
            sixthStagePicture.Image = bitmap = ImageLab.FifthStage(bitmap);
        }

        private void SeventhStageBtn_Click(object sender, EventArgs e)
        {
            seventhStagePIcture.Image = bitmap = ImageLab.SixthStage(bitmap);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            eighthPicutr.Image = bitmap = ImageLab.SeventhStage(bitmap);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = bitmap = ImageLab.EighthStage(bitmap);
        }

        private void SecondStageBtn_Click(object sender, EventArgs e)
        {
            bitmap = ImageLab.SecondStage(bitmap);

            secondStageImage.Image = bitmap;
        }

        private void FourthStageBtn_Click(object sender, EventArgs e)
        {
            fourthStageImg.Image = bitmap = ImageLab.SeedFilling(bitmap, 0, 1);
            fourthStageImg.Image = bitmap = ImageLab.SeedFilling(bitmap, 1, 0);
            fourthStageImg.Image = ImageLab.ThirdStage(bitmap);
        }
    }
}