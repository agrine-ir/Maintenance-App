namespace TestApp
{
    partial class Form1
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
            this.alButton1 = new Agrine.Maintenance.UI.ALButton();
            this.SuspendLayout();
            // 
            // alButton1
            // 
            this.alButton1.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.alButton1.BorderColor = System.Drawing.Color.White;
            this.alButton1.BorderRadius = 10;
            this.alButton1.BorderSize = 4;
            this.alButton1.FlatAppearance.BorderSize = 0;
            this.alButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.alButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alButton1.ForeColor = System.Drawing.Color.White;
            this.alButton1.Location = new System.Drawing.Point(558, 344);
            this.alButton1.Name = "alButton1";
            this.alButton1.Size = new System.Drawing.Size(234, 112);
            this.alButton1.TabIndex = 0;
            this.alButton1.Text = "سلام و درود";
            this.alButton1.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(862, 500);
            this.Controls.Add(this.alButton1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Agrine.Maintenance.UI.ALButton alButton1;
    }
}

