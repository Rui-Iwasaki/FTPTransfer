using System;
using System.IO;

namespace FTPTransfer
{
    public class ErrorFilePath
    {
        public const string ERROR_FILE_DIL = @"\ErrLog";
        public const string ERROR_FILE_NAME = @"\FtpErrorLog.txt";
    }

    public static class ErrorLog
    {
        //---------------------------------------------------------------------------
        //  エラーログ書き込み処理
        //
        //  説明：
        //  引数：strErrMsg：エラーメッセージ
        //  戻値：
        //---------------------------------------------------------------------------
        public static void WriteErrorLog(String strErrMsg)
        {
            string strFilePath = Directory.GetCurrentDirectory();
            DateTime nowDt = DateTime.Now;

            strFilePath += ErrorFilePath.ERROR_FILE_DIL + ErrorFilePath.ERROR_FILE_NAME;

            CreateErrLogDirectory();

            File.AppendAllText(strFilePath, "[" + nowDt.ToString("yyyy/MM/dd hh:mm:ss") + "] " + strErrMsg + "\n");
        }

        //---------------------------------------------------------------------------
        //  エラーログ保存ディレクトリ処理
        //
        //  説明：エラーログ保存用ディレクトリがなければ作成
        //  引数：
        //  戻値：
        //---------------------------------------------------------------------------
        private static void CreateErrLogDirectory()
        {
            string strDilPath = Directory.GetCurrentDirectory();

            strDilPath += ErrorFilePath.ERROR_FILE_DIL;

            if (!Directory.Exists(strDilPath))
            {
                Directory.CreateDirectory(strDilPath);
            }

        }
    }
}
