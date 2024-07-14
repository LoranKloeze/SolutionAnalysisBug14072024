using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SolutionAnalysisBug.Models;


 [Index(nameof(ParentId), Name = "index_wiki_articles_on_parent_id")]
 [Index(nameof(Slug), Name = "index_wiki_articles_on_slug", IsUnique = true)]
public class WikiArticle
{
    public int Id { get; set; }
    
    [StringLength(512)]
    public required string Title { get; set; }

    [StringLength(50000)]
    public string Content { get; set; } = "";
    
    [StringLength(512)]
    public required string Slug { get; set; }
    public int? ParentId { get; set; }
    public WikiArticle? Parent { get; set; }
    public List<WikiArticle> Children { get; set; } = [];
    
    public List<string>? RoleNames { get; set; } = [];
    
    public static int MaxLevels { get; set; } = 4;

}