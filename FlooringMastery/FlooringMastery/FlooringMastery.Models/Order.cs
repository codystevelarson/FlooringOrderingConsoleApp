using System;

namespace FlooringMastery.Models
{
    public class Order
    {
        public DateTime OrderDate { get; set; }
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string State { get; set; }
        public decimal TaxRate { get; set; }
        public string ProductType { get; set; }
        public decimal Area { get; set; }
        public decimal CostPerSquareFoot { get; set; }
        public decimal LaborCostPerSquareFoot { get; set; }
        public decimal MaterialCost => Area * CostPerSquareFoot;
        public decimal LaborCost => Area * LaborCostPerSquareFoot;
        public decimal Tax => (MaterialCost + LaborCost) * (TaxRate / 100);
        public decimal Total => (MaterialCost + LaborCost + Tax);

        public Order() { }
        public Order(Order that)
        {
            OrderDate = that.OrderDate;
            OrderNumber = that.OrderNumber;
            CustomerName = that.CustomerName;
            State = that.State;
            TaxRate = that.TaxRate;
            ProductType = that.ProductType;
            Area = that.Area;
            CostPerSquareFoot = that.CostPerSquareFoot;
            LaborCostPerSquareFoot = that.LaborCostPerSquareFoot;
        }

        public bool AreEqualOrders(Order a, Order b)
        {
            bool result = false;
            if
                (
                a.OrderDate == b.OrderDate
                && a.OrderNumber == b.OrderNumber
                && a.CustomerName == b.CustomerName
                && a.State == b.State
                && a.TaxRate == b.TaxRate
                && a.ProductType == b.ProductType
                && a.Area == b.Area
                && a.CostPerSquareFoot == b.CostPerSquareFoot
                && a.LaborCostPerSquareFoot == b.LaborCostPerSquareFoot
                )
            {
                result = true;
            }
            return result;
        }
    }
}
