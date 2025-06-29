namespace BookCurationApp_V1.Models;

public class Book
{
    public string Title { get; set; } = string.Empty;
    public List<string> Authors { get; set; } = new();
    public string Thumbnail { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string AgeRange { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty;
    public string DifficultyColor { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string PublishedDate { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<PurchaseLink> PurchaseLinks { get; set; } = new();

    // 表示用プロパティ
    public string AuthorsText => Authors.Any() ? string.Join(", ", Authors) : "Unknown Author";
    public string DisplayThumbnail => !string.IsNullOrEmpty(Thumbnail) ? Thumbnail : "https://via.placeholder.com/200x300/7CC576/FFFFFF?text=📚+No+Image";
    public string DisplayAgeRange => !string.IsNullOrEmpty(AgeRange) ? AgeRange : "年齢不明";
}

public class PurchaseLink
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}