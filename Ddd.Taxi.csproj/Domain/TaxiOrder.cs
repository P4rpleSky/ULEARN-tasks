using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Ddd.Infrastructure;

namespace Ddd.Taxi.Domain
{
	// In real aplication it whould be the place where database is used to find driver by its Id.
	// But in this exercise it is just a mock to simulate database
	public class DriversRepository
	{
		public void FillDriverToOrder(int driverId, TaxiOrder order)
		{
			if (driverId == 15)
				order.SetDriver(driverId, "Drive", "Driverson", new Car("Lada sedan", "Baklazhan", "A123BT 66"));
			else
				throw new Exception("Unknown driver id " + driverId);
		}
	}

	public class TaxiApi : ITaxiApi<TaxiOrder>
	{
		private readonly DriversRepository driversRepo;
		private readonly Func<DateTime> currentTime;
		private int idCounter;

		public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
		{
			this.driversRepo = driversRepo;
			this.currentTime = currentTime;
		}

		public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
			=> TaxiOrder.CreateOrderWithoutDestination(
				new PersonName(firstName, lastName), 
				new Address(street, building), 
				idCounter++,
				currentTime());

		public void UpdateDestination(TaxiOrder order, string street, string building)
			=> order.UpdateDestination(new Address(street, building));

		public void AssignDriver(TaxiOrder order, int driverId)
			=> order.AssignDriver(driverId, driversRepo, currentTime());

		public void UnassignDriver(TaxiOrder order)
			=> order.UnassignDriver();

		public string GetDriverFullInfo(TaxiOrder order)
			=> order.GetDriverFullInfo();

		public string GetShortOrderInfo(TaxiOrder order)
			=> order.GetShortOrderInfo();

		private DateTime GetLastProgressTime(TaxiOrder order)
			=> order.GetLastProgressTime();

		private string FormatName(string firstName, string lastName)
			=> TaxiOrder.FormatName(firstName, lastName);

		private string FormatAddress(string street, string building)
			=> TaxiOrder.FormatAddress(street, building);

		public void Cancel(TaxiOrder order)
			=> order.Cancel(currentTime());

		public void StartRide(TaxiOrder order)
			=> order.StartRide(currentTime());

		public void FinishRide(TaxiOrder order)
			=> order.FinishRide(currentTime());
	}

	public class Driver : Entity<int>
	{
		public Driver(int id, PersonName personName, Car car) : base(id)
        {
			FirstName = personName.FirstName;
			LastName = personName.LastName;
			Car = car;
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Car Car { get; }
    }
	
	public class Car : ValueType<Car>
    {
		public Car(string model, string color, string plateNumber)
		{
			Color = color;
			Model = model;
			PlateNumber = plateNumber;
		}

		public string Color { get; set; }
		public string Model { get; set; }
		public string PlateNumber { get; set; }
	}

	public class OrderTime : ValueType<OrderTime>
	{
		public OrderTime(DateTime creation)
        {
			Creation = creation;
		}
		
		public DateTime Creation { get; }
		public DateTime DriverAssignment { get; set; }
		public DateTime Cancel { get; set; }
		public DateTime StartRide { get; set; }
		public DateTime FinishRide { get; set; }
	}

	public class TaxiOrder : Entity<int>
	{
		public TaxiOrder(PersonName clientName, Address address, int id, DateTime creationTime)
			: base(id)
        {
			ClientName = clientName;
			Start = address;
			Destination = new Address(null, null);
			Driver = new Driver(-1 , new PersonName(null, null), new Car(null, null, null));
			Status = TaxiOrderStatus.WaitingForDriver;
			Time = new OrderTime(creationTime);
		}
 
		public PersonName ClientName { get; }
		public Address Start { get; }
		public Address Destination { get; private set; }
		public Driver Driver { get; private set; }
		public TaxiOrderStatus Status { get; private set; }		
		public OrderTime Time { get; }

		public void SetDriver(int id, string firstName, string lastName, Car car)
			=> Driver = new Driver(id, new PersonName(firstName, lastName), car);

		public void ChangeStatusTo(TaxiOrderStatus newStatus)
		{
			Status = newStatus;
		}

		public static TaxiOrder CreateOrderWithoutDestination(PersonName personName, Address address, int id, DateTime time)
		{
			return new TaxiOrder(personName, address, id, time);
		}

		public void UpdateDestination(Address address)
		{
			Destination = address;
		}

		public void AssignDriver(int driverId, DriversRepository driversRepo, DateTime time)
		{
			if (!(Driver.FirstName is null))
				throw new InvalidOperationException(
					$"Cannot assign driver. He is already assigned");
			driversRepo.FillDriverToOrder(driverId, this);
			Time.DriverAssignment = time;
			ChangeStatusTo(TaxiOrderStatus.WaitingCarArrival);
		}

		public void UnassignDriver()
		{
			if (Driver.FirstName is null || Status == TaxiOrderStatus.InProgress)
				throw new InvalidOperationException(
					$"Cannot unassign driver. Taxi order status: { Status.ToString() }");
			Driver.FirstName = null;
			Driver.LastName = null;
			ChangeStatusTo(TaxiOrderStatus.WaitingForDriver);
		}

		public string GetDriverFullInfo()
		{
			if (Status == TaxiOrderStatus.WaitingForDriver) return null;
			return string.Join(" ",
				"Id: " + Driver.Id,
				"DriverName: " + FormatName(Driver.FirstName, Driver.LastName),
				"Color: " + Driver.Car.Color,
				"CarModel: " + Driver.Car.Model,
				"PlateNumber: " + Driver.Car.PlateNumber);
		}

		public string GetShortOrderInfo()
		{
			return string.Join(" ",
				"OrderId: " + Id,
				"Status: " + Status,
				"Client: " + FormatName(ClientName.FirstName, ClientName.LastName),
				"Driver: " + FormatName(Driver.FirstName, Driver.LastName),
				"From: " + FormatAddress(Start.Street, Start.Building),
				"To: " + FormatAddress(Destination.Street, Destination.Building),
				"LastProgressTime: " + GetLastProgressTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		}

		public DateTime GetLastProgressTime()
		{
			if (Status == TaxiOrderStatus.WaitingForDriver) return Time.Creation;
			if (Status == TaxiOrderStatus.WaitingCarArrival) return Time.DriverAssignment;
			if (Status == TaxiOrderStatus.InProgress) return Time.StartRide;
			if (Status == TaxiOrderStatus.Finished) return Time.FinishRide;
			if (Status == TaxiOrderStatus.Canceled) return Time.Cancel;
			throw new NotSupportedException(Status.ToString());
		}

		public static string FormatName(string firstName, string lastName)
		{
			return string.Join(" ", new[] { firstName, lastName }.Where(n => n != null));
		}

		public static string FormatAddress(string street, string building)
		{
			return string.Join(" ", new[] { street, building }.Where(n => n != null));
		}

		public void Cancel(DateTime time)
		{
			if (Status == TaxiOrderStatus.InProgress)
				throw new InvalidOperationException(
					$"Cannot cancel order. Taxi order status: { Status.ToString() }");
			ChangeStatusTo(TaxiOrderStatus.Canceled);
			Time.Cancel = time;
		}

		public void StartRide(DateTime time)
		{
			if (Driver.FirstName is null)
				throw new InvalidOperationException(
					$"Cannot start ride. Driver isn't assigned yet!");
			ChangeStatusTo(TaxiOrderStatus.InProgress);
			Time.StartRide = time;
		}

		public void FinishRide(DateTime time)
		{
			if (Status != TaxiOrderStatus.InProgress)
				throw new InvalidOperationException(
					$"Cannot finish ride. Taxi order status: { Status.ToString() }");
			if (Driver.FirstName is null)
				throw new InvalidOperationException(
					$"Cannot finish ride. Driver isn't assigned yet!");
			ChangeStatusTo(TaxiOrderStatus.Finished);
			Time.FinishRide = time;
		}
	}
}