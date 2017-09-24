namespace OPCUAClient
{
   partial class MainFrm
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
         this.CreateSessionBtn = new System.Windows.Forms.Button();
         this.ReadNodeBtn = new System.Windows.Forms.Button();
         this.lvMonitored = new System.Windows.Forms.ListView();
         this.Node = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.tbServerState = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // CreateSessionBtn
         // 
         this.CreateSessionBtn.Location = new System.Drawing.Point(13, 14);
         this.CreateSessionBtn.Name = "CreateSessionBtn";
         this.CreateSessionBtn.Size = new System.Drawing.Size(121, 23);
         this.CreateSessionBtn.TabIndex = 1;
         this.CreateSessionBtn.Text = "Create Session";
         this.CreateSessionBtn.UseVisualStyleBackColor = true;
         this.CreateSessionBtn.Click += new System.EventHandler(this.CreateSessionBtn_Click);
         // 
         // ReadNodeBtn
         // 
         this.ReadNodeBtn.Location = new System.Drawing.Point(13, 60);
         this.ReadNodeBtn.Name = "ReadNodeBtn";
         this.ReadNodeBtn.Size = new System.Drawing.Size(75, 23);
         this.ReadNodeBtn.TabIndex = 6;
         this.ReadNodeBtn.Text = "Read Node";
         this.ReadNodeBtn.UseVisualStyleBackColor = true;
         this.ReadNodeBtn.Click += new System.EventHandler(this.ReadNode_Click);
         // 
         // lvMonitored
         // 
         this.lvMonitored.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Node,
            this.Value});
         this.lvMonitored.Location = new System.Drawing.Point(13, 114);
         this.lvMonitored.Name = "lvMonitored";
         this.lvMonitored.Size = new System.Drawing.Size(703, 360);
         this.lvMonitored.TabIndex = 7;
         this.lvMonitored.UseCompatibleStateImageBehavior = false;
         this.lvMonitored.View = System.Windows.Forms.View.Details;
         // 
         // Node
         // 
         this.Node.Text = "Node";
         this.Node.Width = 251;
         // 
         // Value
         // 
         this.Value.Text = "Value";
         // 
         // tbServerState
         // 
         this.tbServerState.Location = new System.Drawing.Point(154, 14);
         this.tbServerState.Name = "tbServerState";
         this.tbServerState.Size = new System.Drawing.Size(373, 20);
         this.tbServerState.TabIndex = 8;
         // 
         // MainFrm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(728, 528);
         this.Controls.Add(this.tbServerState);
         this.Controls.Add(this.lvMonitored);
         this.Controls.Add(this.ReadNodeBtn);
         this.Controls.Add(this.CreateSessionBtn);
         this.Name = "MainFrm";
         this.Text = "UAClient";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Button CreateSessionBtn;
      private System.Windows.Forms.Button ReadNodeBtn;
      private System.Windows.Forms.ListView lvMonitored;
      private System.Windows.Forms.ColumnHeader Node;
      private System.Windows.Forms.ColumnHeader Value;
      private System.Windows.Forms.TextBox tbServerState;
   }
}

