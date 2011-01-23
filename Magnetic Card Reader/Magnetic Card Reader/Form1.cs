﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WaveReader;

namespace Magnetic_Card_Reader
{
    public partial class Form1 : Form
    {
        MagneticCardReader mcReader = new MagneticCardReader();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int yVal;
            int lastyVal = 0;
            int currSide = 1;
            bool currStateHi = false;

            Stream inpFile = new StreamReader("..\\..\\..\\test3 - ec k - rev.wav").BaseStream;
            WaveData wavData = new WaveData(inpFile);

            for (int i = 0; i < wavData.NumberOfFrames; i++)
            {
                yVal = wavData.Samples[0][i];
                if (yVal > 0x7FFF) yVal -= 0xFFFF;

                if (((lastyVal < yVal && currSide < 0) || (lastyVal > yVal && currSide > 0)) && (System.Math.Abs(yVal) > 0xFF))
                {
                    currSide = 0;
                    currStateHi = (yVal > 0xFF);
                    mcReader.addNewSignalState(i, Convert.ToInt32(currStateHi));
                }

                if (lastyVal * yVal < 1)
                {
                    currSide = Convert.ToInt32(!currStateHi)*2-1;
                }

                lastyVal = yVal;
            }

            label1.Text = "Output: " + mcReader.getDataString();
            inpFile.Close();
        }

    }
}