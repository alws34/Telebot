namespace Telebot
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.fastOlv = new BrightIdeasSoftware.FastObjectListView();
            this.olvDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvText = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.fastOlv)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Telebot";
            // 
            // fastOlv
            // 
            this.fastOlv.AllColumns.Add(this.olvDate);
            this.fastOlv.AllColumns.Add(this.olvText);
            this.fastOlv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvDate,
            this.olvText});
            this.fastOlv.Cursor = System.Windows.Forms.Cursors.Default;
            this.fastOlv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastOlv.HideSelection = false;
            this.fastOlv.Location = new System.Drawing.Point(0, 0);
            this.fastOlv.Name = "fastOlv";
            this.fastOlv.ShowGroups = false;
            this.fastOlv.Size = new System.Drawing.Size(693, 346);
            this.fastOlv.TabIndex = 0;
            this.fastOlv.UseCompatibleStateImageBehavior = false;
            this.fastOlv.View = System.Windows.Forms.View.Details;
            this.fastOlv.VirtualMode = true;
            // 
            // olvDate
            // 
            this.olvDate.AspectName = "DateTime";
            this.olvDate.Text = "Date";
            this.olvDate.Width = 147;
            // 
            // olvText
            // 
            this.olvText.AspectName = "Text";
            this.olvText.Text = "Details";
            this.olvText.Width = 285;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 346);
            this.Controls.Add(this.fastOlv);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Telebot";
            ((System.ComponentModel.ISupportInitialize)(this.fastOlv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private BrightIdeasSoftware.FastObjectListView fastOlv;
        private BrightIdeasSoftware.OLVColumn olvDate;
        private BrightIdeasSoftware.OLVColumn olvText;
    }
}

