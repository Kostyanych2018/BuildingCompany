namespace BuildingCompany.Domain.Entities;

public class Material : Entity
{
    public string Name { get; set; }
    public string UnitOfMeasure { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    
    public Material (){}
    public Material(string name,
        string unitOfMeasure,
        decimal unitPrice,
        decimal initialQuantity)
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
        Quantity = initialQuantity;
    }

    public void UpdateDetails(string name, string unitOfMeasure, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название материала не может быть пустым.", nameof(name));
        if (string.IsNullOrWhiteSpace(unitOfMeasure))
            throw new ArgumentException("Единица измерения не может быть пустой.", nameof(unitOfMeasure));
        if (unitPrice < 0)
            throw new ArgumentException("Цена за единицу не может быть отрицательной.", nameof(unitPrice));

        Name = name;
        UnitOfMeasure = unitOfMeasure;
        UnitPrice = unitPrice;
    }

    public void Add(decimal quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Количество для добавления не может быть отрицательным.", nameof(quantity));
        Quantity += quantity;
    }

    public void Remove(decimal quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Количество для удаления не может быть отрицательным.", nameof(quantity));
        if (Quantity < quantity)
            throw new InvalidOperationException($"Недостаточно материала '{Name}' на складе." +
                                                $" Доступно: {Quantity} {UnitOfMeasure}, Требуется удалить: {quantity} {UnitOfMeasure}");
        Quantity-=quantity;
    }
}