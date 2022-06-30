/*
 * File:    Order.cs
 * 
 * Author:  Nicholas Shinn
 * 
 * Date:    10/01/2021
 * 
 * Update:  10/01/2021 -> Created Order class
 *          10/02/2021 -> Implemented method to check Warehouse stock and adjust if possible
 *          10/04/2021 -> Implemented monitoring to perform tasks asynchronously
 *					06/30/2022 ->	Added files to github repository
 */
using System;
using System.Diagnostics;
using System.Threading;

namespace Warehousing
{
    
    class Order
    {
        // Trackers for how many of each color to order
        private int orange;
        private int blue;
        private int aqua;

        // Stopwatch to track time elapsed
        private Stopwatch timer = new Stopwatch();

        /*
         * Function:    Order
         * 
         * Input:       orangeOrdered   ->  integer representing how many orange to order
         *              blueOrdered     ->  integer representing how many blue to order
         *              aquaOrdered     ->  integer representing how many aqua to order
         * 
         * Description: Constructor for an Order object, sets amount of each color to order
         */
        public Order(int orangeOrdered, int blueOrdered, int aquaOrdered)
        {
            orange = orangeOrdered;
            blue = blueOrdered;
            aqua = aquaOrdered;
        }

        /*
        * Function:    CompleteOrder
        * 
        * Input:       o    ->  Object array containing orange [0], blue [1], and aqua [2] warehouses
        * 
        * Output:      String representation of the order manifest with elapsed time
        * 
        * Description: Checks if an order needs access to each warehouse. If so, grabs the lock for that warehouse
        *              and tries to pull the desired amount of stock. If there isn't enough stock to fill order, 
        *              releases the lock until there is.
        */
        public string CompleteOrder(Object o)
        {
            // Starts the timer
            timer.Start();

            // Converts the object into an object array
            Object[] warehouses = (Object[])o;

            // If ordering orange, grab the orange lock
            if(orange > 0)
            {
                Warehouse og = (Warehouse)warehouses[0];

                Monitor.Enter(og);
                
                // Attempt to grab order. If not enough stock available, release the lock until order can be completed.
                bool done = false;
                while (!done)
                {
                    if (og.GetStock() >= orange)
                    {
                        og.TakeStock(orange);
                        done = true;
                    }
                    else
                    {
                        og.ToString();
                        Monitor.Wait(og);
                    }
                }
                Monitor.Pulse(og);
                Monitor.Exit(og);
            }

            // If ordering blue, grab the blue lock
            if (blue > 0)
            {
                Warehouse be = (Warehouse)warehouses[1];

                Monitor.Enter(be);

                // Attempt to grab order. If not enough stock available, release the lock until order can be completed.
                bool done = false;
                while (!done)
                {
                    if (be.GetStock() >= blue)
                    {
                        be.TakeStock(blue);
                        done = true;
                    }
                    else
                    {
                        be.ToString();
                        Monitor.Wait(be);
                    }
                }
                Monitor.Pulse(be);
                Monitor.Exit(be);
            }

            // If ordering aqua, grab the aqua lock
            if(aqua > 0)
            {
                Warehouse aa = (Warehouse)warehouses[2];

                Monitor.Enter(aa);

                // Attempt to grab order. If not enough stock available, release the lock until order can be completed.
                bool done = false;
                while (!done)
                {
                    if (aa.GetStock() >= aqua)
                    {
                        aa.TakeStock(aqua);
                        done = true;
                    }
                    else
                    {
                        aa.ToString();
                        Monitor.Wait(aa);
                    }
                }
                Monitor.Pulse(aa);
                Monitor.Exit(aa);
            }

            // Stops the timer
            timer.Stop();

            // Returns string representation of order manifest with elapsed time
            return ToString();
        }

        /*
        * Function:    ToString
        * 
        * Output:      String representation of the order manifest with the elapsed time
        * 
        * Description: Returns a copy of the order manifest to be used as the Task result. Elapsed time is
        *              concatenated to the front.
        */
        public override string ToString()
        {

            return timer.ElapsedMilliseconds + " order " + orange + " " + blue + " " + aqua + " \n";
        }
    }
}
