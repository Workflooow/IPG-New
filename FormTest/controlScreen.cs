using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using Shell32;
using System.Reflection;
using FormTest.Properties;
using IWshRuntimeLibrary;
using Microsoft.Win32;

namespace FormTest
{
    public partial class controlScreen : Form
    {
        displayScreen displayScreen1;
        FolderBrowserDialog folderDialog = new FolderBrowserDialog();
        imageDatabase seeker;
        public readonly Screen cScreen = Screen.AllScreens[0];
        (int x, int y)[] positionListCategories = new (int, int)[] { (25, 20), (25, 330), (25, 640), (25, 950) };
        //int[] positionListSeasons = new int[] { 112, 422, 732, 1042}; //evenned out
        int[] positionListSeasons = new int[] { 180, 380, 580, 780, 980}; 
        List<Image> categoryImages = new List<Image>();
        List<Image> seasonImages = new List<Image>();
        List<controlScreenButton> categoryButtons = new List<controlScreenButton>();
        List<controlScreenButton> seasonButtons = new List<controlScreenButton>();
        controlScreenButton currentSeason = new controlScreenButton(); 
        controlScreenButton currentCategory = new controlScreenButton();
        Pen pen = new Pen(Color.Gray);
        ResourceSet rs = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
        List<Image> allImages = new List<Image>();
        WshShell wS = new WshShell();

        public controlScreen()
        {
            //this.BackColor = Color.FromArgb(45,45,48);
            this.BackColor = Color.FromArgb(50,53,59);
            Load += new EventHandler(Form1_Load);
            FormBorderStyle = FormBorderStyle.None;
            this.Paint += new PaintEventHandler(drawLine);

            if (!Directory.Exists(Properties.Settings.Default.fileDirectoryPath))
            {
                folderDialog.ShowDialog();
                Properties.Settings.Default.fileDirectoryPath = folderDialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
            
            seeker = new imageDatabase();

            for (int i = 0; i < 8; i++)
            {
                allImages.Add(rs.GetObject("k" + i) as Image);
            }

            for (int i = 0; i < 8; i++)
            {
                allImages.Add(rs.GetObject("s" + i) as Image);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location = cScreen.WorkingArea.Location;
            Size = cScreen.WorkingArea.Size;
            displayScreen1 = new displayScreen(this, seeker);
            displayScreen1.Visible = true;
            uiStyle1();
        }

        private void drawLine(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            pen.Width = 2;
            pen.Color = Color.Gray;
            g.DrawLine(pen, new Point(20,600), new Point(1260,600));
        }

        private void uiStyle1()
        {
            for (int i = 0; i < 4; i++)
            {
                controlScreenButton b = new controlScreenButton();
                Controls.Add(b);
                seasonButtons.Add(b);
                b.Parent = this;
                b.Size = new Size(120, 120);
                b.Top = (this.ClientSize.Height / 10) * 8;
                b.controlInt = i;
                b.highLightImage = new Bitmap(allImages[i+12], b.Size);
                b.mainImage = new Bitmap(allImages[i+8], b.Size);
                b.BackgroundImage = b.mainImage;
                b.Click += new EventHandler(style1SeasonFunc);
                b.Click += new EventHandler(stopButtonFunc);
                b.Left = positionListSeasons[i];
            }

            //populate the category buttons
            for (int i = 0; i < 4; i++)
            {
                controlScreenButton b = new controlScreenButton();
                Controls.Add(b);
                categoryButtons.Add(b);
                b.Parent = this;
                b.Size = new Size(305, 545);
                b.Top = positionListCategories[i].x;
                b.Left = positionListCategories[i].y;
                b.controlInt = i;
                b.highLightImage = new Bitmap(allImages[i+4],b.Size);
                b.mainImage = new Bitmap(allImages[i],b.Size);
                b.BackgroundImage = b.mainImage;
                b.Click += new EventHandler(style1ButtonFunc);
                b.Click += new EventHandler(stopButtonFunc);
            }

             controlScreenButton nextButton = new controlScreenButton();
             Controls.Add(nextButton);
             nextButton.Parent = this;
             nextButton.Size = new Size(120, 120);
             nextButton.Top = (this.ClientSize.Height / 10) * 8;
             nextButton.Left = positionListSeasons[4];
             nextButton.Click += new EventHandler(displayScreen1.displayNextImage);
             nextButton.mainImage = rs.GetObject("next") as Image;
             nextButton.highLightImage = nextButton.mainImage;
             nextButton.BackgroundImage = nextButton.mainImage;
             nextButton.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void style1SeasonFunc(object sender, EventArgs e)
        {
            seeker.setSeason((sender as controlScreenButton).controlInt);

            if (currentSeason != sender as controlScreenButton)
	        {
                currentSeason.BackgroundImage = currentSeason.mainImage;
                currentSeason.BackColor = Color.FromArgb(100,100,100);
                currentSeason = (sender as controlScreenButton);
	        }
            
        }

        private void style1ButtonFunc(object sender, EventArgs e)
        {
            seeker.setLocation((sender as controlScreenButton).controlInt);

            if (currentCategory != sender as controlScreenButton)
	        {
                currentCategory.BackgroundImage = currentCategory.mainImage;
                currentCategory.BackColor = Color.FromArgb(100,100,100);
                currentCategory = (sender as controlScreenButton);
	        }
        }

        private void stopButtonFunc(object sender, EventArgs e)
        {
                seeker.constructImageList();
                displayScreen1.display();
        }
    }
}
