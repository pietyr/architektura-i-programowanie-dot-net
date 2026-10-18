using System.ComponentModel;

namespace lab3.Models;

public class Fruit
{
    [DisplayName("Id")]
    public int Id { get; set; }

    [DisplayName("Nazwa")]
    public required string Name { get; init; }

    [DisplayName("Słodki")]
    public bool IsSweet { get; init; }

    [DisplayName("Cena")]
    public decimal Price { get; init; }

    public static Fruit Create()
    {
        Random r = Random.Shared;
        string[] names =
        [
            "Apple",
            "Banana",
            "Cherry",
            "Durian",
            "Elderberry",
            "Grape",
            "Jackfruit"
        ];

        return new Fruit
        {
            Name = names[r.Next(names.Length)],
            IsSweet = r.NextDouble() > 0.5,
            Price = Math.Round((decimal)(r.NextDouble() * 10), 2)
        };
    }
}
