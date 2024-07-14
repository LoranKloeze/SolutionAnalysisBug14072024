using InterPedia.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SolutionAnalysisBug.Models;

namespace SolutionAnalysisBug.Pages;

public class Permissions(ApplicationDbContext context) : PageModel
{

    public List<WikiArticle> RootArticles { get; set; } = [];

    [BindProperty] public InputModel Input { get; set; } = new();
    
    public async Task OnGetAsync()
    {
        var query = context.WikiArticles
            .Where(a => a.ParentId == null)
            .Include(a => a.Children);
        for (var i = 0; i < WikiArticle.MaxLevels; i++)
        {
            query = query.ThenInclude(a => a.Children);
        }
        
        RootArticles = await query.ToListAsync();

    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var permissionsPerArticle = CreatePermissionsDictionary();
        foreach (var wikiArticle in await context.WikiArticles.ToListAsync())
        {
            wikiArticle.RoleNames = 
                permissionsPerArticle.TryGetValue(wikiArticle.Id, out var roles) ? roles : [];
        }

        await context.SaveChangesAsync();
        return RedirectToPage("/Index");
    }

    private Dictionary<int, List<string>> CreatePermissionsDictionary()
    {
        Dictionary<int, List<string>> permissionsPerArticle = new();
        foreach (var permissionPair in Input.Permissions)
        {
            var splitted = permissionPair.Split("-");
            var role = splitted[0];

            var id = int.Parse(splitted[1]);
            if (!permissionsPerArticle.TryGetValue(id, out var roles))
            {
                roles = [];
                permissionsPerArticle[id] = roles;
            }
            roles.Add(role);
        }
        return permissionsPerArticle;
    }

    public class InputModel
    {
        public List<string> Permissions { get; set; } = [];
    }
    
    public static IIncludableQueryable<WikiArticle,List<WikiArticle>> IncludeChildren( 
        IIncludableQueryable<WikiArticle,List<WikiArticle>> query, int levels)
    {
        return levels <= 0 ? query : IncludeChildren(query.ThenInclude(a => a.Children), levels - 1);
    }

    
    
}
