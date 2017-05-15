using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

//來源 https://www.youtube.com/watch?v=oeuvL1_5UIQ

namespace crowler1
{
    class Program
    {        
        static void Main(string[] args)
        {
            //指定要爬取的網址
            string url = "http://www.automobile.tn/neuf/bmw.3/";
            startCrowlerAsync(url);
            Console.ReadLine();
        }
        private static async Task startCrowlerAsync(string url)
        {
            //取得網頁原始碼，存進string html中
            var httpCleint = new HttpClient();
            string html = await httpCleint.GetStringAsync(url);

            //尋找整個html內的<div class="article_new_car article_last_modele">....</div>，存於divs中
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            List<HtmlNode> divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("article_new_car article_last_modele"))
                .ToList();
            Console.WriteLine(  "get div list");

            //依序處理每個div的內容        
            foreach (HtmlNode div in divs)
            {
                //<h2>BMW Série 3</h2>       
                string name = div?.Descendants("h2")?.FirstOrDefault().InnerText; //use ? to check if not null

                string price = div.Descendants("div").FirstOrDefault().InnerText;
                                
                //<img src="https://catalogue.automobile.tn/max/2015/12/22263.jpg" alt="BMW Série 3">
                //使用childattributes進入src，並擷取其value
                string imgUrl = div.Descendants("img").FirstOrDefault()
                    .ChildAttributes("src").FirstOrDefault().Value;
                
                string link = div.Descendants("a").FirstOrDefault()
                    .ChildAttributes("href").FirstOrDefault().Value;
                Console.WriteLine(name +"  :  "+price+"          "+imgUrl);
            }
            


        }
    }
}
