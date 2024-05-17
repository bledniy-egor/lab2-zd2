using System;
using System.Collections.Generic;
using System.Xml;

// Класс для представления заказа
public class Order
{
    public ShipTo ShipTo { get; set; }
    public List<Item> Items { get; set; }

    public Order()
    {
        Items = new List<Item>();
    }
}

// Класс для представления информации о доставке
public class ShipTo
{
    public string Name { get; set; }
    public string Street { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
}

// Класс для представления товара
public class Item
{
    public string Title { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

// Статический класс для преобразования XML в объект Order
public static class XmlToOrderConverter
{
    public static Order ConvertFromXml(string xmlString)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlString);

        Order order = new Order();

        // ... (код для ShipTo)

        // Заполнение списка товаров
        XmlNodeList itemNodes = doc.SelectNodes("/shipOrder/items/item");
        foreach (XmlNode itemNode in itemNodes)
        {
            int quantity;
            decimal price;

            // Обработка quantity с TryParse()
            if (int.TryParse(itemNode.SelectSingleNode("quantity").InnerText.Trim(), out quantity))
            {
                // Преобразование quantity прошло успешно

                // Обработка price с TryParse()
                if (decimal.TryParse(itemNode.SelectSingleNode("price").InnerText, out price))
                {
                    // Преобразование price прошло успешно

                    order.Items.Add(new Item
                    {
                        Title = itemNode.SelectSingleNode("title").InnerText,
                        Quantity = quantity,
                        Price = price
                    });
                }
                else
                {
                    Console.WriteLine($"Ошибка: Некорректный формат строки для price: {itemNode.SelectSingleNode("price").InnerText}");
                }
            }
            else
            {
                Console.WriteLine($"Ошибка: Некорректный формат строки для quantity: {itemNode.SelectSingleNode("quantity").InnerText}");
            }
        }

        return order;
    }
}
// Пример использования
public class Program
{
    public static void Main(string[] args)
    {
        string xml = @"<?xml version=""1.0"" ?>
<shipOrder>
<shipTo>
<name>Tove Svendson</name>
<street>Ragnhildvei 2</street>
<address>4000 Stavanger</address>
<country>Norway</country>
</shipTo>
<items>
<item>
<title>Empire Burlesque</title>
<quantity> 1</quantity>
<price>10.90</price>
</item>
<item>
<title>Hide your heart</title>
<quantity>1</quantity>
<price>9.90</price>
</item>
</items>
</shipOrder>";

        Order order = XmlToOrderConverter.ConvertFromXml(xml);

        Console.WriteLine("Информация о заказе:");
        Console.WriteLine($"Имя получателя: {order.ShipTo.Name}");
        Console.WriteLine($"Адрес доставки: {order.ShipTo.Street}, {order.ShipTo.Address}, {order.ShipTo.Country}");
        Console.WriteLine("Товары:");
        foreach (Item item in order.Items)
        {
            Console.WriteLine($"Название: {item.Title}, Количество: {item.Quantity}, Цена: {item.Price}");
        }
    }
}