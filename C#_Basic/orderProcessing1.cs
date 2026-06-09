using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OrderProcess;

public delegate void Notify(int id);

public enum Status
{
    Pending,
    InProgress,
    Completed,
}

class Order
{
    public int orderID;
    public string orderName;
    public string productName;
    public int orderAmount;
    public Status status;

    public Order(int id, string name, string pname, int amount, Status status1)
    {
        orderID = id;
        orderName = name;
        productName = pname;
        orderAmount = amount;
        status = status1;
    }
}

class Program
{
    static List<Order> orderList = new List<Order>();
    static Queue<Order> orderInProgress = new Queue<Order>();
    static List<Order> orderCompleted = new List<Order>();
    static Dictionary<int, Order> orderDictionary = new Dictionary<int, Order>();
    static Queue<Order> orderQueue = new Queue<Order>();
    static Hashtable orderHashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);

    public static void Main(string[] Arg)
    {
        bool running = true;

        AddProduct();
        DisplayProduct();

        do
        {
            Console.WriteLine("\n1. Add Order");
            Console.WriteLine("2. Process next Order.");
            Console.WriteLine("3. Complete current Order.");
            Console.WriteLine("4. Search Order with Order Id.");
            Console.WriteLine("5. Display All Order.");
            Console.WriteLine("6. Display Pending Order.");
            Console.WriteLine("7. Display in-Progress Order.");
            Console.WriteLine("8. Display Completed order.");
            Console.WriteLine("9. Display High value order (>5000).");
            Console.WriteLine("10. Display order summary.");
            Console.WriteLine("11. Exit.\n");

            Console.WriteLine("Select your Choice: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddOrder();
                    break;
                case "2":
                    ProcessNextOrder();
                    break;
                case "3":
                    CompleteCurrentOrder();
                    break;
                case "4":
                    SearchWithOrderId();
                    break;
                case "5":
                    DisplayAllOrder();
                    break;
                case "6":
                    DisplayPendingOrder();
                    break;
                case "7":
                    DisplayInProgress();
                    break;
                case "8":
                    DisplayCompletedOrder();
                    break;
                case "9":
                    DisplayHighValue();
                    break;
                case "10":
                    DisplayOrderSummary();
                    break;
                case "11":
                    running = false;
                    Console.WriteLine("Exiting...\n");
                    break;
                default:
                    Console.WriteLine("Please enter valid choice.\n");
                    break;
            }
        } while (running);
    }

    static void AddProduct()
    {
        orderHashtable.Add("Laptop", 65000);
        orderHashtable.Add("Mobile", 45000);
        orderHashtable.Add("Monitor", 10000);
        orderHashtable.Add("Mouse", 499);
        orderHashtable.Add("KeyBoard", 999);
        orderHashtable.Add("CPU", 75000);
    }

    static void DisplayProduct()
    {
        Console.WriteLine("Product and their Prices: ");

        // Console.WriteLine(" Product Name : Price");

        foreach (DictionaryEntry entry in orderHashtable)
        {
            Console.WriteLine($" {entry.Key} : {entry.Value}");
        }
    }

    static void AddOrder()
    {
        Console.WriteLine("Enter Order Id: ");
        if (!int.TryParse(Console.ReadLine(), out int inputId) || inputId < 0)
        {
            Console.WriteLine("Please enter valid order id.\n");
            return;
        }
        if (orderList.Any(e => e.orderID == inputId))
        {
            Console.WriteLine("Order id already exist.\n");
            return;
        }

        Console.WriteLine("Enter customer Name: ");
        string inputName = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputName))
        {
            Console.WriteLine("Enter valid name.\n");
            return;
        }

        Console.WriteLine("Enter product Name: ");
        string inputProductName = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputProductName))
        {
            Console.WriteLine("Enter valid name.\n");
            return;
        }
        if (!orderHashtable.ContainsKey(inputProductName))
        {
            Console.WriteLine("Product is not in List or Please enter name properly.\n");
            return;
        }

        int price = (int)orderHashtable[inputProductName]!;

        Order order = new Order(inputId, inputName, inputProductName, price, Status.Pending);

        orderList.Add(order);
        orderQueue.Enqueue(order);
        orderDictionary.Add(inputId, order);
    }

    static void ProcessNextOrder()
    {
        if (orderQueue.Any())
        {
            var orderDequeue = orderQueue.Dequeue();
            orderDequeue.status = Status.InProgress;
            orderInProgress.Enqueue(orderDequeue);
            Notify notification = ShowMessageProcess;
            notification(orderDequeue.orderID);
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    static void CompleteCurrentOrder()
    {
        if (orderInProgress.Any())
        {
            var orderDequeue = orderInProgress.Dequeue();
            orderDequeue.status = Status.Completed;
            orderCompleted.Add(orderDequeue);
            Notify notification = ShowMessageCompleted;
            notification(orderDequeue.orderID);
        }
        else
        {
            Console.WriteLine("No in Progress order.\n");
        }
    }

    static void SearchWithOrderId()
    {
        if (orderList.Any())
        {
            Console.WriteLine("Enter Order Id to be Search: ");
            if (!int.TryParse(Console.ReadLine(), out int inputId) || inputId < 0)
            {
                Console.WriteLine("Please enter valid order id.\n");
                return;
            }

            Console.WriteLine($"Displaying Order {inputId} details: ");
            if (orderDictionary.ContainsKey(inputId))
            {
                ShowData(orderDictionary[inputId]);
            }
            else
            {
                Console.WriteLine("No Order found.\n");
            }
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    static void DisplayAllOrder()
    {
        if (orderList.Any())
        {
            Console.WriteLine("Displaying All Order: ");
            foreach (var list in orderList)
            {
                ShowData(list);
            }
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    static void DisplayPendingOrder()
    {
        if (orderQueue.Count() > 0)
        {
            Console.WriteLine("Displaying Pending Order: ");
            foreach (var list in orderQueue)
            {
                ShowData(list);
            }
        }
        else
        {
            Console.WriteLine("No Pending Order.\n");
        }
    }

    static void DisplayInProgress()
    {
        if (orderInProgress.Count() > 0)
        {
            Console.WriteLine("Displaying In Progress Order: ");
            foreach (var list in orderInProgress)
            {
                ShowData(list);
            }
        }
        else
        {
            Console.WriteLine("Order not yet in Progress.\n");
        }
    }

    static void DisplayCompletedOrder()
    {
        if (orderCompleted.Any())
        {
            Console.WriteLine("Displaying completed Order: ");
            foreach (var list in orderCompleted)
            {
                ShowData(list);
            }
        }
        else
        {
            Console.WriteLine("No completed Order yet.\n");
        }
    }

    static void DisplayHighValue()
    {
        if (orderList.Any())
        {
            Console.WriteLine("Displaying High Order value (>5000): ");
            foreach (var list in orderList)
            {
                if (list.orderAmount > 5000)
                {
                    ShowData(list);
                }
            }
        }
        else
        {
            Console.WriteLine("No order yet.\n");
        }
    }

    static void DisplayOrderSummary()
    {
        Console.WriteLine($"Total Orders: {orderList.Count}\n");
        Console.WriteLine($"Pending Orders    : {orderQueue.Count}");
        Console.WriteLine($"InProgress Orders : {orderInProgress.Count}");
        Console.WriteLine($"Completed Orders  : {orderCompleted.Count}");
        int revenue = orderList.Sum(x => x.orderAmount);
        Console.WriteLine($"Total Revenue: {revenue}");
    }

    static void ShowMessageProcess(int id) =>
        Console.WriteLine($"Order {id} Processed Successfully.\n");

    static void ShowMessageCompleted(int id) =>
        Console.WriteLine($"Order {id} Completed Successfully.\n");

    static void ShowData(Order list)
    {
        Console.WriteLine(
            $"Id: {list.orderID} | Customer Name: {list.orderName} | Product Name: {list.productName} | Amount: {list.orderAmount} | Status: {list.status}\n"
        );
    }
}
