using System;
using System.Linq;

namespace markAnalyzer
{
    class Program
    {
        static void calculate(int a, int b, int c)
        {
            int totalMarks = a + b + c;
            int averageMarks = totalMarks / 3;
            int minMarks = Math.Min(a, Math.Min(b, c));
            int maxMarks = Math.Max(a, Math.Max(b, c));
            string result = totalMarks >= 112 ? "Pass" : "Fail";

            Console.WriteLine("Total of Student " + totalMarks + "\n");
            Console.WriteLine("Average of Student " + averageMarks + "\n");
            Console.WriteLine("Highest of Student " + maxMarks + "\n");
            Console.WriteLine("Lowest of Student " + minMarks + "\n");
            Console.WriteLine("Result of Student " + result + "\n");
        }

        static void Main(string[] args)
        {
            string[] name = new string[100];
            int[] mathsMarks = new int[100];
            int[] scienceMarks = new int[100];
            int[] englishMarks = new int[100];
            int count = 0;
            bool runnnig = true;

            do
            {
                Console.WriteLine("\n1. Add Student");
                Console.WriteLine("2. Display Marks");
                Console.WriteLine("3. Calculate Result");
                Console.WriteLine("4. Search Student Name");
                Console.WriteLine("5. Exit");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        if (count != 100)
                        {
                            Console.WriteLine("\nEnter Student Name: ");
                            string newName = Console.ReadLine() ?? "";
                            name[count] = newName;

                            Console.WriteLine("Enter Student Maths Marks: ");
                            string newMathsMarks = Console.ReadLine() ?? "";
                            mathsMarks[count] = int.Parse(newMathsMarks);

                            Console.WriteLine("Enter Student Science Marks: ");
                            string newScienceMarks = Console.ReadLine() ?? "";
                            scienceMarks[count] = int.Parse(newScienceMarks);

                            Console.WriteLine("Enter Student English Marks: ");
                            string newEnglishMarks = Console.ReadLine() ?? "";
                            englishMarks[count] = int.Parse(newEnglishMarks);

                            count = count + 1;
                        }
                        else
                        {
                            Console.WriteLine("Size is full...");
                        }

                        break;
                    case "2":
                        if (count != 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                Console.WriteLine("\n" + name[i] + "'s " + "Marks");
                                Console.WriteLine("Maths Marks" + " " + mathsMarks[i]);
                                Console.WriteLine("Science Marks" + " " + scienceMarks[i]);
                                Console.WriteLine("Maths Marks" + " " + englishMarks[i] + "\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Student Data Found.\n");
                        }
                        break;

                    case "3":
                        if (count != 0)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                Console.WriteLine("\n" + name[i] + "'s " + "Results");
                                calculate(mathsMarks[i], scienceMarks[i], englishMarks[i]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Student Data Found.\n");
                        }

                        break;

                    case "4":
                        if (count != 0)
                        {
                            Console.WriteLine("\nEnter student name to find");
                            string findName = Console.ReadLine() ?? "";
                            for (int i = 0; i < count; i++)
                            {
                                if (
                                    name[i].IndexOf(findName, StringComparison.OrdinalIgnoreCase)
                                    >= 0
                                )
                                {
                                    Console.WriteLine("\n" + name[i] + "'s " + "Marks");
                                    calculate(mathsMarks[i], scienceMarks[i], englishMarks[i]);
                                }
                            }
                            // int index = Array.IndexOf(name, findName);
                            // if (index != -1)
                            // {
                            //     Console.WriteLine("\n" + name[index] + "'s " + "Marks");
                            //     Console.WriteLine("Maths Marks" + " " + mathsMarks[index]);
                            //     Console.WriteLine("Science Marks" + " " + scienceMarks[index]);
                            //     Console.WriteLine("Maths Marks" + " " + englishMarks[index] + "\n");

                            //     calculate(
                            //         mathsMarks[index],
                            //         scienceMarks[index],
                            //         englishMarks[index]
                            //     );

                            //     // int totalMarksCase4 =
                            //     //     mathsMarks[index] + scienceMarks[index] + englishMarks[index];
                            //     // int averageMarksCase4 = totalMarksCase4 / 3;
                            //     // int minMarksCase4 = Math.Min(
                            //     //     mathsMarks[index],
                            //     //     Math.Min(scienceMarks[index], englishMarks[index])
                            //     // );
                            //     // int maxMarksCase4 = Math.Max(
                            //     //     mathsMarks[index],
                            //     //     Math.Max(scienceMarks[index], englishMarks[index])
                            //     // );
                            //     // string resultCase4 = totalMarksCase4 >= 112 ? "Pass" : "Fail";

                            //     // Console.WriteLine("Total of Student " + totalMarksCase4 + "\n");
                            //     // Console.WriteLine("Average of Student " + averageMarksCase4 + "\n");
                            //     // Console.WriteLine("Highest of Student " + maxMarksCase4 + "\n");
                            //     // Console.WriteLine("Lowest of Student " + minMarksCase4 + "\n");
                            //     // Console.WriteLine("Result of Student " + resultCase4 + "\n");
                            // }
                            // else
                            // {
                            //     Console.WriteLine("No Student Data Found.\n");
                            // }
                        }
                        else
                        {
                            Console.WriteLine("No Student Data Found.\n");
                        }
                        break;

                    case "5":
                        Console.WriteLine("Exiting...");
                        runnnig = false;
                        break;

                    default:
                        Console.WriteLine("Please enter valid choice");
                        break;
                }
            } while (runnnig);
        }
    }
}
