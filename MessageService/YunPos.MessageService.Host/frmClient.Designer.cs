namespace YunPos.MessageService.Host
{
    partial class frmClient
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOperate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.pnlSendMessage = new System.Windows.Forms.Panel();
            this.Group1 = new System.Windows.Forms.GroupBox();
            this.txtReceivedMessage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDestClient = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pnlSendMessage.SuspendLayout();
            this.Group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtClientId);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.btnOperate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 50);
            this.panel1.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(294, 19);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "未连接";
            // 
            // btnOperate
            // 
            this.btnOperate.Location = new System.Drawing.Point(213, 14);
            this.btnOperate.Name = "btnOperate";
            this.btnOperate.Size = new System.Drawing.Size(75, 23);
            this.btnOperate.TabIndex = 4;
            this.btnOperate.Text = "连接";
            this.btnOperate.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "连接状态:";
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(77, 15);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(130, 21);
            this.txtClientId.TabIndex = 6;
            this.txtClientId.Text = "ClientA";
            // 
            // pnlSendMessage
            // 
            this.pnlSendMessage.Controls.Add(this.btnSend);
            this.pnlSendMessage.Controls.Add(this.txtMessage);
            this.pnlSendMessage.Controls.Add(this.label3);
            this.pnlSendMessage.Controls.Add(this.txtDestClient);
            this.pnlSendMessage.Controls.Add(this.label2);
            this.pnlSendMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSendMessage.Location = new System.Drawing.Point(0, 50);
            this.pnlSendMessage.Name = "pnlSendMessage";
            this.pnlSendMessage.Size = new System.Drawing.Size(587, 92);
            this.pnlSendMessage.TabIndex = 1;
            // 
            // Group1
            // 
            this.Group1.Controls.Add(this.txtReceivedMessage);
            this.Group1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Group1.Location = new System.Drawing.Point(0, 142);
            this.Group1.Name = "Group1";
            this.Group1.Size = new System.Drawing.Size(587, 231);
            this.Group1.TabIndex = 2;
            this.Group1.TabStop = false;
            this.Group1.Text = "收到的消息";
            // 
            // txtReceivedMessage
            // 
            this.txtReceivedMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReceivedMessage.Location = new System.Drawing.Point(3, 17);
            this.txtReceivedMessage.Multiline = true;
            this.txtReceivedMessage.Name = "txtReceivedMessage";
            this.txtReceivedMessage.Size = new System.Drawing.Size(581, 211);
            this.txtReceivedMessage.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "发送目标:";
            // 
            // txtDestClient
            // 
            this.txtDestClient.Location = new System.Drawing.Point(77, 12);
            this.txtDestClient.Name = "txtDestClient";
            this.txtDestClient.Size = new System.Drawing.Size(130, 21);
            this.txtDestClient.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "消息:";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(77, 53);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(258, 21);
            this.txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(374, 16);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 373);
            this.Controls.Add(this.Group1);
            this.Controls.Add(this.pnlSendMessage);
            this.Controls.Add(this.panel1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WCF Client";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlSendMessage.ResumeLayout(false);
            this.pnlSendMessage.PerformLayout();
            this.Group1.ResumeLayout(false);
            this.Group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnOperate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.Panel pnlSendMessage;
        private System.Windows.Forms.GroupBox Group1;
        private System.Windows.Forms.TextBox txtReceivedMessage;
        private System.Windows.Forms.TextBox txtDestClient;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
    }
}