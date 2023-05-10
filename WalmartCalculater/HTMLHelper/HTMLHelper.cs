using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using WalmartCalculater.StringHelper;
using WalmartCalculater.Model;
using WalmartCalculater.Actions;
namespace WalmartCalculater.HTMLHelper
{
    public static class HTMLHelper
    {
        public static (List<Product> products,double deliveryFees,double eVouvcher) LoadHtml()
        {
            List<Product> productsList = new List<Product>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(@"test.html");
            var products = doc.DocumentNode.SelectNodes("//td[@width='65%']").ToList();
            foreach (HtmlNode product in products)
            {
                productsList.Add(ExtractProduct(product));
            }
            return (productsList, getDeliveryFees(doc), geteVouvcher(doc));
        }
        public static double getDeliveryFees(HtmlDocument doc)
        {
            HtmlNode DelievryNode = doc.DocumentNode.SelectSingleNode("//td[text()='Delivery fee :']");
            var nextSibling = DelievryNode.NextSibling;
            var value = nextSibling?.InnerText?.Trim();
            double deliveryFee=!string.IsNullOrEmpty(value)?getValidNumber(value):getValidNumber(getNextSibling(nextSibling));
            return deliveryFee;
        }

        public static double geteVouvcher(HtmlDocument doc)
        {
            HtmlNode DelievryNode = doc.DocumentNode.SelectSingleNode("//td[text()='eVoucher:']");
            var nextSibling = DelievryNode.NextSibling;
            var value = nextSibling?.InnerText?.Trim();
            double eVoucher = !string.IsNullOrEmpty(value) ? getValidNumber(value) : getValidNumber(getNextSibling(nextSibling));
            return eVoucher;
        }
        public static Product ExtractProduct(HtmlNode product)
        {
            double[] values = new double[10];
            Product newProduct = new Product();
            string title = product.SelectNodes(".//td[@align='left']/strong").First().InnerText;
            title=StringHelper.StringHelper.FormateTitle(title);
            newProduct.ProductId = IDGenerator.GetIdForProduct();
            newProduct.Name = title;
                bool ExecutionCompleteForCurrentIteration= false;
                var productAttributes = product.SelectNodes(".//td")?.ToList();
                foreach (var productAttribute in productAttributes)
                {
                    var nextSibling = productAttribute.NextSibling;
                    var value = nextSibling?.InnerText?.Trim();
                    string switchSelecterValue = StringHelper.StringHelper.ExtractValue(productAttribute.InnerText);
                    switch (switchSelecterValue)
                    {
                        case "Price":
                                newProduct.IndividualPrice= !string.IsNullOrEmpty(value) ?getValidNumber(value): getValidNumber(getNextSibling(nextSibling));
                        ExecutionCompleteForCurrentIteration = true;
                            break;
                        case "QTY":
                            newProduct.Quantity = !string.IsNullOrEmpty(value) ? getValidNumber(value) : getValidNumber(getNextSibling(nextSibling));
                        ExecutionCompleteForCurrentIteration = true;
                            break;
                        case "Total":
                            newProduct.HasTax = !string.IsNullOrEmpty(value) ? checkTax(value) : checkTax(getNextSibling(nextSibling));
                            newProduct.Price = !string.IsNullOrEmpty(value) ? getValidNumber(value) : getValidNumber(getNextSibling(nextSibling));
                        ExecutionCompleteForCurrentIteration = true;
                            break;
                    }
                    if (!ExecutionCompleteForCurrentIteration)
                    {
                        bool matchesPattern = Regex.IsMatch(switchSelecterValue, @"^x\d+$");
                        if(matchesPattern)
                        {
                           newProduct.Quantity = getValidNumber(switchSelecterValue);
                        }
                    }
                }
            return newProduct;
        }


        private static bool checkTax(string value)
        {
            return value.Contains("J");
        }

        private static string getNextSibling(HtmlNode node)
        {
            var nextOne = node.NextSibling;
            if (!string.IsNullOrEmpty(nextOne.InnerText.Trim()))
            {
                return StringHelper.StringHelper.ExtractValue(nextOne.InnerText.Trim());
            }
            else
            {
                getNextSibling(nextOne);
            }
            return "";
        }
        public static double getValidNumber(string value)
        {
            if(value.Contains("J"))
            {
                value= value.Replace("J", "");
            }else if (value.Contains("D"))
            {
                value= value.Replace("D", "");
            }
            if (value.Contains("x"))
            {
                value = value.Replace("x", "");
            }
            return double.TryParse(value, out double ValidValue)? ValidValue:0;
        }
    }
}
