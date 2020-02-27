using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQRobot.Ui
{
    public interface IQQAPI
    {
        bool SetGroupMemberBanSpeak(long groupId, long qqId, TimeSpan time);
        bool RecallMessage(int msgId);
        int SendGroupMessage(long groupId, string message);
    }
}
