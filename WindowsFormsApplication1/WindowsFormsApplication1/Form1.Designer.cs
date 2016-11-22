namespace WindowsFormsApplication1
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
            this.Listen = new System.Windows.Forms.Button();
            this.Send = new System.Windows.Forms.Button();
            this.Disconnect = new System.Windows.Forms.Button();
            this.Data = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Listen
            // 
            this.Listen.Location = new System.Drawing.Point(0, 0);
            this.Listen.Name = "Listen";
            this.Listen.Size = new System.Drawing.Size(75, 23);
            this.Listen.TabIndex = 0;
            this.Listen.Text = "Listen";
            this.Listen.UseVisualStyleBackColor = true;
            this.Listen.Click += new System.EventHandler(this.Listen_Click);
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(82, -1);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(75, 23);
            this.Send.TabIndex = 1;
            this.Send.Text = "Send Data";
            this.Send.UseVisualStyleBackColor = true;
            // 
            // Disconnect
            // 
            this.Disconnect.Location = new System.Drawing.Point(164, -2);
            this.Disconnect.Name = "Disconnect";
            this.Disconnect.Size = new System.Drawing.Size(75, 23);
            this.Disconnect.TabIndex = 2;
            this.Disconnect.Text = "Disconnect";
            this.Disconnect.UseVisualStyleBackColor = true;
            // 
            // Data
            // 
            this.Data.Location = new System.Drawing.Point(0, 49);
            this.Data.Multiline = true;
            this.Data.Name = "Data";
            this.Data.Size = new System.Drawing.Size(157, 134);
            this.Data.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Data);
            this.Controls.Add(this.Disconnect);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.Listen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Listen;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.Button Disconnect;
        private System.Windows.Forms.TextBox Data;
    }
}

