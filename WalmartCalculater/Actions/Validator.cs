using WalmartCalculater.Model;
namespace WalmartCalculater.Actions
{
    public class Validator
    {
        public static bool IsValidateProduct(Product product)
        {
            return !string.IsNullOrEmpty(product.Name) 
                && Validator.ValidQuantity(product.Quantity.ToString()) 
                && ValidPrice(product.Price.ToString());
        }

        private static bool ValidQuantity(string input)
        {
            if (int.TryParse(input, out int res) && res >= 1)
            {
                return true;
            }
            else
            {
                throw new System.Exception("Invalid quantity!!");
            }
        }

        private static bool ValidPrice(string input)
        {
            if (double.TryParse(input, out double res) && res > 0)
            {
                return true;
            }
            else
            {
                throw new System.Exception("Invalid price!!");
            }
        }

        public static bool ValidatePerson(Person person)
        {
            return !string.IsNullOrEmpty(person.Name);
        }

        public static bool ValidateGroup(Group group)
        {
            return group.GroupId != null && group.People != null && !string.IsNullOrEmpty(group.GroupName);
        }
    }
}
