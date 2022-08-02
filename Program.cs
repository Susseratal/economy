using System;
using Glob = System.Globalization;

#nullable disable 

namespace Game
{
    // economy object //
    public class Economy
    {
        public string name;
        public int val;

        public Economy(string nameInit, int valInit)
        {
            name = nameInit;
            val = valInit;
        }
    }

    // Item data type //
    public class Item
    {
        public string category;
        public int cost;

        public Item(string categoryInit, int costInit)
        {
            category = categoryInit;
            cost = costInit;
        }

        public static Item[] makeItems()
        {
            var gun = new Item("Gun", 200);
            var jeep = new Item("jeep", 200);
            var robot = new Item("Robot", 200);

            Item[] items = new Item[3] {gun, jeep, robot};
            return items;
        }
    }
    
    // business object //
    public class Node
    {
        public string name;
        public int supply; // how many units of goods they have to sell
        public int money; // how many pesos they have
        public Item buy;
        public Item sell;
        public int demand; // how much of the stuff they buy they own

        public Node(string nameInit, int supplyInit, int moneyInit, Item buyInit, Item sellInit, int demandInit)
        {
            name = nameInit;
            supply = supplyInit;
            money = moneyInit;
            buy = buyInit;
            sell = sellInit;
            demand = demandInit;
        }

        public static Node[] createNodes(Item gun, Item jeep, Item robot, int totalVal)
        {
            int money = (totalVal/3);
            int demand = 5;

            var gunsInc = new Node("gunsInc", 10, money, robot, gun, demand);
            var jeepInc = new Node("jeepInc", 10, money, gun, jeep, demand);
            var robotsInc = new Node("robotsInc", 10, money, jeep, robot, demand);

            Node[] companies = new Node[3] {gunsInc, jeepInc, robotsInc};
            return companies;
        }

        public static void turn()
        {
            // take a turn beefcake
        }
    }

    class Program
    {
        public static string divider = "///////////////////////////////////////////////////////////////////";

        static void displayCurrent(int nationalVal, int week, Economy nation, Node[] companies)
        {
            Console.WriteLine("{0}", divider);
            Console.WriteLine("Week {0}", week);

            foreach(Node n in companies)
            {
                Console.WriteLine("{0} money = {1}      |       supply = {2}", n.name, n.money, n.supply);
                nationalVal = nationalVal + n.money;
            }

            Console.WriteLine("{0} total value = {1}", nation.name, nationalVal); // national value is calculated by how much money the businesses have
        }
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Nation name and initial value are required to run the sim"); 
            }
            else
            {

                Item[] items = Item.makeItems(); // make some items

                string name = args[0];
                Glob.TextInfo textInfo = new Glob.CultureInfo("en-UK", false).TextInfo;
                name = textInfo.ToTitleCase(name);

                string valString = args[1];
                int val = Int32.Parse(valString);
                Economy nation = new Economy(name, val); // initialise a new nation with a given name and starting wealth

                Node[] companies = Node.createNodes(items[0], items[1], items[2], nation.val); // create all the companies, each being given a third of the nation's wealth

                int week = 1;
                while(week <= 52) // primary loop
                {
                    displayCurrent(0, week, nation, companies);
                    foreach(Node n in companies)
                    {
                        double increase = (n.demand*0.5);
                        n.supply = (n.supply + Convert.ToInt32(increase)); // each week the company generates units of their item based on (demand*0.5) - rounds down so if you got 1 demand, fuck you
                    }

                    week++; // it's a new dawn it's a new day
                    Console.Read();
                }
            }
        }
    }
}
