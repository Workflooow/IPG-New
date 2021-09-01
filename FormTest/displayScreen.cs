using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shell32;

namespace FormTest
{
    public partial class displayScreen : Form
    {
        imageDatabase seeker;
        Timer aTimer = new Timer();
        PictureBox pb1 = new PictureBox();
        private int displayIndex = 0;

        public displayScreen(controlScreen cs, imageDatabase s)
        {
            this.BackColor = Color.FromArgb(54,57,63);
            seeker = s;
            seeker.constructImageList();

            aTimer.Tick += new EventHandler(displayOnaTimer);
            aTimer.Interval = 3000;
            Load += new EventHandler(Form2_Load);

            Controls.Add(pb1);
            pb1.SizeMode = PictureBoxSizeMode.Zoom;
            FormBorderStyle = FormBorderStyle.None;
        }

        public void Form2_Load(object sender, EventArgs e)
        {
            Location = Screen.AllScreens[0].WorkingArea.Location;
            WindowState = FormWindowState.Maximized;
            pb1.Size = Size;
            
        }

        public void fillWindow(FolderItem2 foo)
        {
            if (pb1.Image != null)
            {
                pb1.Image.Dispose();
            }
            pb1.Image = Image.FromFile(foo.Path);
        }

        public void clearImage()
        {
            if(pb1.Image != null)
            {
                pb1.Image.Dispose();
                pb1.Image = null;
            }
        }

        public void display()
        {
            
            List<Shell32.FolderItem2> temp = seeker.getDisplayItems();
            if (aTimer.Enabled)
	        {
                aTimer.Stop();
	        }

            if(temp.Count>0)
            {
                fillWindow(temp[0]);
                Console.WriteLine(seeker.sortCategory);
                aTimer.Start();
            }
            else
            {
                temp = seeker.allItems;
                fillWindow(temp[0]);
                aTimer.Start();
                Console.WriteLine(seeker.sortCategory + "," + "but list was empty");
            }
        }

        public void stopDisplay()
        {
            aTimer.Stop();
            clearImage();
        }

        private void displayOnaTimer(Object myObject,EventArgs myEventArgs)
        {
            var foo = seeker.getDisplayItems();
            if(foo.Count == 0)
            {
                foo = seeker.allItems;
            }
            if (displayIndex<foo.Count()-1)
            {
                displayIndex++;
            }
            else
            {
                displayIndex = 0;
            }
            fillWindow(foo[displayIndex]);
            Console.WriteLine(seeker.getData(foo[displayIndex]));
        }

        public void displayNextImage(object sender, EventArgs e)
        {
            displayIndex++;
            fillWindow(seeker.allItems[displayIndex]);
            aTimer.Stop();
            aTimer.Start();
        }
    }
}
