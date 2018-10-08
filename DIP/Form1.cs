using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DIP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //my vaiables
        public string fileName = null;
        public Bitmap myBitmap, resultBitmap;
        public int W, H;
        public float[,] convMask = new float[5, 5];
        public float scale, offset;
        public float t1;
        public int t2;

        float[,] redArray, greenArray, blueArray;

        float[,] paddedRed, paddedGreen, paddedBlue, redResult, greenResult, blueResult;



        private void load_Click(object sender, EventArgs e)
        {
            try
            {


/// this part opens an open file dialog, reads the input file and creats an object in from the bitmap
/// class then reads all the values of the image, stores them in redArray, greenArray and blueArray
/// and performs the padding for each plane
/// 
                openFileDialog1.InitialDirectory = "c:\\My Documents";
                openFileDialog1.Filter = "All files (*.*)|*.*";
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((openFileDialog1.OpenFile()) != null)
                    {
                        openFileDialog1.OpenFile();
                        fileName = openFileDialog1.FileName;
                    }
                    else
                    {
                       
                    }
                }
                label1.Text = fileName;
                myBitmap = new Bitmap(fileName);
                resultBitmap = new Bitmap(fileName);
                pictureBox1.BackgroundImage = myBitmap;


                H = myBitmap.Height;
                W = myBitmap.Width;

                redArray = new float[H, W];
                greenArray = new float[H, W];
                blueArray = new float[H, W];

                paddedRed = new float[H + 4, W + 4];
                paddedGreen = new float[H + 4, W + 4];
                paddedBlue = new float[H + 4, W + 4];

                redResult = new float[H, W];
                greenResult = new float[H, W];
                blueResult = new float[H, W];



                Color tmpColor;
                int i, j;


                for (i = 0; i < H; i++)
                {
                    for (j = 0; j < W; j++)
                    {
                        tmpColor = myBitmap.GetPixel(j, i); // changed from (j,i)
                        redArray[i, j] = tmpColor.R;
                        paddedRed[i + 2, j + 2] = tmpColor.R;
                        greenArray[i, j] = tmpColor.G;
                        paddedGreen[i + 2, j + 2] = tmpColor.G;
                        blueArray[i, j] = tmpColor.B;
                        paddedBlue[i + 2, j + 2] = tmpColor.B;
                    }
                }

                for (i = 0; i < H; i++)
                {
                    paddedRed[i, 0] = paddedRed[i, 1] = redArray[i, 0];
                    paddedRed[i, W + 2] = paddedRed[i, W + 3] = redArray[i, W - 1];

                    paddedBlue[i, 0] = paddedBlue[i, 1] = blueArray[i, 0];
                    paddedBlue[i, W + 2] = paddedBlue[i, W + 3] = blueArray[i, W - 1];

                    paddedGreen[i, 1] = paddedGreen[i, 0] = greenArray[i, 0];
                    paddedGreen[i, W + 2] = paddedGreen[i, W + 3] = greenArray[i, W - 1];
                }

                for (j = 0; j < W; j++)
                {
                    paddedRed[0, j] = paddedRed[1, j] = redArray[0, j];
                    paddedRed[H + 2, j] = paddedRed[H + 3, j] = redArray[H - 1, j]; // changed from W+2/W+3 to H+2/H+3

                    paddedBlue[0, j] = paddedBlue[1, j] = blueArray[0, j];
                    paddedBlue[H + 2, j] = paddedBlue[H + 3, j] = blueArray[H - 1, j];

                    paddedGreen[0, j] = paddedGreen[1, j] = greenArray[0, j];
                    paddedGreen[H + 2, j] = paddedGreen[H + 3, j] = greenArray[H - 1, j];

                }

                paddedRed[0, 0] = paddedRed[0, 1] = paddedRed[1, 0] = paddedRed[1, 1] = redArray[0, 0];
                paddedRed[0, W - 1] = paddedRed[0, W - 2] = paddedRed[1, W - 1] = paddedRed[1, W - 2] = redArray[0, W - 1];
                paddedRed[H - 2, W - 2] = paddedRed[H - 2, W - 1] = paddedRed[H - 1, W - 2] = paddedRed[H - 1, W - 1] = redArray[H - 1, W - 1];
                paddedRed[H - 1, 0] = paddedRed[H - 1, 1] = paddedRed[H - 2, 1] = paddedRed[H - 2, 0] = redArray[H - 1, 0];

                paddedGreen[0, 0] = paddedGreen[0, 1] = paddedGreen[1, 0] = paddedGreen[1, 1] = greenArray[0, 0];
                paddedGreen[0, W - 1] = paddedGreen[0, W - 2] = paddedGreen[1, W - 1] = paddedGreen[1, W - 2] = greenArray[0, W - 1];
                paddedGreen[H - 2, W - 2] = paddedGreen[H - 2, W - 1] = paddedGreen[H - 1, W - 2] = paddedGreen[H - 1, W - 1] = greenArray[H - 1, W - 1];
                paddedGreen[H - 1, 0] = paddedGreen[H - 1, 1] = paddedGreen[H - 2, 1] = paddedGreen[H - 2, 0] = greenArray[H - 1, 0];


                paddedBlue[0, 0] = paddedBlue[0, 1] = paddedBlue[1, 0] = paddedBlue[1, 1] = blueArray[0, 0];
                paddedBlue[0, W - 1] = paddedBlue[0, W - 2] = paddedBlue[1, W - 1] = paddedBlue[1, W - 2] = blueArray[0, W - 1];
                paddedBlue[H - 2, W - 2] = paddedBlue[H - 2, W - 1] = paddedBlue[H - 1, W - 2] = paddedBlue[H - 1, W - 1] = blueArray[H - 1, W - 1];
                paddedBlue[H - 1, 0] = paddedBlue[H - 1, 1] = paddedBlue[H - 2, 1] = paddedBlue[H - 2, 0] = blueArray[H - 1, 0];
            }
            catch (Exception expec)
            {
                MessageBox.Show("Image incompatible", "Check Please");
            }
        }



///
       /// in this part, values of the convolution mask are obtained , and convolution is performed.
        private void preview_Click(object sender, EventArgs e)
        {
            try
            {


                resultBitmap = new Bitmap(W, H);
                float redMin = 10000;
                float greenMin = 10000;
                float blueMin = 10000;
                float redMax = -10000;
                float greenMax = -10000;
                float blueMax = -10000;

                int i, j, t, k;
                float redSum = 0;
                float greenSum = 0;
                float blueSum = 0;


                ///
                /// 
                /// reading the values of the convoution mask



                convMask[0, 0] = (float)(textBox1.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox1.Text));
                convMask[0, 1] = (float)(textBox2.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox2.Text));
                convMask[0, 2] = (float)(textBox3.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox3.Text));
                convMask[0, 3] = (float)(textBox4.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox4.Text));
                convMask[0, 4] = (float)(textBox5.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox5.Text));

                convMask[1, 0] = (float)(textBox6.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox6.Text));
                convMask[1, 1] = (float)(textBox7.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox7.Text));
                convMask[1, 2] = (float)(textBox8.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox8.Text));
                convMask[1, 3] = (float)(textBox9.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox9.Text));
                convMask[1, 4] = (float)(textBox10.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox10.Text));

                convMask[2, 0] = (float)(textBox11.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox11.Text));
                convMask[2, 1] = (float)(textBox12.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox12.Text));
                convMask[2, 2] = (float)(textBox13.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox13.Text));
                convMask[2, 3] = (float)(textBox14.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox14.Text));
                convMask[2, 4] = (float)(textBox15.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox15.Text));

                convMask[3, 0] = (float)(textBox16.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox16.Text));
                convMask[3, 1] = (float)(textBox17.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox17.Text));
                convMask[3, 2] = (float)(textBox18.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox18.Text));
                convMask[3, 3] = (float)(textBox19.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox19.Text));
                convMask[3, 4] = (float)(textBox20.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox20.Text));

                convMask[4, 0] = (float)(textBox21.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox21.Text));
                convMask[4, 1] = (float)(textBox22.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox22.Text));
                convMask[4, 2] = (float)(textBox23.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox23.Text));
                convMask[4, 3] = (float)(textBox24.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox24.Text));
                convMask[4, 4] = (float)(textBox25.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox25.Text));


                scale = (float)(textBox26.Text.Trim() == "" ? 1 : Convert.ToDouble(textBox26.Text));
                offset = (float)(textBox27.Text.Trim() == "" ? 0 : Convert.ToDouble(textBox27.Text));

                //
                ///
                ///
                ////












                

                //multiplying convoloution mask

                //**********************************************************************************************//

                if (checkBoxRed.Checked == true)
                {
                    for (i = 0; i < H; i++)
                    {
                        for (j = 0; j < W; j++)
                        {
                            for (t = 0; t < 5; t++)
                            {
                                for (k = 0; k < 5; k++)
                                {

                                    redSum = redSum + convMask[t, k] * paddedRed[i + t, j + k];

                                }
                            }

                            redResult[i, j] = (int)((float)redSum / (float)scale) + offset;

                            redSum = 0;

                            if (redResult[i, j] > redMax)
                            {
                                redMax = redResult[i, j];
                            }
                            if (redResult[i, j] < redMin)
                            {
                                redMin = redResult[i, j];
                            }


                        }
                    }
                }
                else
                {
                    for (i = 0; i < H; i++)
                    {
                        for (j = 0; j < W; j++)
                        {
                            redResult[i, j] = redArray[i, j];
                        }
                    }
                }


                if (checkBoxGreen.Checked == true)
                {
                    for (i = 0; i < H; i++)
                    {
                        for (j = 0; j < W; j++)
                        {
                            for (t = 0; t < 5; t++)
                            {
                                for (k = 0; k < 5; k++)
                                {


                                    greenSum = greenSum + convMask[t, k] * paddedGreen[i + t, j + k];




                                }
                            }


                            greenResult[i, j] = (int)((float)greenSum / (float)scale) + offset;


                            greenSum = 0;
                            if (greenResult[i, j] > greenMax)
                            {
                                greenMax = greenResult[i, j];
                            }
                            if (greenResult[i, j] < greenMin)
                            {
                                greenMin = greenResult[i, j];
                            }


                        }
                    }
                }
                else
                {
                    for (i = 0; i < H; i++)
                    {
                        for (j = 0; j < W; j++)
                        {
                            greenResult[i, j] = greenArray[i, j];
                        }
                    }
                }


                if (checkBoxBlue.Checked == true)
                {
                    for (i = 0; i < H; i++)
                    {
                        for (j = 0; j < W; j++)
                        {
                            for (t = 0; t < 5; t++)
                            {
                                for (k = 0; k < 5; k++)
                                {


                                    blueSum = blueSum + convMask[t, k] * paddedBlue[i + t, j + k];



                                }
                            }


                            blueResult[i, j] = (int)((float)blueSum / (float)scale) + offset;



                            blueSum = 0;
                            if (blueResult[i, j] > blueMax)
                            {
                                blueMax = blueResult[i, j];
                            }
                            if (blueResult[i, j] < blueMin)
                            {
                                blueMin = blueResult[i, j];
                            }

                        }
                    }
                }
                else
                {

                    for (i = 0; i < H; i++)
                    {
                        for (j = 0; j < W; j++)
                        {
                            blueResult[i, j] = blueArray[i, j];
                        }
                    }
                }

                //******************************************************************************************//
                
                /// preparing the data for display (mapping)

                for (i = 0; i < H; i++)
                {
                    for (j = 0; j < W; j++)
                    {


                        if (checkBoxRed.Checked == true)
                        {
                            if (redMax != redMin)
                                redResult[i, j] = ((int)((float)(redResult[i, j] - redMin) * 255 / (float)(redMax - redMin)));
                            else
                                redResult[i, j] = 0;
                        }
                        if (checkBoxGreen.Checked == true)
                        {
                            if (greenMax != greenMin)
                                greenResult[i, j] = ((int)((float)(greenResult[i, j] - greenMin) * 255 / (float)(greenMax - greenMin)));
                            else
                                greenResult[i, j] = 0;
                        }
                        if (checkBoxBlue.Checked == true)
                        {
                            if (blueMax != blueMin)
                                blueResult[i, j] = ((int)((float)(blueResult[i, j] - blueMin) * 255 / (float)(blueMax - blueMin)));
                            else
                                blueResult[i, j] = 0;
                        }
                    }
                }


                


                for (i = 0; i < H; i++)
                {
                    for (j = 0; j < W; j++)
                    {
                        resultBitmap.SetPixel(j, i, Color.FromArgb((int)redResult[i, j], (int)greenResult[i, j], (int)blueResult[i, j]));                        
                    }
                }               

               pictureBox1.BackgroundImage = resultBitmap;


            }
            catch (Exception exep)
            {
                System.Windows.Forms.MessageBox.Show(exep.ToString(), "Next time");
            }





        }// end of preview

        

        private void save_Click(object sender, EventArgs e)
        {
            string saveLocation = "";

            saveFileDialog1.InitialDirectory = "c:\\My Documents";
            saveFileDialog1.Filter = " Bitmap File (*.bmp)|*.bmp";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.ShowDialog();
            saveLocation = saveFileDialog1.FileName;
            resultBitmap.Save(saveLocation);

         


        }




    }
}