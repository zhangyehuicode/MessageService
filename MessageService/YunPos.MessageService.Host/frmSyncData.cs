using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YunPos.MessageService.Host
{
    public partial class frmSyncData : Form
    {
        string _posApiDomain = string.Empty;

        public frmSyncData()
        {
            InitializeComponent();
            Init();
            _posApiDomain = AppContext.Settings.YunService;
        }

        #region methods
        void Init()
        {
            btnSyncData.Click += btnSyncData_Click;
        }

        #endregion

        #region event handlers
        void btnSyncData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_posApiDomain))
            {
                MessageBox.Show("PosApi地址未配置，请检查配置");
                return;
            }
            new SyncDataTask().Start(null);
            txtLog.Text += "已发送同步数据请求\r\n";
            txtLog.ScrollToCaret();
        }
        #endregion
    }
}
