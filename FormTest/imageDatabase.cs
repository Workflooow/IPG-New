using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization.Configuration;

namespace FormTest
{
    public class imageDatabase
    {
        List<Shell32.FolderItem2> displayItems = new List<Shell32.FolderItem2>();
        public List<Shell32.FolderItem2> allItems = new List<Shell32.FolderItem2>();

        string root;
        string[] seasons = new string[] { "Vår", "Sommer", "Høst", "Vinter" };
        string[] locations = new string[] { "Hus/Kjøkken", "Åker/Eng/Skog", "Verksted", 
                                            "Fjøs/Dyrestell", "Husstell/Kjøkken" };
        public string sortSeason = "Vår";
        public string sortLocation = "Verksted";

        Dictionary<string,HashSet<Shell32.FolderItem2>> dynamicCategories = new Dictionary<string,HashSet<Shell32.FolderItem2>>();

        Shell32.Shell shell;
        Shell32.Folder picFolder;

        public imageDatabase()
        {
            shell = new Shell32.Shell();
            picFolder = shell.NameSpace(Properties.Settings.Default.fileDirectoryPath);

            foreach (Shell32.FolderItem2 item in picFolder.Items())
            {
                allItems.Add(item);
            }

            foreach (var item in allItems)
	        {
                string c = picFolder.GetDetailsOf(item, 18);
                foreach (string s in c.Split(new string[] { "; " }, StringSplitOptions.None))
                {
                    if (!dynamicCategories.ContainsKey(s))
	                {
                        dynamicCategories.Add(s,new HashSet<Shell32.FolderItem2>());
	                }
                    dynamicCategories[s].Add(item);
                }
	        }
        }

        public void constructImageList()
        {
            HashSet<Shell32.FolderItem2> imageList = new HashSet<Shell32.FolderItem2>(dynamicCategories[sortSeason]);
            imageList.IntersectWith(dynamicCategories[sortLocation]);
            displayItems = new List<Shell32.FolderItem2>(imageList);
        }

        public List<Shell32.FolderItem2> getDisplayItems()
        {
            return displayItems;
        }

        public void setSeason(int a)
        {
            this.sortSeason = seasons[a];
        }

        public void setLocation(int a)
        {
            this.sortLocation = locations[a];
        }

        //debugfunction, writes current image data to console through displayscreem
        public string getData(Shell32.FolderItem2 im)
        {
           return picFolder.GetDetailsOf(im, 18);
        }
    }
}
