using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReduxLauncher
{
    public partial class Interface : Form
    {
        PatchHandler handler;

        public Interface()
        {
            InitializeComponent();

            handler = new PatchHandler(this);

            progressBar.Style = ProgressBarStyle.Continuous;
        }

        public ProgressBar ProgressBar()
        {
            return progressBar;
        }

        public void UpdatePatchNotes(string notes)
        {
            if (patchNotes.InvokeRequired)
            {
                patchNotes.Invoke
                    (new MethodInvoker(delegate
                        {
                            patchNotes.Text += notes + "\n";
                            patchNotes.SelectionStart = patchNotes.Text.Length;
                            patchNotes.ScrollToCaret();
                        }));
            }

            else
            {
                patchNotes.Text += notes + "\n";
                patchNotes.SelectionStart = patchNotes.Text.Length;
                patchNotes.ScrollToCaret();
            }
            
        }

        private void Patch_btn_Click(object sender, EventArgs e)
        {
        }

        private void Launch_btn_Click(object sender, EventArgs e)
        {

        }

        private void Download_btn_Click(object sender, EventArgs e)
        {
            handler.InitializeDownload();
        }
    }
}
