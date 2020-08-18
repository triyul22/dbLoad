using System.Xml;

namespace dbLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlNodeList nodeList;
            XmlDocument doc = new XmlDocument();

            doc.Load("OfferSample.xml");

            nodeList = doc.GetElementsByTagName("offer");

            foreach (XmlElement node in nodeList)
            {
                var type = node.GetAttribute("type"); // получить тип текущего объекта

                HelperMethods.addToDb(HelperMethods.getQueryStrings(node.ChildNodes), type); 
            }
        }

        
    }
}

