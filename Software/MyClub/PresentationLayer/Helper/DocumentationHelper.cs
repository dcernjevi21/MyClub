using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLayer.Helper
{
    public class DocumentationHelper
    {
        

        public static void OpenUserDocumentation()
        {
            try
            {
                //https://www.c-sharpcorner.com/forums/opening-pdf-files-from-resources
                byte[] PDF = null;
                if (CurrentUser.User.RoleID == 1)
                {
                    PDF = Properties.Resources.MyClub_admini;
                }
                else if (CurrentUser.User.RoleID == 2)
                {
                    PDF = Properties.Resources.MyClub_treneri;
                }
                else
                {
                    PDF = Properties.Resources.FintessSys_korisnici;
                }
                MemoryStream ms = new MemoryStream(PDF);
                FileStream f = new FileStream("help-members.pdf", FileMode.OpenOrCreate);

                ms.WriteTo(f);

                f.Close();
                ms.Close();

                Process.Start("help-members.pdf");
            }
            catch (Exception)
            {
                //MessageBox.Show("Nije moguće otvoriti dokumentaciju!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
