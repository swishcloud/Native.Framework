using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQRobot.Ui.Models
{
   public class GroupMessage
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public string Message { get;  set; }
        public long QQId{ get; set; }
        public long GroupId { get; set; }
        public string[] ImagePaths { get; set; }
    }
}
