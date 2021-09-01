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
        string[] category = new string[] { "Folk og Interiør", "Handelssteder", "Handelsvare" };
        public string sortCategory = "Folk og Interiør";

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
                foreach (string s in c.Split(new string[] { ";" }, StringSplitOptions.None))
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
            HashSet<Shell32.FolderItem2> imageList = new HashSet<Shell32.FolderItem2>(dynamicCategories[sortCategory]);
            displayItems = new List<Shell32.FolderItem2>(imageList);
        }

        public List<Shell32.FolderItem2> getDisplayItems()
        {
            return displayItems;
        }

        public void setCategory(int a)
        {
            this.sortCategory = category[a];
        }

        //debugfunction, writes current image data to console through displayscreem
        public string getData(Shell32.FolderItem2 im)
        {
           return picFolder.GetDetailsOf(im, 18);
        }
    }
}
