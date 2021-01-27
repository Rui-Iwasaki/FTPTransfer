using System;
using System.Collections.Generic;
using System.IO;

namespace FTPTransfer
{
    public static class CsvCtrl
    {
        //---------------------------------------------------------------------------
        //  転送ファイル有無と一覧作成
        //
        //  説明：
        //  引数： 
        //  戻値： ファイル一覧
        //---------------------------------------------------------------------------
        public static List<string> GetUploadFilePathList()
        {
            List<string> Paths = new List<string>();
            string strFileName = "";
            FileInfo file;
            long lTotalfileSize = 0;

            try
            {
                // ファイル出力先
                string strCsvPath = IniFile.GetValueString("OutputCSVFile", "OutputCSVFolderPath");
                // 最大アップロード容量
                long lLimitSize = IniFile.GetValueInt("FTP", "FtpFopFileSizeLimit"); ;

                if(Directory.Exists(strCsvPath) == false)
                {
                    return Paths;
                }

                // 出力トレンドファイル有無の確認
                string[] arryTrend = Directory.GetFiles(strCsvPath, "*_TREND.csv");
                string[] arryEvent = Directory.GetFiles(strCsvPath, "*_EVENT.csv");

                if (arryTrend.Length == 0 && arryEvent.Length == 0)
                {
                    return Paths;
                }


                int nTrendCnt, nEventCnt;
                bool isNonTrendFile, isNonEventFile;
                nTrendCnt = 0;
                nEventCnt = 0;
                isNonTrendFile = false;
                isNonEventFile = false;

                // 1つずつファイルの容量を確認し、制限内であればファイルの名前とパスを
                // リストに加えていく
                while (true)
                {
                    if (arryTrend.Length > nTrendCnt)
                    {
                        strFileName = arryTrend[nTrendCnt];
                        file = new FileInfo(strFileName);
                        if ((lTotalfileSize + file.Length) < lLimitSize)
                        {
                            Paths.Add(Path.GetFileName(strFileName));
                            Paths.Add(strFileName);
                            nTrendCnt++;
                        }
                        lTotalfileSize += file.Length;

                    }
                    else
                    {
                        isNonTrendFile = true;
                    }

                    if (arryEvent.Length > nEventCnt)
                    {
                        strFileName = arryEvent[nEventCnt];
                        file = new FileInfo(strFileName);
                        if ((lTotalfileSize + file.Length) < lLimitSize)
                        {
                            Paths.Add(Path.GetFileName(strFileName));
                            Paths.Add(strFileName);
                            nEventCnt++;
                        }
                        lTotalfileSize += file.Length;
                    }
                    else
                    {
                        isNonEventFile = true;
                    }

                    if (lTotalfileSize >= lLimitSize)
                    {
                        break;
                    }

                    if(isNonTrendFile == true && isNonEventFile == true)
                    {
                        break;
                    }
                }
                
            }
            catch(Exception ex)
            {

            }

            return Paths;
        }

        //---------------------------------------------------------------------------
        //  ファイル削除
        //
        //  説明：
        //  引数： 削除ファイルパス
        //  戻値： 
        //---------------------------------------------------------------------------
        public static void DeleteFile(string strFilePath)
        {
            File.Delete(strFilePath);
        }

    }
}
