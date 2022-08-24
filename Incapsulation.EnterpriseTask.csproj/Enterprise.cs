using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        readonly Guid guid;
        string name;
        string inn;
        DateTime establishDate;
        TimeSpan activeTimeSpan;

        public Guid Guid { get; }
        public string Name { get; set; }
        public string Inn 
        {
            get => this.inn; 
            set
            {
                if (this.inn.Length != 10 || !this.inn.All(z => char.IsDigit(z)))
                    throw new ArgumentException();
                this.inn = value;
            }
        }
        public DateTime EstablishDate { get; set; }
        public TimeSpan ActiveTimeSpan { get => DateTime.Now - this.EstablishDate; }

        public Enterprise(Guid guid)
        {
            this.guid = guid;
        }

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            var amount = 0.0;
            foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == guid))
                amount += t.Amount;
            return amount;
        }
    }
}
