namespace Server
{
  partial class ServerForm
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
      this.label1 = new System.Windows.Forms.Label();
      this.portTextBox = new System.Windows.Forms.TextBox();
      this.startBtn = new System.Windows.Forms.Button();
      this.messageTextBox = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "port:";
      // 
      // portTextBox
      // 
      this.portTextBox.Location = new System.Drawing.Point(63, 13);
      this.portTextBox.Name = "portTextBox";
      this.portTextBox.Size = new System.Drawing.Size(100, 22);
      this.portTextBox.TabIndex = 1;
      // 
      // startBtn
      // 
      this.startBtn.Location = new System.Drawing.Point(17, 47);
      this.startBtn.Name = "startBtn";
      this.startBtn.Size = new System.Drawing.Size(322, 36);
      this.startBtn.TabIndex = 2;
      this.startBtn.Text = "start";
      this.startBtn.UseVisualStyleBackColor = true;
      this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
      // 
      // messageTextBox
      // 
      this.messageTextBox.Location = new System.Drawing.Point(17, 89);
      this.messageTextBox.Name = "messageTextBox";
      this.messageTextBox.Size = new System.Drawing.Size(322, 349);
      this.messageTextBox.TabIndex = 3;
      this.messageTextBox.Text = "";
      // 
      // ServerForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(351, 450);
      this.Controls.Add(this.messageTextBox);
      this.Controls.Add(this.startBtn);
      this.Controls.Add(this.portTextBox);
      this.Controls.Add(this.label1);
      this.Name = "ServerForm";
      this.Text = "Server";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox portTextBox;
    private System.Windows.Forms.Button startBtn;
    private System.Windows.Forms.RichTextBox messageTextBox;
  }
}

