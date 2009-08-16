namespace BattleConsole
{
    partial class BattleGUI
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
            this.tmAmi = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmAmi
            // 
            this.tmAmi.Interval = 10;
            this.tmAmi.Tick += new System.EventHandler(this.tmAmi_Tick);
            // 
            // BattleGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 283);
            this.Name = "BattleGUI";
            this.Text = "BattleGUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BattleGUI_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmAmi;

    }
}