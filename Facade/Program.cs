
using System;

namespace Facade
{
    class Program
    {
        static void Main(string[] args)
        {
            ShoppingItem shoppingItem = new ShoppingItem();
            shoppingItem.AddItemToCart("item1",5);

        }
    }

    public class Stock
    {
        public bool CheckItemStockByItemID(string itemId)
        {
            Console.WriteLine($"Item id:{itemId} is available");
            return true;
        }

        public bool LockItemsTemporary(string itemId, int qty)
        {
            Console.WriteLine($"Item id: {itemId} ,{qty} are locked");
            return true;
        }
    }

    public class Taxes
    {
        public string GetStateTax(string stateCode)
        {
            return $"{stateCode} state tax is 6% ";
        }
    }

    public class ItemCost
    {
        public string GetItemCostByItemID(string itemId)
        {
            Console.WriteLine($"Item id:{itemId} cost is $35");
            return "35";
        }
    }

    public class ShoppingItem
    {
        public bool AddItemToCart(string itemCode, int qty)
        {
            Stock stock = new Stock();
            stock.CheckItemStockByItemID(itemCode);
            stock.LockItemsTemporary(itemCode, qty);

            ItemCost item = new ItemCost();
            item.GetItemCostByItemID(itemCode);

            Taxes tx = new Taxes();
            tx.GetStateTax("NG");

            return true;
        }
    }
}