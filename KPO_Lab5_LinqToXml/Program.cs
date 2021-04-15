using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace KPO_Lab5_LinqToXml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Task 1: Start...");
            Task1();
            Console.WriteLine("Task 1: Completed\nCreated Xml file - Task1_Result_File\n");
            
            Console.WriteLine("Task 2: Start...");
            
            Task2();
            Console.WriteLine("Task 2 Results:");
            var res = Task2();
            res.ToList().ForEach(x => Console.Write(x));
            Console.WriteLine();
            //foreach (var entry in res)
            //{
            //    Console.WriteLine(entry);
            //}

            Console.WriteLine("Task 3: Start...");
            Task3();
            Console.WriteLine("Task 3: Completed\nCreated Xml file - Task3_Result_File\n");

            Console.WriteLine("Task 4: Start...");
            Task4();
            Console.WriteLine("Task 4: Completed\nCreated Xml file - Task4_Result_File\n");

            Console.WriteLine("Task 5: Start...");
            Task5();
            Console.WriteLine("Task 5: Completed\nCreated Xml file - Task5_Result_File\n");

            Console.WriteLine("Task 6: Start...");
            Task6();
            Console.WriteLine("Task 6: Completed\nCreated Xml file - Task6_Result_File\n");

            Console.WriteLine("Task 7: Start...");
            Task7();
            Console.WriteLine("Task 7: Completed\nCreated Xml file - Task7_Result_File\n");

            Console.WriteLine("Task 8: Start...");
            Task8();
            Console.WriteLine("Task 8: Completed\nCreated Xml file - Task8_Result_File\n");

        }

        //LinqXml4
        static void Task1()
        {
            XDocument res;
            XElement root = new XElement("root");
            string fileName = "Task1_File.txt";
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs);
                while (!sr.EndOfStream)
                {
                    string[] words = sr.ReadLine().Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    //XElement nextLine = new XElement("line");
                    //foreach (string word in words)
                    //    nextLine.Add(new XElement("word", word));
                    
                    root.Add( new XElement( "line", from x in words orderby x select new XElement("word", x)));
                }
            }
            res = new XDocument(root);
            res.Save(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task1_Result_File");
        }

        //LinqXml12
        static IEnumerable<string> Task2()
        {
            XDocument src = XDocument.Load(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task2_Input_File");
    
            var temp = from str in (from elem in (from x in src.Root.Elements() orderby x.Name.LocalName group x by x.Name) select (elem.Key.ToString() + " //\t" + elem.AsEnumerable().Count().ToString() + "\n")) select str;

            return temp;
            //var groups = src.Root.Elements().GroupBy(x => x.Name).Select(x => x.Key.ToString());

            //var nums = from x in groups select x.Count();

            //Dictionary<string,int> namesDict = new Dictionary<string, int>();
            //foreach (XElement item in src.Root.Elements())
            //{
            //    string key = item.Name.ToString();
            //    if (namesDict.Keys.Contains(key)) namesDict[key]++;
            //    else namesDict.Add(key, 1);
            //}


        }

        //LinqXml24
        static void Task3()
        {
            XDocument src = XDocument.Load(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task3_Input_File");
            XDocument res = new XDocument(src);

            res.Root.DescendantNodes().Where(x => x is XComment && (x.Parent.Name == "root" || x.Parent.Parent.Name == "root")).ToList().ForEach(x => x.Remove());

            //List<XNode> remNodes = new List<XNode>();
            //foreach (var item in res.Root.Nodes())
            //{
            //    if (item is XComment) {remNodes.Add(item); continue;}
            //    if (item is XElement)
            //    {
            //        foreach (var firstItem in ((XElement)item).Nodes())
            //            if (firstItem is XComment) remNodes.Add(firstItem);
            //    }
            //}
            //foreach (var node in remNodes)
            //{
            //    node.Remove();
            //}
            res.Save(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task3_Result_File");

        }

        //LinqXml34
        static void Task4()
        {
            XDocument src = XDocument.Load(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task4_Input_File");
            XDocument res = new XDocument(src);

            res.Root.Elements().Where(x => x.HasAttributes).ToList().
                ForEach(x => x.ReplaceAttributes(x.Attributes().Select(attr => new XElement(attr.Name, attr.Value))));
            
            

            //foreach (var item in res.Root.Elements())
            //{
            //    if (!item.HasAttributes)
            //        continue;
            //    var list = from attr in item.Attributes()
            //               select new XElement(attr.Name, attr.Value);

            //    item.ReplaceAttributes(list);
            //}

            res.Save(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task4_Result_File");
        }

        //LinqXml44
        static void Task5()
        { 
            string attrName = "number";
            XDocument src = XDocument.Load(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task5_Input_File");
            XDocument res = new XDocument(src);

            foreach (var item in res.Descendants())
            {
                if (!item.HasElements) continue;
                GetMinAttr(item, attrName);
            }

            res.Save(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task5_Result_File");
        }

        static void GetMinAttr(XElement item, string aName)
        {
            double? min = null;
            foreach(var elem in item.Descendants())
            {
                if (!elem.HasAttributes) continue;
                foreach(var attr in elem.Attributes())
                {
                    if (attr.Name.ToString() == aName && (min == null || double.Parse(attr.Value) < min)) min = double.Parse(attr.Value.ToString());
                }
            }
            item.SetAttributeValue("min", min);
        }

        //LinqXml54
        static void Task6()
        {
            XDocument src = XDocument.Load(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task6_Input_File");
            XDocument res = new XDocument(src);

            XNamespace rootNs = res.Root.Name.NamespaceName;
            foreach(var item in res.Root.Elements())
            {
                XElement prefElem = new XElement(rootNs + item.Name.LocalName);
                item.Name = prefElem.Name;
                foreach(var item2 in item.Elements())
                {
                    XElement prefElem2 = new XElement(rootNs + item2.Name.LocalName);
                    item2.Name = prefElem2.Name;
                }
            }
            res.Save(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task6_Result_File");
        }

        //LinqXml64
        static void Task7()
        {
            XDocument src = XDocument.Load(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task7_Input_File");

            DateTime time = default;

            List<int> years = new List<int>();
            Dictionary<int, int> clientsTime = new Dictionary<int, int>();
            Dictionary<int, int> yearClients = new Dictionary<int, int>();
            Dictionary<int, int> monthClients = new Dictionary<int, int>();

            //Формирование списков
            foreach(var item in src.Root.Elements())
            {
                foreach (var data in item.Elements())
                {
                    if (data.Name.LocalName == "date")
                    {
                        yearClients.Add(int.Parse(item.Attribute("id").Value), DateTime.Parse(data.Value).Year);
                        if (!years.Contains(DateTime.Parse(data.Value).Year))
                        {
                            years.Add(DateTime.Parse(data.Value).Year);
                        }
                        monthClients.Add(int.Parse(item.Attribute("id").Value), DateTime.Parse(data.Value).Month);
                    }
                    if (data.Name.LocalName == "time")
                    {
                        string timestr = GetTimeString(data.Value);
                        time = DateTime.Parse(timestr);
                        clientsTime.Add(int.Parse(item.Attribute("id").Value), time.Hour * 60 + time.Minute);
                    }
                }
            }

            //Сортировка списков
            years = years.OrderByDescending(x => x).ToList();
            yearClients = yearClients.OrderBy(x => x.Key).ToDictionary(x=>x.Key, y=>y.Value);

            //Создание Xml документа
            List<XElement> elems = new List<XElement>();
            foreach (var year in years)
            {
                XElement currElem = new XElement($"y{year}");
                for (int i = 1; i < 13; i++)
                {
                    var tempID = from client in monthClients where client.Value == i select client.Key;
                    if (monthClients.ContainsValue(i) && yearClients[tempID.First()]==year) currElem.Add(new XElement($"m{i}"));
                }
                    

                foreach (var entry in yearClients)
                {
                    if (entry.Value == year)
                    {
                        int month = monthClients[entry.Key];
                        foreach (var m in currElem.Elements())
                        {
                            if (m.Name.LocalName == $"m{month}") 
                                m.Add(new XElement("client",
                                    new XAttribute("id", entry.Key),
                                    new XAttribute("time", clientsTime[entry.Key])));
                        }
                    }
                }

                elems.Add(currElem);

                XDocument res = new XDocument(new XElement("root"));
                res.Root.Add(elems);
                res.Save(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task7_Result_File");
            }

        }

        static string GetTimeString(string str)
        {
            str = str.Trim('P');
            str = str.Trim('T');
            str = str.Trim('M');

            string[] time = str.Split('H');

            string res = $"{time[0]}:{time[1]}:00";
            return res;
        }

        //LinqXml74

        static void Task8()
        {
            XDocument src = XDocument.Load(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task8_Input_File");

            SortedDictionary<string, Comp> comps = new SortedDictionary<string, Comp>();
            SortedDictionary<string, Station> allStations = new SortedDictionary<string, Station>();
            SortedDictionary<string, Brand> allBrands = new SortedDictionary<string, Brand>();

            //Формирование списка
            foreach (var item in src.Root.Elements())
            {
                string[] temp = item.Attribute("station").Value.Split('_');
                string name = temp[1];
                string street = temp[0];
                Station tempStation = new Station() { Street = street };
                Brand tempBrand = new Brand() { Label = item.Name.LocalName.ToString(), Cost = item.Attribute("price").Value };


                if (!comps.ContainsKey(name))
                {
                    comps.Add(name, new Comp() { Name = name });
                    tempStation.brands.Add(tempBrand.Label, tempBrand);
                    comps[name].stations.Add(tempStation.Street,tempStation);
                    if (!allStations.ContainsKey(tempStation.Street)) allStations.Add(tempStation.Street, tempStation);
                    if (!allBrands.ContainsKey(tempBrand.Label)) allBrands.Add(tempBrand.Label, tempBrand);
                }
                else
                {
                    if (!comps[name].stations.ContainsKey(street))
                    {
                        
                        tempStation.brands.Add(tempBrand.Label, tempBrand);
                        comps[name].stations.Add(tempStation.Street, tempStation);
                        if (!allStations.ContainsKey(tempStation.Street)) allStations.Add(tempStation.Street, tempStation);
                        if (!allBrands.ContainsKey(tempBrand.Label)) allBrands.Add(tempBrand.Label, tempBrand);
                    }
                    else
                    {
                        comps[name].stations[street].brands.Add(tempBrand.Label, tempBrand);
                        if (!allBrands.ContainsKey(tempBrand.Label)) allBrands.Add(tempBrand.Label, tempBrand);
                    }
                }
            }

            
            //Дополнение
            foreach (var comp in comps.Values)
            {
                foreach (var station in allStations.Values)
                {
                    if (!comp.stations.ContainsKey(station.Street))
                        comp.stations.Add(station.Street, new Station() { Street = station.Street});
                }
                
                foreach (var station in comp.stations.Values)
                {
                    foreach (var brand in allBrands.Keys)
                    {
                        if (!station.brands.ContainsKey(brand))
                            station.brands.Add(brand, new Brand() { Label = brand, Cost = "0" });
                    }
                }
            }

            //Создание Xml документа

            XDocument res = new XDocument(new XElement("Root"));

            foreach (var comp in comps.Values)
            {
                XElement entry = new XElement(comp.Name);
                foreach (var station in comp.stations)
                {
                    XElement subEntry = new XElement(station.Value.Street);
                    foreach (var brand in station.Value.brands.Values)
                        subEntry.SetAttributeValue(brand.Label, brand.Cost);
                    entry.Add(subEntry);
                }
                res.Root.Add(entry);
            }

            res.Save(@"C:\Users\Acer\source\repos\KPO_Lab5_LinqToXml\KPO_Lab5_LinqToXml\Result_Xml_Files\Task8_Result_File");
        }

        public class Comp
        {
            public string Name { get; set; }

            public SortedDictionary<string,Station> stations = new SortedDictionary<string, Station>();
            
        }

        public class Station
        {
            public string Street { get; set; }

            public SortedDictionary<string,Brand> brands = new SortedDictionary<string, Brand>();
        }

        public class Brand
        {
            public string Label { get; set; }
            public string Cost { get; set; }
        }

    }
}
