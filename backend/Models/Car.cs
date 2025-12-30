namespace backend.Models;

public class Car
{
    public int Id { get; set; }
    public string? Name { get; set; } // Название машины
    public string? Brand { get; set; } // Марка
    public int Year { get; set; } // Год
    public decimal Price { get; set; } // Цена
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Связь с запчастями
    public ICollection<Part> Parts { get; set; } = new List<Part>();
}
