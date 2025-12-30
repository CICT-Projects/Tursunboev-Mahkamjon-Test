using System.Text.Json.Serialization;

namespace backend.Models;

public class Part
{
    public int Id { get; set; }
    public string? Name { get; set; } // Название запчасти
    public decimal Price { get; set; } // Цена
    public string? Type { get; set; } // Тип (двигатель, кузов, электроника, салон)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Внешний ключ
    public int CarId { get; set; }
    
    // Не сериализуем обратную ссылку на Car
    [JsonIgnore]
    public Car? Car { get; set; }
}
