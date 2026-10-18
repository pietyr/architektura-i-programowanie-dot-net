namespace lab3.Models;

public class FruitService
{
    private readonly List<Fruit> fruits;

    public FruitService()
    {
        fruits = new List<Fruit>();
        for (int i = 0; i < 10; i++)
        {
            Fruit fruit = Fruit.Create();
            fruit.Id = i + 1;
            fruits.Add(fruit);
        }
    }

    public IEnumerable<Fruit> GetFruits()
    {
        return fruits;
    }

    public void Add(Fruit fruit)
    {
        // Pusta lista: Max() rzuciłoby wyjątek – stąd osobna ścieżka.
        fruit.Id = fruits.Count == 0 ? 1 : fruits.Max(x => x.Id) + 1;
        fruits.Add(fruit);
    }

    public bool Remove(int id)
    {
        Fruit? fruit = fruits.FirstOrDefault(f => f.Id == id);
        if (fruit is null)
        {
            return false;
        }

        fruits.Remove(fruit);
        return true;
    }
}
