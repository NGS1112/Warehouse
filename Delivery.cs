/*
 * File:    Delivery.cs
 * 
 * Author:  Nicholas Shinn
 * 
 * Date:    10/01/2021
 * 
 * Update:  10/01/2021 -> Created Delivery class
 *          10/02/2021 -> Implemented Delivery class for delivering asynchronously and making partial deliveries
 *					06/30/2022 ->	Added files to github repository
 */
using System;
using System.Threading;
using System.Diagnostics;

namespace Warehousing
{
    class Delivery
    {
        // Variables for the delivery amount, color value used as warehouse index, and color name
        private int amount;
        private int color;
        private string colorName;

        // Stopwatch to be used for elapsed time
        private Stopwatch timer = new Stopwatch();

        /*
        * Function:    Delivery
        * 
        * Input:       deliveryAmount       ->  integer representing how many units delivered
        *              deliveryColor        ->  integer representing warehouse index to use
        *              deliveryColorName    ->  string representing color of units being delivered
        * 
        * Description: Constructor for a Delivery object. Sets amount of units, warehouse to be used, and color of units
        */
        public Delivery(int deliveryAmount, int deliveryColor, string deliveryColorName)
        {
            amount = deliveryAmount;
            color = deliveryColor;
            colorName = deliveryColorName;
        }

        /*
        * Function:    CompleteDelivery
        * 
        * Input:       o    ->  Object array containing the warehouses to be used
        * 
        * Output:       String representation of the order manifest with elapsed time
        * 
        * Description: Grabs the lock for the desired warehouse before checking if there is enough space to complete delivery.
        *              If there is, delivers the stock and releases the lock. If there isn't, delivers as much as possible
        *              before releasing the lock and looping again once awakened. 
        */
        public string CompleteDelivery(Object o)
        {
            // Starts the timer
            timer.Start();

            // Retrieves destination warehouse based on delivery color
            Object[] temp = (Object[])o;
            Warehouse destination = (Warehouse)temp[color];

            // Grabs the lock for the warehouse
            Monitor.Enter(destination);

            // If warehouse has enough space, complete the delivery
            if(destination.GetSpace() >= amount)
            {
                destination.AddStock(amount);
            }
            // Otherwise, attempt partial delivery and release lock until space is available
            else
            {
                int partial = amount;
                while(partial > 0)
                {
                    if(destination.GetSpace() > 0)
                    {
                        partial -= destination.GetSpace();
                        destination.AddStock(destination.GetSpace());
                    }
                    else
                    {
                        Monitor.Wait(destination);
                    }
                }
            }
            
            // Alert the next task the lock is available and release it
            Monitor.Pulse(destination);
            Monitor.Exit(destination);

            // Stops the timer
            timer.Stop();

            // Returns string representation of delivery manifest with elapsed time
            return ToString();
        }

        /*
        * Function:    ToSTring
        * 
        * Output:      String representation of the delivery manifest with elapsed time
        * 
        * Description: Concatenates the elapsed time to a copy of the delivery manifest.
        */
        public override string ToString()
        {
            return timer.ElapsedMilliseconds + " deliver " + amount + " " + colorName + "\n";
        }
    }
}
