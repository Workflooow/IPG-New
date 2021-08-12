using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormTest
{
    class controlScreenButton : Button
    {
        public int controlInt = 0;
        public Image mainImage = null;
        public Image highLightImage = null;

        public controlScreenButton()
        {
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0; 
                FlatAppearance.MouseOverBackColor = Color.LightGray;
        }

        protected override void OnClick(EventArgs e)
        {
            this.BackgroundImage = highLightImage;
            this.BackColor = Color.LightGray;
            base.OnClick(e);
        }
    }
}