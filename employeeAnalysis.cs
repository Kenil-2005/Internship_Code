using System;
using System.Collections.Generic;

namespace EmployeeManagementSystem
{
    // 1. Enum for Employee Type
    public enum EmployeeType
    {
        Permanent,
        Contract,
    }

    // 2. Custom Exception
    public class InvalidSalaryException : Exception
    {
        public InvalidSalaryException(string message)
            : base(message) { }
    }

    // 3. Interface for Report Generation
    public interface IReportable
    {
        string GenerateReport();
    }

    // 4. Base Class
    public abstract class Employee : IReportable
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public EmployeeType Type { get; set; }

        public Employee(int id, string name, EmployeeType type)
        {
            EmployeeId = id;
            EmployeeName = name;
            Type = type;
        }

        public abstract decimal CalculateSalary();

        public virtual string GenerateReport()
        {
            return $"ID: {EmployeeId} | Name: {EmployeeName} | Type: {Type} | Salary: ${CalculateSalary():F2}";
        }
    }

    // 5. Derived Classes
    public class PermanentEmployee : Employee
    {
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }

        public PermanentEmployee(int id, string name, decimal basicSalary, decimal bonus)
            : base(id, name, EmployeeType.Permanent)
        {
            if (basicSalary < 0 || bonus < 0)
                throw new InvalidSalaryException("Salary and Bonus cannot be negative.");

            BasicSalary = basicSalary;
            Bonus = bonus;
        }

        public override decimal CalculateSalary()
        {
            return SalaryCalculator.CalculatePermanentSalary(BasicSalary, Bonus);
        }
    }

    public class ContractEmployee : Employee
    {
        public decimal HourlyRate { get; set; }
        public decimal HoursWorked { get; set; }

        public ContractEmployee(int id, string name, decimal hourlyRate, decimal hoursWorked)
            : base(id, name, EmployeeType.Contract)
        {
            if (hourlyRate < 0 || hoursWorked < 0)
                throw new InvalidSalaryException(
                    "Hourly Rate and Hours Worked cannot be negative."
                );

            HourlyRate = hourlyRate;
            this.HoursWorked = hoursWorked;
        }

        public override decimal CalculateSalary()
        {
            return SalaryCalculator.CalculateContractSalary(HourlyRate, HoursWorked);
        }
    }

    // 6. Static Class
    public static class SalaryCalculator
    {
        public static decimal CalculatePermanentSalary(decimal basic, decimal bonus)
        {
            return basic + bonus;
        }

        public static decimal CalculateContractSalary(decimal rate, decimal hours)
        {
            return rate * hours;
        }
    }

    // 7. Main Program & In-Memory Collection
    class Program
    {
        static List<Employee> employees = new List<Employee>();

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=====================================");
                Console.WriteLine("    EMPLOYEE MANAGEMENT SYSTEM       ");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Display Employees");
                Console.WriteLine("3. Calculate Salary (Total Payroll)");
                Console.WriteLine("4. Generate Report");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice (1-5): ");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;
                    case "2":
                        DisplayEmployees();
                        break;
                    case "3":
                        CalculateTotalSalary();
                        break;
                    case "4":
                        GenerateReports();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Exiting application...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddEmployee()
        {
            try
            {
                Console.Write("Enter Employee ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                    throw new FormatException("Invalid ID format.");

                Console.Write("Enter Employee Name: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty.");

                Console.Write("Enter Type (0 for Permanent, 1 for Contract): ");
                if (
                    !int.TryParse(Console.ReadLine(), out int typeChoice)
                    || (typeChoice != 0 && typeChoice != 1)
                )
                    throw new FormatException("Invalid type choice.");

                EmployeeType empType = (EmployeeType)typeChoice;

                if (empType == EmployeeType.Permanent)
                {
                    Console.Write("Enter Basic Salary: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal basic))
                        throw new FormatException("Invalid salary format.");
                    Console.Write("Enter Bonus: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal bonus))
                        throw new FormatException("Invalid bonus format.");

                    employees.Add(new PermanentEmployee(id, name, basic, bonus));
                }
                else
                {
                    Console.Write("Enter Hourly Rate: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal rate))
                        throw new FormatException("Invalid rate format.");
                    Console.Write("Enter Hours Worked: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal hours))
                        throw new FormatException("Invalid hours format.");

                    employees.Add(new ContractEmployee(id, name, rate, hours));
                }

                Console.WriteLine("Employee added successfully! Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} Press any key to try again...");
            }
            Console.ReadKey();
        }

        static void DisplayEmployees()
        {
            Console.Clear();
            Console.WriteLine("--- Employee List ---");
            if (employees.Count == 0)
                Console.WriteLine("No employees found.");
            foreach (var emp in employees)
            {
                Console.WriteLine(
                    $"ID: {emp.EmployeeId} | Name: {emp.EmployeeName} | Type: {emp.Type}"
                );
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void CalculateTotalSalary()
        {
            Console.Clear();
            Console.WriteLine("--- Calculate Salaries ---");
            decimal totalPayroll = 0;
            foreach (var emp in employees)
            {
                decimal sal = emp.CalculateSalary();
                Console.WriteLine($"{emp.EmployeeName}'s Salary: ${sal:F2}");
                totalPayroll += sal;
            }
            Console.WriteLine($"\nTotal Monthly Payroll: ${totalPayroll:F2}");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static void GenerateReports()
        {
            Console.Clear();
            Console.WriteLine("--- Employee Reports ---");
            foreach (var emp in employees)
            {
                Console.WriteLine(emp.GenerateReport());
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
