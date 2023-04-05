using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{

    [Route("learnasp2020")]
    public class LearnAspController : Controller
    {
        //[HttpGet("/hoc-lap-trinh-asp/{id:int?}/")]
        [HttpGet("/hoc-lap-trinh-asp/{id:int?}/", Name = "learnasproute")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("bai-kiem-tra")]  //hoặc
        [Route("test/{id?}")] //hoặc
        [Route("/kiem-tra-ngay")] //hoặc
        [Route("abc-[controller]-xyz[action]")] // Url phù hợp = /abc-learnasp-xyztest
        [AcceptVerbs("GET", "POST", "PUT")]//Chấp nhận các phương thức
        public IActionResult Test()
        {
            return Content("Kiểm tra route");
        }
    }
}