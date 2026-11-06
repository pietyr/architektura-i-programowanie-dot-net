using System.ComponentModel.DataAnnotations;

namespace lab5.Models;

public class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    [UIHint("LongText")]
    public string Description { get; set; } = string.Empty;

    [UIHint("Stars")]
    public int Rating { get; set; }

    [UIHint("TrailerLink")]
    public string TrailerLink { get; set; } = string.Empty;
}
