using System.IO;

namespace FTPTransfer
{
    static class NoticeFile
    {
        //---------------------------------------------------------------------------
        //  Noticeファイル作成処理
        //
        //  説明：
        //  引数：
        //  戻値： 0: 作成成功 -1:作成失敗
        //---------------------------------------------------------------------------
        public static int CreateNoticeFile()
        {
            if(!File.Exists(Directory.GetCurrentDirectory() + @"\notice.txt"))
            {
                StreamWriter sw = File.CreateText(Directory.GetCurrentDirectory() + @"\notice.txt");
                sw.Close();
            }

            return 0;
        }

        //---------------------------------------------------------------------------
        //  Noticeファイルのパス
        //
        //  説明：
        //  引数：
        //  戻値：
        //---------------------------------------------------------------------------
        public static string GetNoticeFilePath()
        {
            return Directory.GetCurrentDirectory() + @"\notice.txt";
        }

        //---------------------------------------------------------------------------
        //  Noticeファイル名
        //
        //  説明：
        //  引数：
        //  戻値：
        //---------------------------------------------------------------------------
        public static string GetNoticeFileName()
        {
            return "notice.txt";
        }
    }
}
