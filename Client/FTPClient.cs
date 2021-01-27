using FluentFTP;
using System;
using System.Net;
using System.Security.Authentication;

namespace FTPTransfer
{
    class FTPClient
    {
        Form1 _Mainform;
        private FtpClient _Client;
        private bool _isCientSetupOK;

        public FTPClient(Form1 Mainform)
        {
            _Mainform = Mainform;
            _Client = new FtpClient();
            _isCientSetupOK = false;
        }

        //---------------------------------------------------------------------------
        //  FTP接続パラメータセット
        //
        //  説明：
        //  引数：
        //  戻値： 0: 設定完了 -1:設定失敗
        //---------------------------------------------------------------------------
        public int SetUpClient()
        {
            const string INI_SECTION = "FTP";
            const string INI_FTP_SERVER_KEY = "FtpServer";
            const string INI_FTP_SERVER_PORT_KEY = "FtpServerPort";
            const string INI_FTP_LOGIN_USER_NAME = "FtpUserName";
            const string INI_FTP_LOGIN_PASSWORD = "FtpPassword";
            const string INI_EXPLICT_SET = "ExploctSet";
            const string INI_PROTOCOL = "Protocol";
            // Explicit設定
            _Client.EncryptionMode = FtpEncryptionMode.None;//none
            // プロトコルはTls
            _Client.SslProtocols = SslProtocols.None;//tls


            string strReadStr1, strReadStr2, strReadStr3;
            int mReadvalue;

            // INIファイルからFTPサーバー情報とアクセスパラメータを読み込み
            strReadStr1 = "";
            strReadStr1 = IniFile.GetValueString(INI_SECTION, INI_FTP_SERVER_KEY);
            if (strReadStr1 != "")
            {
                _Client.Host = strReadStr1;

            }
            else
            {
                return -1;
            }

            mReadvalue = 0;
            mReadvalue = IniFile.GetValueInt(INI_SECTION, INI_FTP_SERVER_PORT_KEY);
            if (mReadvalue != 0)
            {
                _Client.Port = mReadvalue;

            }
            else
            {
                return -1;
            }

            strReadStr1 = ""; strReadStr2 = "";
            strReadStr1 = IniFile.GetValueString(INI_SECTION, INI_FTP_LOGIN_USER_NAME);
            strReadStr2 = IniFile.GetValueString(INI_SECTION, INI_FTP_LOGIN_PASSWORD);
            if (strReadStr1 != "" && strReadStr2 != "")
            {
                _Client.Credentials = new NetworkCredential(strReadStr1, strReadStr2);

            }
            else
            {
                return -1;
            }

            // 要求の完了後に接続を閉じる
            _Client.SocketKeepAlive = false;
            // Explicit設定
            mReadvalue = IniFile.GetValueInt(INI_SECTION, INI_EXPLICT_SET);
            _Client.EncryptionMode = (FtpEncryptionMode)mReadvalue;// FtpEncryptionMode.None;//none

            // プロトコルはNone
            mReadvalue = IniFile.GetValueInt(INI_SECTION, INI_PROTOCOL);
            _Client.SslProtocols = (SslProtocols)mReadvalue;//SslProtocols.None;//tls

            // 接続タイムアウトを5秒に設定
            _Client.ConnectTimeout = 5000;
            // 証明書の内容を確認しない
            _Client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);

            _isCientSetupOK = true;

            return 0;
        }
        private void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            // 証明書の内容を確認しない
            e.Accept = true;
        }

        //---------------------------------------------------------------------------
        //  クライアントセットアップ完了
        //
        //  説明：
        //  引数：
        //  戻値： True: 完了 False:未
        //---------------------------------------------------------------------------
        public bool isReady()
        {
            return _isCientSetupOK;
        }

        //---------------------------------------------------------------------------
        //  FTP接続
        //
        //  説明：
        //  引数：
        //  戻値： 0: 接続成功 -1:接続失敗
        //---------------------------------------------------------------------------
        public int ConnectToServer()
        {
            try
            {
                _Client.Connect();
                RecordFTPLog("Connect to server.");
                return 0;
            }
            catch (Exception ex)

            {
                ErrorLog.WriteErrorLog(ex.Message);
                RecordFTPLog("[FAILURE] Connect to server.");
            }
            return -1;
        }

        //---------------------------------------------------------------------------
        //  FTP切断
        //
        //  説明：
        //  引数：
        //  戻値： 0: 成功 -1:失敗
        //---------------------------------------------------------------------------
        public int DisConnectToServer()
        {
            try
            {
                _Client.Disconnect();
                RecordFTPLog("Disconnect to server.");
                return 0;
            }
            catch (Exception ex)

            {
                ErrorLog.WriteErrorLog(ex.Message);
                RecordFTPLog("[FAILURE] Disconnect to server.");
            }
            return -1;
        }

        //---------------------------------------------------------------------------
        //  FTP解放
        //
        //  説明：
        //  引数： 
        //  戻値： 0: 成功 -1:失敗
        //---------------------------------------------------------------------------
        public int DisposeConnecter()
        {
            try
            {
                _Client.Dispose();
                RecordFTPLog("Dispose client");
                return 0;
            }
            catch (Exception ex)

            {
                ErrorLog.WriteErrorLog(ex.Message);
                RecordFTPLog("[FAILURE] Dispose client");
            }
            return -1;
        }

        //---------------------------------------------------------------------------
        //  FTP転送処理
        //
        //  説明：
        //  引数： アップロード名称, アップロードするファイル
        //  戻値： 0: 接続成功 -1:接続失敗
        //---------------------------------------------------------------------------
        public int Upload(String strTargetFilePath, String strSourceFilePath)
        {
            try
            {
                _Client.UploadFile(strSourceFilePath, strTargetFilePath);
                RecordFTPLog("Upload file: " + strTargetFilePath);
                return 0;
            }
            catch (Exception ex)
            {
                ErrorProc(ex.Message);
                RecordFTPLog("[FAILURE] Upload file: " + strTargetFilePath);
            }
            return -1;
        }

        //---------------------------------------------------------------------------
        //  NoticeファイルFTP転送
        //
        //  説明：
        //  引数：
        //  戻値： 0: 成功 -1:失敗
        //---------------------------------------------------------------------------
        public int UploadNotice()
        {
            try
            {
                _Client.UploadFile(NoticeFile.GetNoticeFilePath(), NoticeFile.GetNoticeFileName());
                RecordFTPLog("Upload Notice file.");
                return 0;
            }
            catch (Exception ex)
            {
                ErrorProc(ex.Message);
                RecordFTPLog("[FAILURE] Upload Notice file.");
            }
            return -1;
        }

        //---------------------------------------------------------------------------
        //  Noticeファイル存在有無チェック
        //
        //  説明：
        //  引数：
        //  戻値： 0: 無  1: 有  -1: エラー
        //---------------------------------------------------------------------------
        public int isExistNoticeOnServer()
        {
            try
            {

                if (_Client.FileExists(NoticeFile.GetNoticeFileName()) == true)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
                // if (_Client.FileExists() == true)

            }
            catch (Exception ex)
            {
                ErrorProc(ex.Message);
                RecordFTPLog("[FAILURE] Check Notice file on server.");
            }
            return -1;
        }

        //---------------------------------------------------------------------------
        //  Noticeファイル削除
        //
        //  説明：
        //  引数：
        //  戻値： 0: 成功 -1:失敗
        //---------------------------------------------------------------------------
        public int DeleteNotice()
        {
            try
            {
                if (isExistNoticeOnServer() == 1)
                {
                    _Client.DeleteFile(NoticeFile.GetNoticeFileName());
                }

                return 0;
            }
            catch (Exception ex)
            {
                RecordFTPLog("[FAILURE] Delete Notice file on server.");
                ErrorProc(ex.Message);
            }
            return -1;
        }

        //---------------------------------------------------------------------------
        //  アップロードファイル残存確認
        //
        //  説明：
        //  引数：
        //  戻値： 0: 成功 -1:失敗
        //---------------------------------------------------------------------------
        public bool isRemainCsvFile()
        {
            try
            {


                if (isExistNoticeOnServer() == 1)   //noticeファイル確認
                {

                    DeleteNotice();                 //noticeファイルが残っていた場合削除
                    DisConnectToServer();
                    return true;
                }
                
                FtpListItem[] items = { };         //リストの初期化
                items = _Client.GetListing();      //FOP側のリストを取得
                if (items.Length != 0)             //CSVファイルが残っているか確認
                {
                    RecordFTPLog("[FAILURE] Files are still on server.");
                    DisConnectToServer();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                RecordFTPLog("[FAILURE] Cannot confirm files on server.");
                ErrorProc(ex.Message);
            }
            return false;
        }


        //---------------------------------------------------------------------------
        //  例外エラー処理
        //
        //  説明：
        //  引数：例外エラーメッセージ
        //  戻値：
        //---------------------------------------------------------------------------
        private void ErrorProc(string strErrMsg)
        {
            try
            {
                ErrorLog.WriteErrorLog(strErrMsg);
                this.DisConnectToServer();
                this.DisposeConnecter();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteErrorLog(ex.Message);
            }

        }

        //---------------------------------------------------------------------------
        //  エラーログ出力
        //
        //  説明：
        //  引数：エラーメッセージ
        //  戻値：
        //---------------------------------------------------------------------------
        private void RecordFTPLog(String strLog)
        {
            _Mainform.OutputLog(strLog);
        }

    }


}
