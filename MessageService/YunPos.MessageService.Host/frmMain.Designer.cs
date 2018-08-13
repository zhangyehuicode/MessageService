namespace YunPos.MessageService.Host
{
    partial class frmMain
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
            this.pnlControl = new System.Windows.Forms.Panel();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnSyncData = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOperate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpMain = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.grpClientList = new System.Windows.Forms.GroupBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lstClientList = new System.Windows.Forms.ListBox();
            this.pnlControl.SuspendLayout();
            this.grpMain.SuspendLayout();
            this.grpClientList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControl
            // 
            this.pnlControl.Controls.Add(this.btnExchange);
            this.pnlControl.Controls.Add(this.btnSyncData);
            this.pnlControl.Controls.Add(this.btnClient);
            this.pnlControl.Controls.Add(this.lblStatus);
            this.pnlControl.Controls.Add(this.btnOperate);
            this.pnlControl.Controls.Add(this.label1);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControl.Location = new System.Drawing.Point(0, 0);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(813, 53);
            this.pnlControl.TabIndex = 0;
            // 
            // btnExchange
            // 
            this.btnExchange.Location = new System.Drawing.Point(571, 15);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(75, 23);
            this.btnExchange.TabIndex = 5;
            this.btnExchange.Text = "Exchange";
            this.btnExchange.UseVisualStyleBackColor = true;
            this.btnExchange.Visible = false;
            // 
            // btnSyncData
            // 
            this.btnSyncData.Location = new System.Drawing.Point(454, 15);
            this.btnSyncData.Name = "btnSyncData";
            this.btnSyncData.Size = new System.Drawing.Size(75, 23);
            this.btnSyncData.TabIndex = 4;
            this.btnSyncData.Text = "SyncData";
            this.btnSyncData.UseVisualStyleBackColor = true;
            this.btnSyncData.Visible = false;
            // 
            // btnClient
            // 
            this.btnClient.Location = new System.Drawing.Point(338, 15);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(75, 23);
            this.btnClient.TabIndex = 3;
            this.btnClient.Text = "Client";
            this.btnClient.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(78, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "未启动";
            // 
            // btnOperate
            // 
            this.btnOperate.Location = new System.Drawing.Point(161, 15);
            this.btnOperate.Name = "btnOperate";
            this.btnOperate.Size = new System.Drawing.Size(75, 23);
            this.btnOperate.TabIndex = 1;
            this.btnOperate.Text = "启动";
            this.btnOperate.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务状态:";
            // 
            // grpMain
            // 
            this.grpMain.Controls.Add(this.txtLog);
            this.grpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMain.Location = new System.Drawing.Point(0, 223);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(813, 232);
            this.grpMain.TabIndex = 2;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "服务日志";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 17);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(807, 212);
            this.txtLog.TabIndex = 0;
            // 
            // grpClientList
            // 
            this.grpClientList.Controls.Add(this.btnSendMessage);
            this.grpClientList.Controls.Add(this.txtMessage);
            this.grpClientList.Controls.Add(this.lstClientList);
            this.grpClientList.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpClientList.Location = new System.Drawing.Point(0, 53);
            this.grpClientList.Name = "grpClientList";
            this.grpClientList.Size = new System.Drawing.Size(813, 170);
            this.grpClientList.TabIndex = 3;
            this.grpClientList.TabStop = false;
            this.grpClientList.Text = "客户端列表";
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(338, 116);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(227, 39);
            this.btnSendMessage.TabIndex = 2;
            this.btnSendMessage.Text = "发送消息";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(338, 20);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(227, 90);
            this.txtMessage.TabIndex = 1;
            // 
            // lstClientList
            // 
            this.lstClientList.FormattingEnabled = true;
            this.lstClientList.ItemHeight = 12;
            this.lstClientList.Location = new System.Drawing.Point(6, 20);
            this.lstClientList.Name = "lstClientList";
            this.lstClientList.Size = new System.Drawing.Size(301, 136);
            this.lstClientList.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 455);
            this.Controls.Add(this.grpMain);
            this.Controls.Add(this.grpClientList);
            this.Controls.Add(this.pnlControl);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Host";
            this.pnlControl.ResumeLayout(false);
            this.pnlControl.PerformLayout();
            this.grpMain.ResumeLayout(false);
            this.grpMain.PerformLayout();
            this.grpClientList.ResumeLayout(false);
            this.grpClientList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOperate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpMain;
        private System.Windows.Forms.GroupBox grpClientList;
        private System.Windows.Forms.ListBox lstClientList;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Button btnSyncData;
        private System.Windows.Forms.Button btnExchange;
    }
}

