using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FTPTransfer
{
    public partial class Form1 : Form
    {
        enum ErrCode
        {
            ERROR = -1,
            SUCCESS = 0
        }

        private FTPClient _Client;

        public Form1()
        {
            InitializeComponent();

            _Client = new FTPClient(this);

            int mCycle = 0;

            mCycle = IniFile.GetValueInt("FTP", "UploadCycle");
            if (mCycle > 0)
            {
                timMonitor.Interval = mCycle;
                OutputLog("Read INI file.");
            }
            else
            {
                timMonitor.Interval = 30000;
                OutputLog("[FAILURE] Read INI file. Not start.");
            }

            timMonitor.Enabled = true;

            if (_Client.SetUpClient() == (int)ErrCode.ERROR)
            {
                OutputLog("[FAILURE] Set up client. Not start.");
            }
            else
            {
                OutputLog("Set up client. Start FTP forwarder.");
            }

        }

        //---------------------------------------------------------------------------
        //  定周期で出力ファイル監視 & アップロード
        //
        //  説明：
        //  引数：
        //  戻値： 0: 設定完了 -1:設定失敗
        //---------------------------------------------------------------------------
        private void timMonitor_Tick(object sender, EventArgs e)
        {
            if(_Client.isReady() == false)
            {
                if (_Client.SetUpClient() == (int)ErrCode.ERROR)
                {
                    OutputLog("[FAILURE] Set up client. Not start.");
                    return;
                }
                else
                {
                    OutputLog("Set up client. Not start.");
                }

            }
            
            // アップロードするファイルのリスト取得
            List<string> Paths = new List<string>();
            Paths = CsvCtrl.GetUploadFilePathList();

            // 出力ファイル有無確認
            if(Paths.Count == 0)
            {
                return;
            }

            // FTP接続
            if (_Client.ConnectToServer() == (int)ErrCode.ERROR)
            {
                return;
            }

            // FTP側に送信残があるか確認
            if (_Client.isRemainCsvFile() == true)
            {
                return;
            }

            // Noticeファイルアップロード
            if(_Client.UploadNotice() == (int)ErrCode.ERROR) 
            {
                return;
            }

            // アップロード
            for(int i = 0; i < (Paths.Count / 2); i++)
            {
                if(_Client.Upload(Paths[i * 2], Paths[(i * 2) + 1]) == (int)ErrCode.SUCCESS)
                {
                    CsvCtrl.DeleteFile(Paths[(i * 2) + 1]);
                }
            }

            // Noticeファイル削除
            if(_Client.DeleteNotice() == (int)ErrCode.ERROR)
            {
                return;
            }

            // FTP切断
            if(_Client.DisConnectToServer() == (int)ErrCode.ERROR)
            {
                return;
            }
        }

        //---------------------------------------------------------------------------
        //  ログ表示
        //
        //  説明：
        //  引数：ログ文字列
        //  戻値： 
        //---------------------------------------------------------------------------
        public void OutputLog(string strLog)
        {
            DateTime dtNow = DateTime.Now;
            int nLineCount = txtMessage.Lines.Length;

            if (nLineCount >= 500)
            {
                List<string> lines = new List<string>(txtMessage.Lines);
                lines.RemoveAt(0); // 最古行削除
                txtMessage.Text = String.Join("\r\n", lines.ToArray());
            }
            txtMessage.Text += "[" + dtNow.ToString("yyyy/MM/dd HH:mm:ss") + "] " + strLog + "\n";

            //カレット位置を末尾に移動
            txtMessage.SelectionStart = txtMessage.Text.Length;
            //テキストボックスにフォーカスを移動
            txtMessage.Focus();
            //カレット位置までスクロール
            txtMessage.ScrollToCaret();

        }

        public Object GetFormObj()
        {
            return this;
        }
    }
}
