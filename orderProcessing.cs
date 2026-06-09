using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrderProcessing;

public delegate void Notify(int id);

class Order
{
    public int orderId;
    public string customerName;
    public string productName;
    public int orderAmount;

    public Order(int id, string name, string pName, int totalAmaount)
    {
        orderId = id;
        customerName = name;
        productName = pName;
        orderAmount = totalAmaount;
    }
}

class Program
{
    static List<Order> orderList = new List<Order>();
    static List<Order> orderInProgress = new List<Order>();
    static Dictionary<int, Order> orderDictionary = new Dictionary<int, Order>();
    static Queue<Order> orderQueue = new Queue<Order>();
    static Hashtable productHash = new Hashtable(StringComparer.OrdinalIgnoreCase);

    static void AddProduct()
    {
        productHash.Add("Laptop", 85000);
        productHash.Add("Mobile", 45000);
        productHash.Add("Mouse", 1500);
        productHash.Add("Key Board", 2500);
        productHash.Add("Printer", 3500);
    }

    static void DisplayProduct()
    {
        Console.WriteLine("Product list And there Prices:");
        foreach (DictionaryEntry e in productHash)
        {
            Console.WriteLine($"{e.Key}: {e.Value}");
        }
    }

    public static void Main(string[] arg)
    {
        bool running = true;
        AddProduct();
        DisplayProduct();
        do
        {
            Console.WriteLine("\n1. Add new order");
            Console.WriteLine("2. View all order");
            Console.WriteLine("3. Process Next Order");
            Console.WriteLine("4. Search Order");
            Console.WriteLine("5. Display high value order");
            Console.WriteLine("6. Exit");

            Console.WriteLine("Select your choice: ");
            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1":
                    AddOrder();
                    break;
                case "2":
                    ViewAllOrder();
                    break;
                case "3":
                    ProcessNextOrder();
                    break;
                case "4":
                    SearchOrder();
                    break;
                case "5":
                    DisplayHighValueOrder();
                    break;
                case "6":
                    DisplayInProgress();
                    break;
                case "7":
                    DisplayRemaining();
                    break;
                case "8":
                    Console.WriteLine("Exiting...\n");
                    running = false;
                    break;
                default:
                    Console.WriteLine("Enter valid choice.\n");
                    break;
            }
        } while (running);
    }

    public static void AddOrder()
    {
        Console.WriteLine("Enter Order Id: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Enter valid order id.\n");
            return;
        }
        if (orderList.Any(e => e.orderId == id))
        {
            Console.WriteLine("Record already found.");
            return;
        }

        Console.WriteLine("Enter customer name: ");
        string inputName = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputName))
        {
            Console.WriteLine("Name cannot be empty.\n");
            return;
        }

        Console.WriteLine("Enter product name: ");
        string inputProduct = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputProduct))
        {
            Console.WriteLine("Product name cannot be empty.\n");
            return;
        }
        if (!productHash.ContainsKey(inputProduct))
        {
            Console.WriteLine("Enter product name properly.\n");
            return;
        }

        int orderAmount = (int)productHash[inputProduct];

        orderList.Add(new Order(id, inputName, inputProduct, orderAmount));
        orderQueue.Enqueue(new Order(id, inputName, inputProduct, orderAmount));
        orderDictionary.Add(id, new Order(id, inputName, inputProduct, orderAmount));

        Console.WriteLine("Order added Successfully.\n");
    }

    public static void ViewAllOrder()
    {
        if (orderList.Any())
        {
            Console.WriteLine("Displaying all order");
            foreach (var list in orderList)
            {
                Console.WriteLine(
                    $"Id: {list.orderId} | Name: {list.customerName} | Product Name: {list.productName} | Amount: {list.orderAmount}"
                );
            }
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    public static void ProcessNextOrder()
    {
        if (orderQueue.Count != 0)
        {
            Order dequeueOrder = orderQueue.Dequeue();
            orderInProgress.Add(outputOrder);
            Notify notification = ShowMessage;
            notification(dequeueOrder.orderId);
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    public static void SearchOrder()
    {
        if (orderList.Any())
        {
            Console.WriteLine("Enter order id to be search: ");
            if (!int.TryParse(Console.ReadLine(), out int inputId))
            {
                Console.WriteLine("Enter valid order id.\n");
                return;
            }

            if (orderDictionary.ContainsKey(inputId))
            {
                Order outputOrder = orderDictionary[inputId];
                Console.WriteLine(
                    $"Id: {outputOrder.orderId} | Name: {outputOrder.customerName} | Product Name: {outputOrder.productName} | Amount: {outputOrder.orderAmount}"
                );
            }
            else
            {
                Console.WriteLine($"{inputId} Order id not found.\n");
            }
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    public static void DisplayHighValueOrder()
    {
        if (orderList.Any())
        {
            Console.WriteLine("Displaying highest value Order (>5000): ");
            foreach (Order list in orderList)
            {
                if (list.orderAmount > 5000)
                {
                    Console.WriteLine(
                        $"Id: {list.orderId} | Name: {list.customerName} | Product Name: {list.productName} | Amount: {list.orderAmount}"
                    );
                }
            }
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    public static void DisplayInProgress()
    {
        if (orderInProgress.Any())
        {
            Console.WriteLine("Displaying in process order: ");
            foreach (Order list in orderInProgress)
            {
                Console.WriteLine(
                    $"Id: {list.orderId} | Name: {list.customerName} | Product Name: {list.productName} | Amount: {list.orderAmount}"
                );
            }
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    public static void DisplayRemaining()
    {
        if (orderQueue.Count != 0)
        {
            Console.WriteLine("Displaying Remaining Order: ");
            foreach (Order list in orderQueue)
            {
                Console.WriteLine(
                    $"Id: {list.orderId} | Name: {list.customerName} | Product Name: {list.productName} | Amount: {list.orderAmount}"
                );
            }
        }
        else
        {
            Console.WriteLine("There is no remaining Order.\n");
        }
    }

    static void ShowMessage(int id) => Console.WriteLine($"Order {id} Processed Successfully.");
}
