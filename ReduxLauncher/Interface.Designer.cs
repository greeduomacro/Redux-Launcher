namespace ReduxLauncher
{
    partial class Interface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Interface));
            this.patchNotes = new System.Windows.Forms.RichTextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.Download_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // patchNotes
            // 
            this.patchNotes.BackColor = System.Drawing.Color.DarkGray;
            this.patchNotes.Location = new System.Drawing.Point(12, 411);
            this.patchNotes.Name = "patchNotes";
            this.patchNotes.ReadOnly = true;
            this.patchNotes.Size = new System.Drawing.Size(378, 66);
            this.patchNotes.TabIndex = 0;
            this.patchNotes.Text = "";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 382);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(378, 23);
            this.progressBar.TabIndex = 0;
            this.progressBar.UseWaitCursor = true;
            // 
            // Download_btn
            // 
            this.Download_btn.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.Download_btn.BackColor = System.Drawing.Color.Gray;
            this.Download_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Download_btn.Location = new System.Drawing.Point(13, 483);
            this.Download_btn.Name = "Download_btn";
            this.Download_btn.Size = new System.Drawing.Size(378, 23);
            this.Download_btn.TabIndex = 3;
            this.Download_btn.Text = "Download";
            this.Download_btn.UseVisualStyleBackColor = false;
            this.Download_btn.Click += new System.EventHandler(this.Download_btn_Click);
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(404, 518);
            this.Controls.Add(this.Download_btn);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.patchNotes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Interface";
            this.Text = "Redux Launcher";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox patchNotes;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button Download_btn;
    }
}

