using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalmartCalculater.Actions;
using WalmartCalculater.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WalmartCalculater.HTMLHelper;

namespace WalmartCalculater.ViewModel
{
    public class ProductView : INotifyPropertyChanged
    {
        enum AppliedDiscount{Regular,Discount20,Dicsount10 }
        List<Product> HTMLReceivedProduct;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanges(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        ProductServices ProductServices;
        public ProductView()
        {

            ProductServices = new ProductServices();
            CurrentProduct = new Product();
            addProductCommand = new RelayCommand(AddProductAction);
            deleteProductCommand = new RelayCommand(DeleteProductAction);
            searchProductCommand = new RelayCommand(SearchProductAction);
            updateProductCommand = new RelayCommand(UpdateProductAction);
            resetProductCommand = new RelayCommand(ResetProductAction);
            calculateCommand = new RelayCommand(CalculateCommandAction);
            LoadAllProducts();
            (HTMLReceivedProduct,deliverFee, eVouvcher) = HTMLHelper.HTMLHelper.LoadHtml();
            ActionOnHTMLLOadedProduct(HTMLReceivedProduct);
            ProductList = new ObservableCollection<Product>(HTMLReceivedProduct);
        }

        private double discount=0;

        public double Discount
        {
            get { return discount; }
            set {  if (value != double.NaN) {
                    discount = value;
                } OnPropertyChanges("Discount"); }
        }

        private double extraCharges=0;

        public double ExtraCharges
        {
            get { return extraCharges; }
            set {  if (value != double.NaN) {
                    extraCharges = value;
                } OnPropertyChanges("ExtraCharges"); }
        }


        private Product currentProduct;

        public Product CurrentProduct
        {
            get { return currentProduct; }
            set { currentProduct = value; OnPropertyChanges("CurrentProduct"); }
        }


        private ObservableCollection<Product> productList;

        public ObservableCollection<Product> ProductList
        {
            get { return productList; }
            set { productList = value; OnPropertyChanges("ProductList"); }
        }

        private void ActionOnHTMLLOadedProduct(List<Product> products)
        {
            if (products != null)
            {
                ProductServices.CalCulatePrice(products);
                totalPrice = CalculateTotal(products);
                totalPriceFor20Percent = CalculateTotal(products, AppliedDiscount.Discount20);
                totalPriceFor10Percent = CalculateTotal(products, AppliedDiscount.Dicsount10);
                ProductList = new ObservableCollection<Product>(products);
                CalculateCommandAction();
            }
            else
            {
                Message = "No Products Available.";
            }
        }

        private void LoadAllProducts()
        {
            var products = ProductServices.AllProduct();
            if(products != null){
               ProductServices.CalCulatePrice(products);
               totalPrice = CalculateTotal(products);
               totalPriceFor20Percent = CalculateTotal(products,AppliedDiscount.Discount20);
               totalPriceFor10Percent = CalculateTotal(products,AppliedDiscount.Dicsount10);
               ProductList = new ObservableCollection<Product>(products);
               CalculateCommandAction();
            }
            else
            {
                Message = "No Products Available.";
            }
        }

        private double CalculateTotal(List<Product> products,AppliedDiscount appliedDiscount=AppliedDiscount.Regular)
        {
            double total = 0;
            switch(appliedDiscount)
            {
                case AppliedDiscount.Discount20:
                    foreach (Product product in products)
                    {
                        total += (product.T20Price * product.Quantity);
                    }
                    break;
                case AppliedDiscount.Dicsount10:
                    foreach (Product product in products)
                    {
                        total += (product.T10Price * product.Quantity);
                    }
                    break;
                case AppliedDiscount.Regular:
                default:
                    foreach (Product product in products)
                    {
                        total += (product.IndividualPrice * product.Quantity);
                    }
                    break;
            }
            return Math.Round(total, 2, MidpointRounding.ToEven);
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanges("Message"); }
        }

        private double totalPrice;
        private double totalPriceFor20Percent;
        private double totalPriceFor10Percent;

        private double deliverFee=0;

        public double DeliverFee
        {
            get { return deliverFee; }
        }
        
        private double eVouvcher=0;

        public double EVouvcher
        {
            get { return eVouvcher; }
        }


        private double finalPrice=0;

        public double FinalPrice
        {
            get { return finalPrice; }
            set { finalPrice = value; OnPropertyChanges("FinalPrice"); }
        }

        private double final20Price=0;

        public double Final20Price
        {
            get { return final20Price; }
            set { final20Price = value; OnPropertyChanges("Final20Price"); }
        }

        private double final10Price=0;

        public double Final10Price
        {
            get { return final10Price; }
            set { final10Price = value; OnPropertyChanges("Final10Price"); }
        }

        #region Final Price calculation for 10 percent, 20 percent and Regular
        private void CalculateFinalPrice()
        {
            FinalPrice = totalPrice + ExtraCharges - Discount + DeliverFee + EVouvcher;
        }
        private void CalculateFinalPriceFor20Percent()
        {
            Final20Price = totalPriceFor20Percent + ExtraCharges - Discount + DeliverFee + EVouvcher;
        }
        private void CalculateFinalPriceFor10Percent()
        {
            Final10Price = totalPriceFor10Percent + ExtraCharges - Discount + DeliverFee + EVouvcher;
        }
        #endregion

        #region Add Product
        private RelayCommand addProductCommand;

        public RelayCommand AddProductCommand
        {
            get { return addProductCommand; }
        }
        public void AddProductAction()
        {
            try
            {
                var isSaved = ProductServices.AddProduct(CurrentProduct);
                if (isSaved)
                {
                    LoadAllProducts();
                    CurrentProduct = new Product();
                }
                Message = isSaved ? "Saved!!" : "Failed";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        #endregion

        #region Delete Product
        private RelayCommand deleteProductCommand;

        public RelayCommand DeleteProductCommand
        {
            get { return deleteProductCommand; }
        }
        public void DeleteProductAction()
        {
            try
            {
                var isDeleted = ProductServices.DeleteProduct(CurrentProduct.ProductId);
                if (isDeleted)
                {
                    LoadAllProducts();
                    CurrentProduct = new Product();
                }
                Message = isDeleted ? "Deleted!!" : "Failed";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        #endregion

        #region Search Product
        private RelayCommand searchProductCommand;

        public RelayCommand SearchProductCommand
        {
            get { return searchProductCommand; }
        }
        public void SearchProductAction()
        {
            try
            {
                Product SearchedProduct = ProductServices.SearchProduct(CurrentProduct.ProductId);
                if (SearchedProduct != null)
                {
                    CurrentProduct = SearchedProduct;
                }
                Message = SearchedProduct!=null ? "See your product!!" : "Failed";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        #endregion

        #region Update Product
        private RelayCommand updateProductCommand;

        public RelayCommand UpdateProductCommand
        {
            get { return updateProductCommand; }
        }
        public void UpdateProductAction()
        {
            try
            {
                bool isUpdated = ProductServices.UpdateProduct(CurrentProduct);
                if (isUpdated)
                {
                    LoadAllProducts();
                    CurrentProduct = new Product();
                }
                Message = isUpdated?"Updated succsessfully" : "Failed attempt to update";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        #endregion

        #region Reset Product
        private RelayCommand resetProductCommand;

        public RelayCommand ResetProductCommand
        {
            get { return resetProductCommand; }
        }
        public void ResetProductAction()
        {
            CurrentProduct = new Product();
        }
        #endregion

        #region Final Calculation
        private RelayCommand calculateCommand;

        public RelayCommand CalculateCommand
        {
            get { return calculateCommand; }
        }
        private void CalculateCommandAction()
        {
            CalculateFinalPrice();
            CalculateFinalPriceFor20Percent();
            CalculateFinalPriceFor10Percent();
        }
        #endregion

    }
}
