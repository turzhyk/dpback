using DPBack.Domain.Enums.Products;

namespace DPBack.Domain.Models.Products;

public class LetterConfig
{
    public Letter.Size Size { get; set; }
    public Letter.Color OuterColor { get; set; }
    public Letter.Color InnerColor { get; set; }
    public int Template { get; set; }
    public Letter.Thickness Thickness { get; set; }
}