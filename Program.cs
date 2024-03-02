using System;
using System.Web;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace HtmlParser;
    class Program {
    static void Main(string[] args)
    {
        var doc = new HtmlDocument();
        doc.Load("..\\..\\..\\sample.html");

        var itemNodes = doc.DocumentNode.SelectNodes("//div[@class='item']").ToList();

        List<string> titles = collectTitles(itemNodes);
        List<double> prices = collectPrices(itemNodes);
        List<double> ratings = collectRatings(itemNodes);

        List<Product> allProducts = new List<Product>();

        
        for(int i = 0 ; i < titles.Count; i++)
        {
            string title = titles[i];
            string price = prices[i].ToString();
            if (prices[i] % 1 == 0) {
                price = price + ".00";
            }
            string rating = ratings[i].ToString();
            Product prod = new Product(title, price, rating);
            allProducts.Add(prod);
        }

        string allProductsJson = JsonConvert.SerializeObject(allProducts, Formatting.Indented);
        Console.WriteLine(allProductsJson);
    }

        static List<string> collectTitles(List<HtmlNode> itemNodes)
        {
            var result = new List<string>();

            foreach (var item in itemNodes)
            {
                var imgNode = item.SelectSingleNode(".//img");
                if (imgNode != null)
                {
                    string title = imgNode.GetAttributeValue("alt", "");
                    string decodedTitle = HttpUtility.HtmlDecode(title);
                    result.Add(decodedTitle);
                }   
                
            }

            return result;
        }

        static List<double> collectPrices(List<HtmlNode> itemNodes)
        {
            var result = new List<double>();

            foreach (var item in itemNodes) {
                var priceNode = item.SelectSingleNode(".//span[@class='price-display formatted']/span");
                if (priceNode != null) {
                    string priceText = priceNode.InnerText.Trim();
                    result.Add(parsedPrice(priceText));
                }
            }

            return result;
        }

        static double parsedPrice(string priceText)
        {
            string parsableText = priceText.Trim('$').Trim(',');
            return double.Parse(parsableText);
        }

        static List<double> collectRatings(List<HtmlNode> itemNodes) {
            var result = new List<double>();
            foreach(var item in itemNodes) {
            string ratingString = item.GetAttributeValue("rating", "");
                if (ratingString != null) { 
                    double rating = double.Parse(ratingString);
                    if (rating > 5)
                    {
                        result.Add(rating / 2);
                    }
                    else {
                        result.Add(rating);
                    }
                }
            }
            return result;
        }


    }
