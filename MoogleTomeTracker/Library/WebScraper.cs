using HtmlAgilityPack;
using MoogleTomeTracker.Models;
using MoogleTomeTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Media.Imaging;
using System.IO;

namespace MoogleTomeTracker.Library
{
    public static class MogScrape
    {
        public static async Task<MogData> GetMogItemsFromWeb(string webaddress)
        {
            HttpClient client = new();
            var response = await client.GetAsync(webaddress).ConfigureAwait(false);
            var page = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new();
            doc.LoadHtml(page);

            var id = 0;

            List<MogItem> newItems = new();

            var title = doc.DocumentNode.SelectSingleNode("//html//head//title").InnerText;
            var tName = Regex.Matches(title, @"(?<=\bHunt for\s+)\p{L}+").First().Value;

            foreach (HtmlNode col in doc.DocumentNode.SelectNodes("//table[contains(@class, 'item__list')]//tbody//tr"))
            {
                var imgSrc = col.SelectSingleNode("th//div//div//div//div//img").Attributes["src"].Value;

                var base64Image = await ImageHelper.GetImageAsBase64Url(imgSrc);

                newItems.Add(new MogItem
                {
                    Name = col.SelectSingleNode("th//div//p").InnerText,
                    Id = id,
                    Cost = Convert.ToInt32(col.SelectSingleNode("td").InnerText),
                    Base64Image = base64Image,
                    IsTracked = true,
                    IsAcquired = false
                });

                id++;
            }

            return new MogData
            {
                TomestoneName = tName,
                TomeURL = webaddress,
                MogItems = newItems
            };
        }        
    }

    public static class ImageHelper
    {
        public async static Task<string> GetImageAsBase64Url(string url)
        {
            //var credentials = new NetworkCredential(user, pw);
            //using (var handler = new HttpClientHandler)
            using var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(url);
            //return "image/jpeg;base64," + Convert.ToBase64String(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static BitmapSource BitmapFromBase64(string base64)
        {
            var bytes = Convert.FromBase64String(base64);

            using (var stream = new MemoryStream(bytes))
            {
                return BitmapFrame.Create(stream,
                    BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }
    }
}
