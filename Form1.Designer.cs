﻿namespace FTPTransfer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timMonitor = new System.Windows.Forms.Timer(this.components);
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timMonitor
            // 
            this.timMonitor.Interval = 10000;
            this.timMonitor.Tick += new System.EventHandler(this.timMonitor_Tick);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(32, 54);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(624, 259);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Process Message";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 350);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMessage);
            this.Name = "Form1";
            this.Text = "FTP FORWARDER";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timMonitor;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Label label1;
    }
}
