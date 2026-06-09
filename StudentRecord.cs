using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudentRecord;

[JsonSerializable(typeof(Student))]
[JsonSerializable(typeof(List<Student>))]
public partial class AppJsonContext : JsonSerializerContext { }

public class Student
{
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
    static string filePath = "studentsList.json";

    public static async Task Main(string[] Arg)
    {
        bool running = true;
        do
        {
            Console.WriteLine("\n1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Save Students to File");
            Console.WriteLine("4. Load Students from File");
            Console.WriteLine("5. Generate Report");
            Console.WriteLine("6. Exit\n");

            Console.WriteLine("Enter your choice: ");
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
                    await LoadStudentFromFile();
                    break;
                case "5":
                    await GenrateReport();
                    break;
                case "6":
                    running = false;
                    Console.WriteLine("Exiting...\n");
                    break;
                default:
                    Console.WriteLine("Invalid Choice,Choice between 1 to 6.\n");
                    break;
            }
        } while (running);
    }

    public static void AddStudent()
    {
        Console.WriteLine("Enter student id: ");
        if (!int.TryParse(Console.ReadLine(), out int inputId))
        {
            Console.WriteLine("Please enter valide id.\n");
            return;
        }
        if (studentList.Any(e => e.studentID == inputId))
        {
            Console.WriteLine("Record already found.\n");
            return;
        }

        Console.WriteLine("Enter student Name: ");
        string inputName = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputName))
        {
            Console.WriteLine("Please enter valid name.\n");
            return;
        }

        Console.WriteLine("Enter course Name: ");
        string inputCourseName = Console.ReadLine() ?? "";
        if (String.IsNullOrWhiteSpace(inputCourseName))
        {
            Console.WriteLine("Please enter valid course name.\n");
            return;
        }

        Console.WriteLine("Enter student marks: ");
        if (!int.TryParse(Console.ReadLine(), out int inputMarks))
        {
            Console.WriteLine("Please enter valid Marks");
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

        Console.WriteLine("Student Details: \n");
        foreach (var list in studentList)
        {
            Console.WriteLine(
                $"Student ID: {list.studentID} | Student Name: {list.studentName} | Course Name: {list.studentCourse} | Marks: {list.studentMarks} "
            );
        }
    }

    public static void SaveStudentToFile()
    {
        try
        {
            string jsonList = JsonSerializer.Serialize(
                studentList,
                AppJsonContext.Default.ListStudent
            );
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

    public static async Task LoadStudentFromFile()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No saved data file found.\n");
                return;
            }

            await using FileStream openStream = File.OpenRead(filePath);

            var deserializedList = await JsonSerializer.DeserializeAsync(
                openStream,
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

    public static async Task GenrateReport()
    {
        if (!studentList.Any())
        {
            Console.WriteLine("No students to generate a report for.\n");
            return;
        }

        Console.WriteLine("Processing report in the background...");

        int total = 0;
        int average = 0;
        int highest = 0;

        await Task.Run(() =>
        {
            Parallel.Invoke(
                () => total = TotalStudent(),
                () => average = AvrageStudentMarks(),
                () => highest = HighestMarks()
            );
        });

        Console.WriteLine("\n--- Performance Report ---");
        Console.WriteLine($"Total Students: {total}");
        Console.WriteLine($"Average Marks: {average}");
        Console.WriteLine($"Highest Mark: {highest}\n");
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
