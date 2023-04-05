using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Views.Shared.Components.MessagePage
{
     public class Message {
        public string title {set; get;} = "Thông báo";     // Tiêu đề của Box hiện thị
        public string htmlcontent {set; get;} = "";         // Nội dung HTML hiện thị
        public string urlredirect {set; get;} = "/";        // Url chuyển hướng đến
        public int secondwait {set; get;} = 3;              // Sau secondwait giây thì chuyển
    }
}