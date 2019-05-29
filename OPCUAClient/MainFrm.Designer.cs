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
         this.tbServerState = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.tbHubState = new System.Windows.Forms.TextBox();
         this.btnConnectBackEndServcie = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // tbServerState
         // 
         this.tbServerState.Location = new System.Drawing.Point(116, 14);
         this.tbServerState.Name = "tbServerState";
         this.tbServerState.Size = new System.Drawing.Size(141, 21);
         this.tbServerState.TabIndex = 8;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(13, 17);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(89, 12);
         this.label1.TabIndex = 9;
         this.label1.Text = "UAServerStatus";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(13, 53);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(101, 12);
         this.label2.TabIndex = 10;
         this.label2.Text = "HubConnectStatus";
         // 
         // tbHubState
         // 
         this.tbHubState.Location = new System.Drawing.Point(116, 53);
         this.tbHubState.Name = "tbHubState";
         this.tbHubState.Size = new System.Drawing.Size(141, 21);
         this.tbHubState.TabIndex = 11;
         // 
         // btnConnectBackEndServcie
         // 
         this.btnConnectBackEndServcie.Location = new System.Drawing.Point(15, 109);
         this.btnConnectBackEndServcie.Name = "btnConnectBackEndServcie";
         this.btnConnectBackEndServcie.Size = new System.Drawing.Size(137, 23);
         this.btnConnectBackEndServcie.TabIndex = 12;
         this.btnConnectBackEndServcie.Text = "ConnectBackEndService";
         this.btnConnectBackEndServcie.UseVisualStyleBackColor = true;
         this.btnConnectBackEndServcie.Click += new System.EventHandler(this.btnConnectBackEndServcie_Click);
         // 
         // MainFrm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(273, 162);
         this.Controls.Add(this.btnConnectBackEndServcie);
         this.Controls.Add(this.tbHubState);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.tbServerState);
         this.Name = "MainFrm";
         this.Text = "UAClient";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      private System.Windows.Forms.TextBox tbServerState;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox tbHubState;
      private System.Windows.Forms.Button btnConnectBackEndServcie;
   }
}

