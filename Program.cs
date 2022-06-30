/*
 * File:    Program.cs
 * 
 * Author:  Nicholas Shinn
 * 
 * Date:    10/01/2021
 * 
 * Update:  10/01/2021 -> Implemented input processing and created additional classes
 *          10/02/2021 -> Implemented thread creation via Task, Warehouse class, and Delivery class
 *          10/04/2021 -> Finished implementing Order class
 *					06/30/2022 ->	Added files to github repository
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Warehousing
{
    class Program
    {
        /*
        * Function:    Main
        * 
        * Input:       args[0]  ->  Capacity of the orange warehouse
        *              args[1]  ->  Capacity of the blue warehouse
        *              args[2]  ->  Capacity of the aqua warehouse
        * 
        * Description: Main entry point for the program. Takes the desired capacities for the warehouses from command line
        *              and creates them if possible. Begins reading delivery and order specifications from standard input
        *              and creates tasks based on the provided values. Once finished entering tasks, prints results to terminal.
        */
        public static void Main(string[] args)
        {
            //Check if valid number of arguments provided
            if(args.Length != 3)
            {
                Console.WriteLine("ERROR: Invalid arguments, arguments must be in the form: {orange capacity} {blue capacity} {aqua capacity}\n");
                return;
            }

            // Sets up integers to hold the orange, blue, and aqua warehouse capacities
            int orangeCap;
            int blueCap;
            int aquaCap;

            // Tries to parse integers out from command line arguments. If not possible, prints error message and exits
            try
            {
                orangeCap = int.Parse(args[0]);
                blueCap = int.Parse(args[1]);
                aquaCap = int.Parse(args[2]);
            } 
            catch
            {
                Console.WriteLine("ERROR: Invalid argument format, arguments must be valid integers.\n");
                return;
            }

            // Creates a temporary list to hold the warehouses, creates the three warehouses, and then adds them to an object array
            List<Warehouse> houses = new List<Warehouse>();
            houses.Add(new Warehouse("orange", orangeCap));
            houses.Add(new Warehouse("blue", blueCap));
            houses.Add(new Warehouse("aqua", aquaCap));
            Object[] wares = new Object[] { houses[0], houses[1], houses[2] };

            // Creates a list to hold the tasks
            List<Task<string>> tasks = new List<Task<string>>();

            // Prints instruction message to terminal. Input can be provided via a file or manually typed in
            Console.WriteLine("Provide file path or begin typing deliveries/orders. Use ctrl+Z to quit.");

            // Reads first line of input to determine if reading a file or manually
            string inp = Console.ReadLine();

            // If reading from a file, attempts to open the file
            if (File.Exists(inp))
            {
                // Opens a file stream to read input from specified file
                StreamReader fs = new StreamReader(inp);

                // Loops until end of file, holding value in temp
                string temp;
                while ((temp = fs.ReadLine()) != null)
                {
                    // Splits the line on spaces. If line is in format of a delivery, sends arguments to CreateDelivery.
                    // Otherwise, sends arguments to CreateOrder
                    string[] arguments = temp.Split(' ');

                    if (arguments.Length == 3)
                    {
                        CreateDelivery(arguments, tasks, wares);
                    }
                    else if (arguments.Length == 4)
                    {
                        CreateOrder(arguments, tasks, wares);
                    }
                }
            }
            // Else, begins reading input line by line from terminal
            else
            {
                // Loops to read from standard input until ctrl-Z is entered
                while (inp != null && inp != "q" && inp != "quit" )
                {
                    // Splits the line on spaces. If line is in format of a delivery, sends arguments to CreateDelivery.
                    // If in order format, sends arguments to CreateOrder. Otherwise, print error message.
                    string[] arguments = inp.Split(' ');

                    if (arguments.Length == 3)
                    {
                        CreateDelivery(arguments, tasks, wares);
                    }
                    else if (arguments.Length == 4)
                    {
                        CreateOrder(arguments, tasks, wares);
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Invalid delivery or order format.\n");
                    }

                    // Reads the next line
                    inp = Console.ReadLine();
                }
            }

            // Waits for all the tasks to return
            Task.WaitAll(tasks.ToArray());

            // Prints the manifest returned from each task
            foreach(Task<string> t in tasks)
            {
                Console.WriteLine(t.Result);
            }
        }

        /*
        * Function:    CreateDelivery
        * 
        * Input:       arguments    ->  String array holding the delivery amount [0] and color [1]
        *              tasks        ->  Provided list of tasks to add the newly created delivery task to
        *              wares        ->  Object array holding the warehouses
        * 
        * Description: Creates a new Delivery object before using it to start a task
        */
        private static void CreateDelivery(string[] arguments, List<Task<string>> tasks, Object[] wares)
        {
            // Integers to hold the delivery amount and color
            int amt;
            int col;

            // Tries to parse the delivery amount from arguments as well as determine what color to use before creating delivery.
            // Otherwise, prints error message
            try
            {
                amt = int.Parse(arguments[1]);
                switch (arguments[2])
                {
                    case "orange":
                        col = 0;
                        break;
                    case "blue":
                        col = 1;
                        break;
                    case "aqua":
                        col = 2;
                        break;
                    default:
                        col = 3;
                        Console.WriteLine("ERROR: Invalid color. Color must be orange, blue, or aqua.\n");
                        break;
                }
                if (col != 3)
                {
                    Delivery newdel = new Delivery(amt, col, arguments[2]);
                    tasks.Add(Task<string>.Factory.StartNew(newdel.CompleteDelivery, wares));
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Delivery must be in format: delivery {amount} {color}.\n");
            }
        }

        /*
        * Function:    CreateOrder
        * 
        * Input:       arguments    ->  String array holding the orange amount [0], blue amount [1], and aqua amount [2]
        *              tasks        ->  Provided list of tasks to add the newly created order task to
        *              wares        ->  Object array holding the warehouses
        * 
        * Description: Creates a new Order object before using it to start a task
        */
        private static void CreateOrder(string[] arguments, List<Task<string>> tasks, Object[] wares)
        {
            // Tries to parse order amounts from arguments. If possible, creates Order object. Otherwise, prints error message.
            try
            {
                Order neword = new Order(int.Parse(arguments[1]), int.Parse(arguments[2]), int.Parse(arguments[3]));
                tasks.Add(Task<string>.Factory.StartNew(neword.CompleteOrder, wares));
            }
            catch
            {
                Console.WriteLine("ERROR: Order must be in format: order {orange amount} {blue amount} {aqua amount}.\n");
            }
        }
    }
}
