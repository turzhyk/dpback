namespace DPBack.Domain.Enums.Products;

public class Businesscard
{
    public enum Thickness
    {
        T250,
        T300,
        T350
    }

    public enum Coating
    {
        Glossy,
        Matte,
        SoftTouch
    }

    public enum PrintType
    {
        BW,
        Color,
        None            
    }

}