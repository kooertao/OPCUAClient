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
         this.ReadNodeBtn = new System.Windows.Forms.Button();
         this.lvMonitored = new System.Windows.Forms.ListView();
         this.Node = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.tbServerState = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.tbHubState = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // ReadNodeBtn
         // 
         this.ReadNodeBtn.Location = new System.Drawing.Point(13, 63);
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
         this.lvMonitored.Location = new System.Drawing.Point(13, 92);
         this.lvMonitored.Name = "lvMonitored";
         this.lvMonitored.Size = new System.Drawing.Size(703, 284);
         this.lvMonitored.TabIndex = 7;
         this.lvMonitored.UseCompatibleStateImageBehavior = false;
         this.lvMonitored.View = System.Windows.Forms.View.Details;
         // 
         // Node
         // 
         this.Node.Text = "Node";
         this.Node.Width = 313;
         // 
         // Value
         // 
         this.Value.Text = "Value";
         // 
         // tbServerState
         // 
         this.tbServerState.Location = new System.Drawing.Point(97, 12);
         this.tbServerState.Name = "tbServerState";
         this.tbServerState.Size = new System.Drawing.Size(200, 20);
         this.tbServerState.TabIndex = 8;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(13, 18);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(83, 13);
         this.label1.TabIndex = 9;
         this.label1.Text = "UAServerStatus";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(352, 18);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(97, 13);
         this.label2.TabIndex = 10;
         this.label2.Text = "HubConnectStatus";
         // 
         // tbHubState
         // 
         this.tbHubState.Location = new System.Drawing.Point(455, 11);
         this.tbHubState.Name = "tbHubState";
         this.tbHubState.Size = new System.Drawing.Size(215, 20);
         this.tbHubState.TabIndex = 11;
         // 
         // MainFrm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(721, 381);
         this.Controls.Add(this.tbHubState);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.tbServerState);
         this.Controls.Add(this.lvMonitored);
         this.Controls.Add(this.ReadNodeBtn);
         this.Name = "MainFrm";
         this.Text = "UAClient";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Button ReadNodeBtn;
      private System.Windows.Forms.ListView lvMonitored;
      private System.Windows.Forms.ColumnHeader Node;
      private System.Windows.Forms.ColumnHeader Value;
      private System.Windows.Forms.TextBox tbServerState;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox tbHubState;
   }
}

