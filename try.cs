using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization; // Required for Source Generation

namespace StudentRecord;

// 1. Tell the Source Generator to support your class and list type
[JsonSerializable(typeof(Student))]
[JsonSerializable(typeof(List<Student>))]
public partial class AppJsonContext : JsonSerializerContext { }

// FIX: Added 'public' so the Source Generator can access it without throwing an error
public class Student
{
    // Fix: Added { get; set; } to make these properties so they serialize easily
    public int studentID { get; set; }
    public string studentName { get; set; } = "";
    public string studentCourse { get; set; } = "";
    public int studentMarks { get; set; }

    public Student() { }

    public Student(int id, string name, string cName, int marks)
    {
        studentID = id;
        studentName = name;
        studentCourse = cName;
        studentMarks = marks;
    }
}

public class Program
{
    static List<Student> studentList = new List<Student>();

    // File path where students will be stored
    static string filePath = "students.json";

    public static void Main(string[] Arg)
    {
        bool running = true;
        do
        {
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Save Students to File");
            Console.WriteLine("4. Load Students from File");
            Console.WriteLine("5. Generate Report");
            Console.WriteLine("6. Exit\n");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;
                case "2":
                    ViewStudents();
                    break;
                case "3":
                    SaveStudentToFile();
                    break;
                case "4":
                    LoadStudentFromFile();
                    break;
                case "5":
                    GenrateReport();
                    break;
                case "6":
                    running = false;
                    Console.WriteLine("Exiting...\n");
                    break;
                default:
                    Console.WriteLine("Invalid Choice, Choice between 1 to 6.\n");
                    break;
            }
        } while (running);
    }

    public static void AddStudent()
    {
        Console.Write("Enter student id: ");
        if (!int.TryParse(Console.ReadLine(), out int inputId))
        {
            Console.WriteLine("Please enter valid id.\n");
            return;
        }
        if (studentList.Any(e => e.studentID == inputId))
        {
            Console.WriteLine("Record already found.\n");
            return;
        }

        Console.Write("Enter student Name: ");
        string inputName = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputName))
        {
            Console.WriteLine("Please enter valid name.\n");
            return;
        }

        Console.Write("Enter course Name: ");
        string inputCourseName = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputCourseName))
        {
            Console.WriteLine("Please enter valid course name.\n");
            return;
        }

        Console.Write("Enter student marks: ");
        if (!int.TryParse(Console.ReadLine(), out int inputMarks))
        {
            Console.WriteLine("Please enter valid Marks\n");
            return;
        }

        studentList.Add(new Student(inputId, inputName, inputCourseName, inputMarks));
        Console.WriteLine("Student added successfully!\n");
    }

    public static void ViewStudents()
    {
        if (!studentList.Any())
        {
            Console.WriteLine("No student records found.\n");
            return;
        }

        Console.WriteLine("\nStudent Details: ");
        foreach (var list in studentList)
        {
            Console.WriteLine(
                $"Student ID: {list.studentID} | Student Name: {list.studentName} | Course Name: {list.studentCourse} | Marks: {list.studentMarks}"
            );
        }
        Console.WriteLine();
    }

    public static void SaveStudentToFile()
    {
        try
        {
            // Fix: Pass 'AppJsonContext.Default.ListStudent' context to avoid reflection error
            string jsonList = JsonSerializer.Serialize(
                studentList,
                AppJsonContext.Default.ListStudent
            );

            // Actually saving it to a physical file as your choice description implies
            File.WriteAllText(filePath, jsonList);

            Console.WriteLine($"Data successfully saved to {filePath}!\n");
            Console.WriteLine("Serialized JSON Preview:");
            Console.WriteLine(jsonList + "\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}\n");
        }
    }

    public static void LoadStudentFromFile()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No saved data file found.\n");
                return;
            }

            string jsonList = File.ReadAllText(filePath);

            // Fix: Deserialize using the generated context safe for Native AOT
            var deserializedList = JsonSerializer.Deserialize(
                jsonList,
                AppJsonContext.Default.ListStudent
            );

            if (deserializedList != null)
            {
                foreach (var list in deserializedList)
                {
                    Console.WriteLine(
                        $"Student ID: {list.studentID} | Student Name: {list.studentName} | Course Name: {list.studentCourse} | Marks: {list.studentMarks}"
                    );
                }
                Console.WriteLine("Data successfully loaded from file!\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}\n");
        }
    }

    public static void GenrateReport()
    {
        if (!studentList.Any())
        {
            Console.WriteLine("No students to generate a report for.\n");
            return;
        }

        Console.WriteLine("\n--- Performance Report ---");
        Console.WriteLine($"Total Students: {TotalStudent()}");
        Console.WriteLine($"Average Marks: {AvrageStudentMarks()}");
        Console.WriteLine($"Highest Mark: {HighestMarks()}\n");
    }

    public static int TotalStudent()
    {
        return studentList.Count;
    }

    public static int AvrageStudentMarks()
    {
        return (int)studentList.Average(s => s.studentMarks);
    }

    public static int HighestMarks()
    {
        return studentList.Max(s => s.studentMarks);
    }
}
