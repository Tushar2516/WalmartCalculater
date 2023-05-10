using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WalmartCalculater.Actions;
namespace WalmartCalculater.Model
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanges(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private int productId;

        public int ProductId
        {
            get { return productId; }
            set { productId = value; OnPropertyChanges("ProductId"); }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanges("Name"); }
        }

        private double quantity=1;

        public double Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanges("Quantity"); }
        }

        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; OnPropertyChanges("Price"); }
        }

        private bool hasTax;

        public bool HasTax
        {
            get { return hasTax; }
            set { hasTax = value; OnPropertyChanges("HasTax"); }
        }

        private double individualPrice;

        public double IndividualPrice
        {
            get { return individualPrice; }
            set { individualPrice = value; OnPropertyChanges("IndividualPrice"); }
        }

        private double t20Price;

        public double T20Price
        {
            get { return t20Price; }
            set { t20Price = value; OnPropertyChanges("T20Price"); }
        }

        private double t10Price;

        public double T10Price
        {
            get { return t10Price; }
            set { t10Price = value; OnPropertyChanges("T10Price"); }
        }
    }
}
