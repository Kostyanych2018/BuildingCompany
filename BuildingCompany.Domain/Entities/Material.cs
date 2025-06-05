namespace BuildingCompany.Domain.Entities;

public class Material : Entity
{
    public string Name { get; set; }
    public string UnitOfMeasure { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; } = "Стандарт"; 
    
    public Material (){}
    public Material(string name,
        string unitOfMeasure,
        decimal unitPrice,
        decimal finalPrice,
        int initialQuantity,
        string category = "Стандарт")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название материала не может быть пустым.", nameof(name));
        if (string.IsNullOrWhiteSpace(unitOfMeasure))
            throw new ArgumentException("Единица измерения не может быть пустой.", nameof(unitOfMeasure));
        if (unitPrice < 0)
            throw new ArgumentException("Цена за единицу не может быть отрицательной.", nameof(unitPrice));
        if (initialQuantity < 0)
            throw new ArgumentException("Начальный остаток не может быть отрицательным.", nameof(initialQuantity));
        Name = name;
        UnitOfMeasure = unitOfMeasure;
        UnitPrice = unitPrice;
        FinalPrice = finalPrice;
        Quantity = initialQuantity;
        Category = category;
    }

    public void UpdateDetails(string name, string unitOfMeasure, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название материала не может быть пустым.", nameof(name));
        if (string.IsNullOrWhiteSpace(unitOfMeasure))
            throw new ArgumentException("Единица измерения не может быть пустой.", nameof(unitOfMeasure));

        Name = name;
        UnitOfMeasure = unitOfMeasure;
        Quantity = quantity;
    }

    public void Add(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Количество для добавления не может быть отрицательным.", nameof(quantity));
        Quantity += quantity;
    }

    public void Remove(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Количество для удаления не может быть отрицательным.", nameof(quantity));
        if (Quantity < quantity)
            throw new InvalidOperationException($"Недостаточно материала '{Name}' на складе." +
                                                $" Доступно: {Quantity} {UnitOfMeasure}, Требуется удалить: {quantity} {UnitOfMeasure}");
        Quantity-=quantity;
    }
}