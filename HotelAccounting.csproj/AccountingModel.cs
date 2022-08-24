using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price;
        public double Price
        {
            get { return price; }
            set
            { 
                if (value < 0)
                    throw new ArgumentException();
                price = value;
                Notify(nameof(Price));
                Notify(nameof(Total));
            }
            
        }

        private int nightsCount;
        public int NightsCount
        {
            set
            {
                if (value <= 0)
                    throw new ArgumentException();
                nightsCount = value;
                Notify(nameof(NightsCount));
                Notify(nameof(Total));
            }
            get { return nightsCount; }
        }

        private double discount;
        public double Discount
        {
            set
            {
                if (value > 100)
                    throw new ArgumentException();
                discount = value;
				total = price * nightsCount * (1 - discount / 100);
                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
            get { return discount; }
        }

        private double total;
        public double Total
        {
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                total = value;
                discount = (1 - total / (price * nightsCount))*100;
                Notify(nameof(Total));
                Notify(nameof(Discount));
            }
            get { return price * nightsCount * (1 - discount / 100); }
        }
    }
}