/*
 * File:    Warehouse.cs
 * 
 * Author:  Nicholas Shinn
 * 
 * Date:    10/01/2021
 * 
 * Update:  10/01/2021 -> Created Warehouse class
 *          10/02/2021 -> Implemented Warehouse class to track capacity, stock, received stock, and ordered stock.
 *                        Added methods for other classes to alter stock
 *					06/30/2022 ->	Added files to github repository
 */
namespace Warehousing
{
    class Warehouse
    {
        // Color and capacity of the warehouse set by constructor
        private string color;
        private int capacity;

        // Integers for current stock, stock received, and stock ordered
        private int stock = 0;
        private int received = 0;
        private int ordered = 0;

        /*
         * Function:    Warehouse
         * 
         * Input:       warehouseColor      ->  string for color of the warehouse being created
         *              warehouseCapacity   ->  integer for capacity of the warehouse being created
         * 
         * Description: Constructor for a warehouse object. Sets the color and size.
         */
        public Warehouse(string warehouseColor, int warehouseCapacity)
        {
            color = warehouseColor;
            capacity = warehouseCapacity;
        }

        /*
         * Function:    AddStock
         * 
         * Input:       change  ->  integer representing how much to add to stock
         * 
         * Description: Adds stock to the warehouse when called by a delivery. Adjusts stock and received values.
         */
        public void AddStock(int change)
        {
            // Adds change to stock and recieved integers
            received += change;
            stock += change;
        }

        /*
         * Function:    TakeStock
         * 
         * Input:       change  ->  integer representing how much to remove from stock
         * 
         * Description: Remoives stock from the warehouse when called by an order. Adjusts stock and ordered values.
         */
        public void TakeStock(int change)
        {
            // Adds change to ordered integer and subtracts it from stock integer
            ordered += change;
            stock -= change;
        }

        /*
         * Function:    GetStock
         * 
         * Output:      stock   -> integer representing how much stock the warehouse currently has
         * 
         * Description: Returns the current stock of the warehouse. Used by orders to determine if order can be filled.
         */
        public int GetStock()
        {
            return stock;
        }

        /*
         * Function:    GetSpace
         * 
         * Output:      Integer representing how much space there is in the warehouse
         * 
         * Description: Returns the calculated difference between the warehouse capacity and current stock. Used by 
         *              deliveries to determien if there is room to complete the delivery.
         */
        public int GetSpace()
        {
            // Returns calculated available space
            return capacity - stock;
        }

        /*
         * Function:    ToString
         * 
         * Output:      String representing the warehouse object
         * 
         * Description: Returns a string representation of the warehouse detailing the color, how much stock was recieved,
         *              how much stock was ordered, and what is left over.
         */
        public override string ToString()
        {
            return color + " warehouse: " + received + " - " + ordered + ": " + stock + "\n";
        }
    }
}
