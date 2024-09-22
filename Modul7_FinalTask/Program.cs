using System;

abstract class Delivery
{
    public string Address { get; set; }
    public abstract void Deliver();
}

class HomeDelivery : Delivery
{
    public string CourierName { get; set; }
    public override void Deliver()
    {
        Console.WriteLine("Доставка на дом курьером {0} по адресу {1}", CourierName, Address);
    }
}

class PickPointDelivery : Delivery
{
    public string PickPointLocation { get; set; }
    public string Company { get; set; }
    public override void Deliver()
    {
        Console.WriteLine("Доставка в пункт выдачи {0}, компания: {1}", PickPointLocation, Company);
    }
}

class ShopDelivery : Delivery
{
    public string ShopName { get; set; }
    public override void Deliver()
    {
        Console.WriteLine("Доставка в магазин {0} по адресу {1}", ShopName, Address);
    }
}

class Product
{
    public string Name { get; set; }
    public double Price { get; set; }

    public Product(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"Товар {Name} за {Price}";
    }

    public static double operator +(double total, Product product)
    {
        return total + product.Price;
    }
}

class Order<TDelivery> where TDelivery : Delivery
{
    public TDelivery Delivery { get; set; }
    public int Number { get; set; }
    private Product[] Products { get; set; }

    public Order(TDelivery delivery, int number, Product[] products)
    {
        Delivery = delivery;
        Number = number;
        Products = products;
    }

    public void DisplayOrderDetails()
    {
        Console.WriteLine("Order #{0}:", Number);
        foreach (var product in Products)
        {
            Console.WriteLine(product);
        }
        Delivery.Deliver();
    }

    public double GetTotalPrice()
    {
        double total = 0;
        foreach (var product in Products)
        {
            total += product;
        }
        return total;
    }
}

static class OrderExtensions
{
    public static void DisplayTotalPrice(this Order<Delivery> order)
    {
        Console.WriteLine("Total Price: {0}", order.GetTotalPrice());
    }
}

class Program
{
    static void Main()
    {
        Delivery delivery = null;

        while (delivery == null)
        {
            Console.WriteLine("Введите тип доставки (1 - Дом, 2 - Пункт выдачи, 3 - Магазин): ");
            int deliveryType;
            if (int.TryParse(Console.ReadLine(), out deliveryType))
            {
                switch (deliveryType)
                {
                    case 1:
                        Console.WriteLine("Введите адрес доставки на дом:");
                        string homeAddress = Console.ReadLine();
                        Console.WriteLine("Введите имя курьера:");
                        string courierName = Console.ReadLine();
                        delivery = new HomeDelivery { Address = homeAddress, CourierName = courierName };
                        break;
                    case 2:
                        Console.WriteLine("Введите адрес пункта выдачи:");
                        string pickPointLocation = Console.ReadLine();
                        Console.WriteLine("Введите название компании:");
                        string company = Console.ReadLine();
                        delivery = new PickPointDelivery { Address = pickPointLocation, PickPointLocation = pickPointLocation, Company = company };
                        break;
                    case 3:
                        Console.WriteLine("Введите адрес магазина:");
                        string shopAddress = Console.ReadLine();
                        Console.WriteLine("Введите название магазина:");
                        string shopName = Console.ReadLine();
                        delivery = new ShopDelivery { Address = shopAddress, ShopName = shopName };
                        break;
                    default:
                        Console.WriteLine("Неверный тип доставки. Попробуйте снова.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Ошибка: введите числовое значение.");
            }
        }

        Console.WriteLine("Введите количество товаров в заказе:");
        int productCount;
        while (!int.TryParse(Console.ReadLine(), out productCount) || productCount <= 0)
        {
            Console.WriteLine("Ошибка: введите положительное число.");
        }

        Product[] products = new Product[productCount];

        for (int i = 0; i < productCount; i++)
        {
            Console.WriteLine("Введите название товара {0}:", i + 1);
            string productName = Console.ReadLine();
            double productPrice;
            Console.WriteLine("Введите цену товара {0}:", i + 1);
            while (!double.TryParse(Console.ReadLine(), out productPrice) || productPrice <= 0)
            {
                Console.WriteLine("Ошибка: введите положительное значение цены.");
            }
            products[i] = new Product(productName, productPrice);
        }

        var order = new Order<Delivery>(delivery, 1, products);
        order.DisplayOrderDetails();
        order.DisplayTotalPrice();
    }
}