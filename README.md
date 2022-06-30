	Project Info
		Title:   Warehouse
		Created: 10/01/2021
		Updated: 06/30/2021
		Author:  Nicholas Shinn

	Project Description
		Written for Parallel and Distributed Systems at RIT, this program simulates a network of
		three warehouses sending and receiving deliveries using multithreading. Orders and 
		deliveries are processed as Tasks, only changing the stock of a specific warehouse if 
		they are in possession of the lock and there is enough product/space to fulfill the 
		Task. If the lock is currently held by another Task, the thread sleeps until a Monitor
		Pulse alerts the next Task that the lock is available. If the current owner of the lock
		can't complete their Task, they will attempt a partial completion and release the lock to
		the next Task waiting for access.

	Project Dependencies
		.NET 6.0

	Project Usage
		Command Line: dotnet run [orange capacity] [blue capacity] [aqua capacity]
		Orders:       order [orange amount] [blue amount] [aqua amount]
		Deliveries:   delivery [amount] [color]
		* Orders and deliveries can also be processed from a file following the same format
		* Use CTRL + z to quit