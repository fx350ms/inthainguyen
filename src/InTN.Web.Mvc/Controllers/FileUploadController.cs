using InTN.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Abp.Authorization;

namespace InTN.Web.Controllers
{
 
    public class FileUploadController : InTNControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public IActionResult Upload()
        //{
        //    return Ok();
        //}
        

        [HttpPost]
        public async Task<IActionResult> UploadSingleFile(IFormFile file)
        {
            // Kiểm tra xem có file nào được gửi lên không

            return Ok();
        }
        //public IActionResult Upload()
        //{
        //    if (HttpContext.Request.Form.Files.Count == 0)
        //    {
        //        return BadRequest(new { success = false, message = "Không có file nào được gửi lên." });
        //    }


        //    return Ok();
        //    // ... logic xử lý file tương tự như trên với IFormFile ...
        //    //  return Ok(new { success = true, message = "File đã được tải lên thành công." });
        //}
    }
}
