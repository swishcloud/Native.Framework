﻿using Native.Core.Domain;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using QQRobot.Ui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Unity;

namespace Native.Core
{
    /// <summary>
    /// 酷Q应用主入口类
    /// </summary>
    public class CQMain
    {
        /// <summary>
        /// 在应用被加载时将调用此方法进行事件注册, 请在此方法里向 <see cref="IUnityContainer"/> 容器中注册需要使用的事件
        /// </summary>
        /// <param name="container">用于注册的 IOC 容器 </param>
        public static void Register(IUnityContainer unityContainer)
        {
            // 在 Json 中, 群消息的 name 字段是: 群消息处理, 因此这里注册的第一个参数也是这样填写
            unityContainer.RegisterType<IGroupMessage, Event_GroupMessage>("群消息处理");

            // 在这里添加窗体菜单的类注册
            unityContainer.RegisterType<IMenuCall, Menu_OpenWindow>("open");

            // 在这里添加窗体菜单的类注册
            unityContainer.RegisterType<ICQStartup, CQStartup>("酷Q启动事件");
        }
    }

    internal class CQStartup : ICQStartup
    {
        public CQStartup()
        {
        }
        void ICQStartup.CQStartup(object sender, CQStartupEventArgs e)
        {
            Facade.Initialize(new QQAPI());
            Facade.OpenMainForm();
        }
    }

    internal class QQAPI : IQQAPI
    {
        CQApi cQApi;
        public QQAPI()
        {
            this.cQApi = AppData.UnityContainer.Resolve<CQApi>("com.example.music"); 
        }
        public bool SetGroupMemberBanSpeak(long groupId, long qqId, TimeSpan time)
        {
            return cQApi.SetGroupMemberBanSpeak(groupId, qqId, time);
        }
        public bool RecallMessage(int msgId)
        {
            return cQApi.RemoveMessage(msgId);
        }
        public int SendGroupMessage(long groupId, string message)
        {
            return cQApi.SendGroupMessage(groupId,message);
        }
    }

    internal class Menu_OpenWindow : IMenuCall
    {
        public void MenuCall(object sender, CQMenuCallEventArgs e)
        {
            Facade.OpenMainForm();
        }
    }

    internal class Event_GroupMessage : IGroupMessage
    {
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            var imagePaths = new List<string>();
            foreach (var c in e.Message.CQCodes)
            {
                if (c.Function == Sdk.Cqp.Enum.CQFunction.Image)
                {
                    string filePath = e.Message.CQApi.ReceiveImage(c);
                    if (File.Exists(filePath))
                    {
                        Thread.Sleep(5000);
                        filePath = e.Message.CQApi.ReceiveImage(c);
                        if (File.Exists(filePath))
                        {
                            Facade.Log($"retry receiving image succeed, 5 seconds after first receiving image failure,path:{filePath}");
                        }
                        else
                        {
                            Facade.Log($"retry receiving image failed, 5 seconds after first receiving image failure,path:{filePath},the process for this image will be abandoned");
                            continue;
                        }
                    }
                    imagePaths.Add(filePath);
                }
            }
            var msg = e.Message.Text;
            msg = Regex.Replace(msg, @"\[CQ:[^\]]+\]", "", RegexOptions.Singleline);
            Facade.ProcessGroupMessage(new QQRobot.Ui.Models.GroupMessage { Desc = e.ToString(), Message =msg,QQId=e.FromQQ.Id,GroupId=e.FromGroup.Id,Id=e.Message.Id,ImagePaths=imagePaths.ToArray() });
        }
    }
}
