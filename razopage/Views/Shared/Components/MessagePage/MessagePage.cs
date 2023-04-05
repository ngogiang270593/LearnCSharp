using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Views.Shared.Components.MessagePage
{
     [ViewComponent]
    public class MessagePage: ViewComponent
    {
         public const string COMPONENTNAME = "MessagePage";
        // Dữ liệu nội dung trang thông báo
        public MessagePage() {

        }
        public IViewComponentResult Invoke(Message message) {
            // Thiết lập Header của HTTP Respone - chuyển hướng về trang đích
            this.HttpContext.Response.Headers.Add("REFRESH",$"{message.secondwait};URL={message.urlredirect}");
            return  View(message);
        }
    }
}