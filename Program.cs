using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Network_Simulation
{
    struct Components
    {
        public double Connection12;
        public double Connection15;
        public double Connection23;
        public double Connection24;
        public double Connection34;
        public double Connection47;
        public double Connection53;
        public double Connection54;
        public double Connection56;
        public double Connection67;
    }

    struct UniquePaths
    {
        public double Connection1247;
        public double Connection12347;
        public double Connection15347;
        public double Connection1547;
        public double Connection1567;
    }

    struct SimulationRecord
    {
        public int              RunIndex;
        public Components       ComponentWeights;
        public UniquePaths      PathWeights;
        public double           ShortestPath;
        public double           AverageShortestPath;
    };

    class Program
    {
        public static int NumberOfRuns = 100000;

        static void Main(string[] args)
        {
            //Variables
            bool exit_program = false;

            //Program Control Flow 
            while (!exit_program)
            {
                switch (DisplayMainMenu())
                {
                    case 1: //Display Prompt
                        DisplayPrompt();
                        break;
                    case 2: //Run Simulation
                        RunSimulation();
                        break;
                    case 3: //Exit Simulation
                        exit_program = true;
                        break;
                    default:
                        PressEnterToReturn("ERROR: Please Provide a Valid Numerical Input and Try Again!");
                        break;
                }

                Console.Clear();


            }
        }

        public static int DisplayMainMenu()
        {
            int selection_code = 0;
            Console.WriteLine("|=====================================================|");
            Console.WriteLine("|       Network Process Probability Simulation!       |");
            Console.WriteLine("|=====================================================|\n\n");
            Console.WriteLine("Please Select an Action to Perform:");
            Console.WriteLine("\n\t1.] Display Prompt");
            Console.WriteLine("\t2.] Run Simulation");
            Console.WriteLine("\t3.] Exit Simulation");

            Console.Write("\nInput: ");
            string Input = Console.ReadLine();

            try
            {
                selection_code = Int32.Parse(Input);
            }
            catch (FormatException)
            {
                //Console.WriteLine("Please Provide a Valid Numerical Input and Try Again!\n");
                selection_code = -1;
            }


            return selection_code;
        }

        public static void DisplayPrompt()
        {
            Console.WriteLine("\n\nA software process is represented as a network with some process time between nodes having Uniform distribution (U) and others with Deterministic values: ");
            Console.WriteLine("\n\tA.] Analyze the performance of the system,");
            Console.WriteLine("\tB.] After adequate samples, show how you quantify the criticality of each path.");
            Console.WriteLine("\tC.] Briefly explain your redesign perspective of such a system.");
            Console.WriteLine("\n\n Here are the directed connections for each node:");
            Console.WriteLine("\tNode_1 -> Node_2, Node_5");
            Console.WriteLine("\tNode_2 -> Node_3, Node_4");
            Console.WriteLine("\tNode_3 -> Node_4");
            Console.WriteLine("\tNode_4 -> Node_7");
            Console.WriteLine("\tNode_5 -> Node_3, Node_4, Node_6");
            Console.WriteLine("\tNode_6 -> Node_7");
            Console.WriteLine("\n\n Here are the weights for each connection:");
            Console.WriteLine("\t(Node_1, Node_2): U(4, 6)");
            Console.WriteLine("\t(Node_1, Node_5): 6");
            Console.WriteLine("\t(Node_2, Node_3): 6");
            Console.WriteLine("\t(Node_2, Node_4): U(6, 8)");
            Console.WriteLine("\t(Node_3, Node_4): Triangle(4, 8, 10)");
            Console.WriteLine("\t(Node_4, Node_7): 4");
            Console.WriteLine("\t(Node_5, Node_3): 8");
            Console.WriteLine("\t(Node_5, Node_4): 11");
            Console.WriteLine("\t(Node_5, Node_6): U(8,10)");
            Console.WriteLine("\t(Node_6, Node_7): U(9,10)");
            PressEnterToReturn("Please press ENTER to return to the main menu...");
        }


        public static void ClearRangeOfConsoleLines(int top, int bottom)
        {
            for (int i = bottom; i > top; i--)
            {
                //Set The Cursor Position for the Interval
                Console.SetCursorPosition(0, Console.CursorTop - 1);

                //Clear Current Console Line
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }

            if (top == bottom)
            {
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }
        }

        public static void RunSimulation()
        {
            //Generate Record Array
            SimulationRecord[] Results = new SimulationRecord[NumberOfRuns];

            //Create a random number generator.
            Random NumberGenerator = new Random();

            //Variable Declaration
            double CumulativeShortestPath = 0;
            int progress = 0;

            for(int i = 0; i < NumberOfRuns; i++)
            {
                Results[i].RunIndex = i;

                //Calculate The Component Weights
                Results[i].ComponentWeights.Connection12 = UniformDistribution(4, 6, NumberGenerator);
                Results[i].ComponentWeights.Connection15 = 6;
                Results[i].ComponentWeights.Connection23 = 6;
                Results[i].ComponentWeights.Connection24 = UniformDistribution(6, 8, NumberGenerator);
                Results[i].ComponentWeights.Connection34 = Triangle(4.0, 10.0, 8.0, NumberGenerator);
                Results[i].ComponentWeights.Connection47 = 4;
                Results[i].ComponentWeights.Connection53 = 8;
                Results[i].ComponentWeights.Connection54 = 11;
                Results[i].ComponentWeights.Connection56 = UniformDistribution(8, 10, NumberGenerator);
                Results[i].ComponentWeights.Connection67 = UniformDistribution(9, 10, NumberGenerator);

                //Calculate the Path Weights
                Results[i].PathWeights.Connection12347 = Results[i].ComponentWeights.Connection12 + Results[i].ComponentWeights.Connection23 + Results[i].ComponentWeights.Connection34 + Results[i].ComponentWeights.Connection47;
                Results[i].PathWeights.Connection15347 = Results[i].ComponentWeights.Connection15 + Results[i].ComponentWeights.Connection53 + Results[i].ComponentWeights.Connection34 + Results[i].ComponentWeights.Connection47;
                Results[i].PathWeights.Connection1247  = Results[i].ComponentWeights.Connection12 + Results[i].ComponentWeights.Connection24 + Results[i].ComponentWeights.Connection47;
                Results[i].PathWeights.Connection1547  = Results[i].ComponentWeights.Connection15 + Results[i].ComponentWeights.Connection54 + Results[i].ComponentWeights.Connection47;
                Results[i].PathWeights.Connection1567  = Results[i].ComponentWeights.Connection15 + Results[i].ComponentWeights.Connection56 + Results[i].ComponentWeights.Connection67;

                //Calculate the Shortest Path
                if(Results[i].PathWeights.Connection12347 <= Results[i].PathWeights.Connection15347 &&
                   Results[i].PathWeights.Connection12347 <= Results[i].PathWeights.Connection1247  &&
                   Results[i].PathWeights.Connection12347 <= Results[i].PathWeights.Connection1547  &&
                   Results[i].PathWeights.Connection12347 <= Results[i].PathWeights.Connection1567 )
                {
                    //Set Connection12347 as the Shortest Path
                    Results[i].ShortestPath = Results[i].PathWeights.Connection12347;
                }
                else if( Results[i].PathWeights.Connection15347 <= Results[i].PathWeights.Connection12347 &&
                         Results[i].PathWeights.Connection15347 <= Results[i].PathWeights.Connection1247 &&
                         Results[i].PathWeights.Connection15347 <= Results[i].PathWeights.Connection1547 &&
                         Results[i].PathWeights.Connection15347 <= Results[i].PathWeights.Connection1567)
                {
                    //Set Connection15347 as the Shortest Path
                    Results[i].ShortestPath = Results[i].PathWeights.Connection15347;
                }
                else if (Results[i].PathWeights.Connection1247 <= Results[i].PathWeights.Connection15347 &&
                         Results[i].PathWeights.Connection1247 <= Results[i].PathWeights.Connection12347 &&
                         Results[i].PathWeights.Connection1247 <= Results[i].PathWeights.Connection1547  &&
                         Results[i].PathWeights.Connection1247 <= Results[i].PathWeights.Connection1567)
                {
                    //Set Connection1247 as the Shortest Path
                    Results[i].ShortestPath = Results[i].PathWeights.Connection1247;
                }
                else if (Results[i].PathWeights.Connection1547 <= Results[i].PathWeights.Connection15347 &&
                         Results[i].PathWeights.Connection1547 <= Results[i].PathWeights.Connection1247  &&
                         Results[i].PathWeights.Connection1547 <= Results[i].PathWeights.Connection12347 &&
                         Results[i].PathWeights.Connection1547 <= Results[i].PathWeights.Connection1567)
                {
                    //Set Connection1547 as the Shortest Path
                    Results[i].ShortestPath = Results[i].PathWeights.Connection1547;
                }
                else
                {
                    //Set Connection1567 as the Shortest Path
                    Results[i].ShortestPath = Results[i].PathWeights.Connection1567;
                }

                //Recalculate the Average
                CumulativeShortestPath += Results[i].ShortestPath;
                Results[i].AverageShortestPath = CumulativeShortestPath / (i + 1);

                //Console.WriteLine(Results[i].AverageShortestPath);
                progress = 100 - (((NumberOfRuns - i) * 100) / NumberOfRuns);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("The Simulation has Started. Current Progress: %" + progress);
            }

            if (false)
            {
                double Interval5 = 0;
                double Interval6 = 0;
                double Interval7 = 0;
                double Interval8 = 0;
                double Interval9 = 0;
                double Interval4 = 0;
                for (int x = 0; x < Results.Length; x++)
                {
                    if (Results[x].ComponentWeights.Connection34 < 5)
                    {
                        Interval4++;
                    }
                    else if (Results[x].ComponentWeights.Connection34 >= 5 && Results[x].ComponentWeights.Connection34 < 6)
                    {
                        Interval5++;
                    }
                    else if (Results[x].ComponentWeights.Connection34 >= 6 && Results[x].ComponentWeights.Connection34 < 7)
                    {
                        Interval6++;
                    }
                    else if (Results[x].ComponentWeights.Connection34 >= 7 && Results[x].ComponentWeights.Connection34 < 8)
                    {
                        Interval7++;
                    }
                    else if (Results[x].ComponentWeights.Connection34 >= 8 && Results[x].ComponentWeights.Connection34 < 9)
                    {
                        Interval8++;
                    }
                    else
                    {
                        Interval9++;
                    }
                }
                Console.WriteLine(Interval4.ToString() + ","
                                + Interval5.ToString() + ","
                                + Interval6.ToString() + ","
                                + Interval7.ToString() + ","
                                + Interval8.ToString() + ","
                                + Interval9.ToString());
            }

            GenerateFile(Results);

            PressEnterToReturn("Please press ENTER to return to the main menu...");


        }

        //Equation Provided By: https://en.wikipedia.org/wiki/Triangular_distribution
        public static double Triangle(double MinimumValue, double MaximumValue, double Mode, Random NumberGenerator){

            Double F = (Mode - MinimumValue) / (MaximumValue - MinimumValue);
            Double U = NumberGenerator.NextDouble();
            if (U > 0 && U < F)
            {
                return MinimumValue + Math.Sqrt(U * (MaximumValue - MinimumValue) * (Mode - MinimumValue));
            }
            else
            {
                return MaximumValue + Math.Sqrt( ( 1 - U ) * (MaximumValue - MinimumValue) * (MaximumValue - Mode));
            }
            
        }

        public static double UniformDistribution(double MinimumValue, double MaximumValue, Random NumberGenerator)
        {
            double RandomNumber = NumberGenerator.NextDouble();
            return MinimumValue + (RandomNumber * (MaximumValue - MinimumValue));
        }

        public static void DisplayDistributions(int[] Entries, string Title, bool PauseAfterDisplay)
        {
            int[] EntryCount = new int[Entries.Length];

            for (int i = 0; i < EntryCount.Length; i++)
            {
                EntryCount[i] = 0;
            }

            for (int i = 0; i < Entries.Length; i++)
            {
                EntryCount[Entries[i]] += 1;
            }

            Console.WriteLine("\n\nDisplaying Distributions for " + Title + " :");

            for (int i = 0; i < EntryCount.Length; i++)
            {
                Console.WriteLine("\tPercent Distribution for value " + (i.ToString()) + " : %" + (float)(((float)EntryCount[i] / (float)Entries.Length) * 100));

            }

            if (PauseAfterDisplay)
            {
                PressEnterToReturn("Please press ENTER to resume the simulation...");
            }

        }

        public static void PressEnterToReturn(string DisplayText)
        {
            Console.WriteLine("\n" + DisplayText + "\n");
            string DummyInput = Console.ReadLine();
        }

        public static void GenerateFile(SimulationRecord[] Record)
        {
            int progress = 0;
            string path = @"C:\Users\Michael\Documents\Network Simulation for " + DateTime.Now.ToString("yyyymmddhhmmss") + @".txt";
            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] infoh = new UTF8Encoding(true).GetBytes("Run Index"                + "," +
                                                                   "Weight for 1->2->4->7"    + "," +
                                                                   "Weight for 1->2->3->4->7" + "," +
                                                                   "Weight for 1->5->3->4->7" + "," +
                                                                   "Weight for 1->5->4->7"    + "," +
                                                                   "Weight for 1->5->6->7"    + "," +
                                                                   "Average Shortest Path"    + "\n");

                    // Add some information to the file.
                    fs.Write(infoh, 0, infoh.Length);

                    for (int i = 0; i < Record.Length; i++)
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(Record[i].RunIndex.ToString() + "," +
                                                                      Record[i].PathWeights.Connection1247.ToString() + "," +
                                                                      Record[i].PathWeights.Connection12347.ToString() + "," +
                                                                      Record[i].PathWeights.Connection15347.ToString() + "," +
                                                                      Record[i].PathWeights.Connection1547.ToString() + "," +
                                                                      Record[i].PathWeights.Connection1567.ToString() + "," +
                                                                      Record[i].AverageShortestPath + "\n");

                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);


                        progress = 100 - (((NumberOfRuns - i) * 100) / NumberOfRuns);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("Outputing the Data to the File Has Started. Current Progress: %" + progress);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }

}


