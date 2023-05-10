using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalmartCalculater.Actions;
namespace WalmartCalculater.Model
{
   public class ProductServices 
    {
        List<Product> products;

        public ProductServices()
        {
            products = new List<Product>();
        }

        public List<Product> AllProduct()
        {
            return products;
        }

        public bool AddProduct(Product product)
        {
            if (Validator.IsValidateProduct(product))
            {
                product.ProductId = IDGenerator.GetIdForProduct();
                products.Add(product);
                return true;
            }
            else
            {
                throw new Exception("Make sure you have entered valid price, name and quantity.");
            }
        }

        public bool DeleteProduct(int id)
        {
            var DeletedCount = products.RemoveAll(product => product.ProductId.Equals(id));
            return DeletedCount > 0;
        }

        public bool UpdateProduct(Product product)
        {
            foreach (Product CurrentProduct in products)
            {
                if (CurrentProduct.ProductId.Equals(product.ProductId) && Validator.IsValidateProduct(product))
                {
                    CurrentProduct.Name = product.Name;
                    CurrentProduct.Price = product.Price;
                    CurrentProduct.Quantity = product.Quantity;
                    return true;
                }
            }
            return false;
        }


        public Product SearchProduct(int productId)
        {
            var FoundedProduct = products.FirstOrDefault(product => product.ProductId.Equals(productId));
            return FoundedProduct;
        }

        public void CalCulatePrice(List<Product> products)
        {
            foreach (Product product in products)
            {
                product.IndividualPrice = CalcualteIndividualProductPrice(product);
                product.T20Price = Cal20PercentDiscount(product);
                product.T10Price = Cal10PercentDiscount(product);
            }
        }

        public double CalcualteIndividualProductPrice(Product product)
        {
            double perUnitPrice = product.Price / product.Quantity;
            double tempStoredPrice = product.HasTax ? perUnitPrice + perUnitPrice * 0.13
    : perUnitPrice;
            return Math.Round(tempStoredPrice, 2, MidpointRounding.ToEven);
        }

        public double Cal20PercentDiscount(Product product)
        {
            double tempStoredPrice;
            double perUnitPrice = product.Price / product.Quantity;
            if (product.HasTax)
            {
                double unitAfter20Percent = perUnitPrice - perUnitPrice*0.2;
                tempStoredPrice = unitAfter20Percent + unitAfter20Percent * 0.13;
            }
            else
            {
                tempStoredPrice = perUnitPrice - perUnitPrice * 0.2;
            }
            return Math.Round(tempStoredPrice, 2, MidpointRounding.ToEven);
        }

        public double Cal10PercentDiscount(Product product)
        {
            double tempStoredPrice;
            double perUnitPrice = product.Price / product.Quantity;
            if (product.HasTax)
            {
                double unitAfter10Percent = perUnitPrice - perUnitPrice*0.1;
                tempStoredPrice = unitAfter10Percent + unitAfter10Percent * 0.13;
            }
            else
            {
                tempStoredPrice=perUnitPrice - perUnitPrice * 0.1;
            }
            return Math.Round(tempStoredPrice, 2, MidpointRounding.ToEven);
        }
    }
}
