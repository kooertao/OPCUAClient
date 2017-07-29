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
            this.AddSubscriptionBtn = new System.Windows.Forms.Button();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.tvItems = new System.Windows.Forms.TreeView();
            this.lvItems = new System.Windows.Forms.ListView();
            this.ReadNode = new System.Windows.Forms.Button();
            this.lvMonitored = new System.Windows.Forms.ListView();
            this.Node = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // CreateSessionBtn
            // 
            this.CreateSessionBtn.Location = new System.Drawing.Point(13, 13);
            this.CreateSessionBtn.Name = "CreateSessionBtn";
            this.CreateSessionBtn.Size = new System.Drawing.Size(121, 21);
            this.CreateSessionBtn.TabIndex = 1;
            this.CreateSessionBtn.Text = "Create Session";
            this.CreateSessionBtn.UseVisualStyleBackColor = true;
            this.CreateSessionBtn.Click += new System.EventHandler(this.CreateSessionBtn_Click);
            // 
            // AddSubscriptionBtn
            // 
            this.AddSubscriptionBtn.Location = new System.Drawing.Point(13, 59);
            this.AddSubscriptionBtn.Name = "AddSubscriptionBtn";
            this.AddSubscriptionBtn.Size = new System.Drawing.Size(121, 21);
            this.AddSubscriptionBtn.TabIndex = 2;
            this.AddSubscriptionBtn.Text = "Add Subscription";
            this.AddSubscriptionBtn.UseVisualStyleBackColor = true;
            this.AddSubscriptionBtn.Click += new System.EventHandler(this.AddSubscriptionBtn_Click);
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Location = new System.Drawing.Point(13, 108);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(121, 22);
            this.BrowseBtn.TabIndex = 3;
            this.BrowseBtn.Text = "Browse";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.ReadNodeBtn_Click);
            // 
            // tvItems
            // 
            this.tvItems.Location = new System.Drawing.Point(140, 108);
            this.tvItems.Name = "tvItems";
            this.tvItems.Size = new System.Drawing.Size(250, 182);
            this.tvItems.TabIndex = 4;
            // 
            // lvItems
            // 
            this.lvItems.Location = new System.Drawing.Point(408, 108);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(287, 126);
            this.lvItems.TabIndex = 5;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            // 
            // ReadNode
            // 
            this.ReadNode.Location = new System.Drawing.Point(13, 318);
            this.ReadNode.Name = "ReadNode";
            this.ReadNode.Size = new System.Drawing.Size(75, 21);
            this.ReadNode.TabIndex = 6;
            this.ReadNode.Text = "Read";
            this.ReadNode.UseVisualStyleBackColor = true;
            this.ReadNode.Click += new System.EventHandler(this.ReadNode_Click);
            // 
            // lvMonitored
            // 
            this.lvMonitored.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Node,
            this.Value});
            this.lvMonitored.Location = new System.Drawing.Point(140, 318);
            this.lvMonitored.Name = "lvMonitored";
            this.lvMonitored.Size = new System.Drawing.Size(479, 142);
            this.lvMonitored.TabIndex = 7;
            this.lvMonitored.UseCompatibleStateImageBehavior = false;
            this.lvMonitored.View = System.Windows.Forms.View.Details;
            // 
            // Node
            // 
            this.Node.Text = "Node";
            this.Node.Width = 200;
            // 
            // Value
            // 
            this.Value.Text = "Value";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 470);
            this.Controls.Add(this.lvMonitored);
            this.Controls.Add(this.ReadNode);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.tvItems);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.AddSubscriptionBtn);
            this.Controls.Add(this.CreateSessionBtn);
            this.Name = "MainFrm";
            this.Text = "UAClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
            this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.Button CreateSessionBtn;
      private System.Windows.Forms.Button AddSubscriptionBtn;
      private System.Windows.Forms.Button BrowseBtn;
      private System.Windows.Forms.TreeView tvItems;
      private System.Windows.Forms.ListView lvItems;
      private System.Windows.Forms.Button ReadNode;
      private System.Windows.Forms.ListView lvMonitored;
      private System.Windows.Forms.ColumnHeader Node;
      private System.Windows.Forms.ColumnHeader Value;
   }
}

