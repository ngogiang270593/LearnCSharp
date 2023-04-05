using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Pages.Blog
{
    //[Authorize(Roles = "Adminstrator,Editor")]

    //[Authorize(Roles = "Adminstrator")]
    //[Authorize(Roles = "Editor")]
    //[Authorize(Policy="CanViewTest")]
    //[Authorize(Policy="CanView")]
    [Authorize(Policy="InGenZ")]
    public class IndexModel : PageModel
    {
        private readonly Identity.Models.AppDbContext _context;

        public IndexModel(Identity.Models.AppDbContext context)
        {
            _context = context;
        }
        public const int ITEMS_PER_PAGE = 10;
        [BindProperty(SupportsGet = true,Name = "p")]
        public int CurrentPage {get;set;}
        public int CountPages {get;set;}
        public IList<Article> Article { get;set; } = default!;


        public async Task OnGetAsync(string SearchString)
        {
            if (_context.Articles != null)
            {
                int totalArticle =  await _context.Articles.CountAsync();
                CountPages = (int)Math.Ceiling((double)totalArticle/ ITEMS_PER_PAGE);
                if(CurrentPage < 1) CurrentPage = 1;
                var qr = from a in _context.Articles
                         orderby a.PublishDate descending
                         select a;
                if(!string.IsNullOrEmpty(SearchString)){
                    Article  =  await qr.Where(a => a.Title.Contains(SearchString)).Skip((CurrentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
                }else{
                    Article = await _context.Articles.Skip((CurrentPage - 1) * ITEMS_PER_PAGE).Take(ITEMS_PER_PAGE).ToListAsync();
                }
            }
        }
    }
}
