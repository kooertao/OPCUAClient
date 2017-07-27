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
         this.tbResult = new System.Windows.Forms.TextBox();
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
         // AddSubscriptionBtn
         // 
         this.AddSubscriptionBtn.Location = new System.Drawing.Point(13, 64);
         this.AddSubscriptionBtn.Name = "AddSubscriptionBtn";
         this.AddSubscriptionBtn.Size = new System.Drawing.Size(121, 23);
         this.AddSubscriptionBtn.TabIndex = 2;
         this.AddSubscriptionBtn.Text = "Add Subscription";
         this.AddSubscriptionBtn.UseVisualStyleBackColor = true;
         this.AddSubscriptionBtn.Click += new System.EventHandler(this.AddSubscriptionBtn_Click);
         // 
         // BrowseBtn
         // 
         this.BrowseBtn.Location = new System.Drawing.Point(13, 117);
         this.BrowseBtn.Name = "BrowseBtn";
         this.BrowseBtn.Size = new System.Drawing.Size(121, 24);
         this.BrowseBtn.TabIndex = 3;
         this.BrowseBtn.Text = "Browse";
         this.BrowseBtn.UseVisualStyleBackColor = true;
         this.BrowseBtn.Click += new System.EventHandler(this.ReadNodeBtn_Click);
         // 
         // tvItems
         // 
         this.tvItems.Location = new System.Drawing.Point(140, 117);
         this.tvItems.Name = "tvItems";
         this.tvItems.Size = new System.Drawing.Size(250, 197);
         this.tvItems.TabIndex = 4;
         // 
         // lvItems
         // 
         this.lvItems.Location = new System.Drawing.Point(408, 117);
         this.lvItems.Name = "lvItems";
         this.lvItems.Size = new System.Drawing.Size(287, 136);
         this.lvItems.TabIndex = 5;
         this.lvItems.UseCompatibleStateImageBehavior = false;
         // 
         // ReadNode
         // 
         this.ReadNode.Location = new System.Drawing.Point(13, 344);
         this.ReadNode.Name = "ReadNode";
         this.ReadNode.Size = new System.Drawing.Size(75, 23);
         this.ReadNode.TabIndex = 6;
         this.ReadNode.Text = "Read";
         this.ReadNode.UseVisualStyleBackColor = true;
         this.ReadNode.Click += new System.EventHandler(this.ReadNode_Click);
         // 
         // tbResult
         // 
         this.tbResult.Location = new System.Drawing.Point(140, 346);
         this.tbResult.Multiline = true;
         this.tbResult.Name = "tbResult";
         this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.tbResult.Size = new System.Drawing.Size(250, 134);
         this.tbResult.TabIndex = 7;
         // 
         // MainFrm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(755, 509);
         this.Controls.Add(this.tbResult);
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
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.Button CreateSessionBtn;
      private System.Windows.Forms.Button AddSubscriptionBtn;
      private System.Windows.Forms.Button BrowseBtn;
      private System.Windows.Forms.TreeView tvItems;
      private System.Windows.Forms.ListView lvItems;
      private System.Windows.Forms.Button ReadNode;
      private System.Windows.Forms.TextBox tbResult;
   }
}

