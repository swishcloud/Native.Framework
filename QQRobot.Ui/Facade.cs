﻿using QQRobot.Ui.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZXing;

namespace QQRobot.Ui
{
    public static class Facade
    {
        private static IQQAPI QQAPI { get; set; }
        public static void Initialize(IQQAPI qqApi)
        {
            QQAPI = qqApi;
        }
        public static void OpenMainForm()
        {
            MainForm.Instance.Show();
        }

        public static void ProcessGroupMessage(GroupMessage groupMessage)
        {
            MainForm.Instance.Log(groupMessage.Desc);
            MainForm.Instance.Log("msg:"+groupMessage.Message);
            File.AppendAllText(Config.path_log,$"desc:{groupMessage.Desc}\r\nmsg:{groupMessage.Message}\r\n");


            //check message if contains any image with barcode,if true then ban the message sender speaking for 5 days
            foreach (var p in groupMessage.ImagePaths)
            {
                // create a barcode reader instance
                IBarcodeReader reader = new BarcodeReader();
                // load a bitmap
                var barcodeBitmap = (Bitmap)Image.FromFile(p);
                // detect and decode the barcode inside the bitmap
                var result = reader.Decode(barcodeBitmap);
                // do something with the result
                if (result != null)
                {
                    var str = $"format:{ result.BarcodeFormat.ToString()} content:{ result.Text}";
                    MainForm.Instance.Log("decoded barcode " + str);

                    //ban speaking
                    MainForm.Instance.Log($"{groupMessage.QQId} triggered ban speaking rule:prohibit sending barcode,baned for 5 days");
                    if (!QQAPI.SetGroupMemberBanSpeak(groupMessage.GroupId, groupMessage.QQId, TimeSpan.FromDays(5)))
                    {
                        MainForm.Instance.Log("ban failed");
                    }
                    if (!QQAPI.RecallMessage(groupMessage.Id))
                    {
                        MainForm.Instance.Log("recall message failed");
                    }
                    QQAPI.SendGroupMessage(groupMessage.GroupId, "Forbidding Advertising!");
                    return;
                }
            }

            //check message content to decide if need to ban the message sender
            var regexs = Config.Instance.ReadRegexsConfig();
            var time = 0;
            var regex = "";
            foreach (var i in regexs)
            {
                var r = Regex.Match(i, "[^ ]+").Value;
                if (!int.TryParse(Regex.Match(i.Trim(Convert.ToChar(8236)), " \\d+ *$").Value, out var t))
                {
                    MainForm.Instance.Log($"rule {i} is invalid,pls check config,each rule must begin with regex expression without any empty characters,followed by number which indicate ban speaking time,the two parts is separated by a empty string");
                    continue;
                }
                if (Regex.IsMatch(groupMessage.Message.Replace("\r\n",""), r, RegexOptions.Singleline))
                {
                    if (t > time)
                    {
                        time = t;
                        regex = r;
                    }
                }
            }
            if (time > 0)
            {
                MainForm.Instance.Log($"{groupMessage.QQId} triggered ban speaking rule:{regex},baned for {time} seconds");
                if(!QQAPI.SetGroupMemberBanSpeak(groupMessage.GroupId, groupMessage.QQId, TimeSpan.FromSeconds(time)))
                {
                    MainForm.Instance.Log("ban failed");
                }
                if (!QQAPI.RecallMessage(groupMessage.Id))
                {
                    MainForm.Instance.Log("recall message failed");
                }
                QQAPI.SendGroupMessage(groupMessage.GroupId, "Forbidding Advertising!");
                return;
            }
        }
    }
}
