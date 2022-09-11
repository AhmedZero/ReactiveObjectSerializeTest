using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReactiveObjectSerializeTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<BrandItem> test = new List<BrandItem>(6);
            for (int i = 0; i < 6; i++)
            {
                var item = new BrandItem() { Header = "Test" + i, QuerySearch = "Test" + i };
                for (int j = 0; j < 50000; j++)
                {
                    item.Contents.Add(new ProductInformations() { Product = "Test" + j });
                }
                test.Add(item);
            }
            var teststring = JsonSerializer.Serialize(test);
            Console.WriteLine("Write File");
            File.WriteAllText("test.json", teststring);
            teststring = "";
            Console.WriteLine("Done");

            Console.ReadKey();
        }

        public sealed class BrandItem : ReactiveObject
        {
            public string? Header { get; set; }
            public string? QuerySearch { get; set; }

            public bool? Enabled { get; set; } = true;

            public ObservableCollection<ProductInformations>? Contents { get; set; } = new();
        }
        public class PriceHistoryData : ReactiveObject
        {
            public double Price { get; set; }
            public DateTime date { get; set; }

        }

        public sealed class ProductInformations : ReactiveObject
        {
           
            public static Comparison<ProductInformations?> SortAscending(Func<ProductInformations, string> selector)
            {
                return (x, y) =>
                {
                    if (x is null && y is null)
                        return 0;
                    else if (x is null)
                        return -1;
                    else if (y is null)
                        return 1;
                    else
                        return string.Compare(selector(x), selector(y));
                };
            }

            public static Comparison<ProductInformations?> SortDescending(Func<ProductInformations, string> selector)
            {
                return (x, y) =>
                {
                    if (x is null && y is null)
                        return 0;
                    else if (x is null)
                        return 1;
                    else if (y is null)
                        return -1;
                    else
                        return string.Compare(selector(y), selector(x));
                };
            }

            private string? product;
            private bool? outofstock = false;

            public string? Product { get => product; set { product = value; this.RaiseAndSetIfChanged(ref product, value); } }

            public ObservableCollection<PriceHistoryData> PriceHistory { get; set; } = new();
            public bool? Outofstock { get => outofstock; set { this.RaiseAndSetIfChanged(ref outofstock, value); } }


            public string Sku { get; set; }
            public string? Link { get; set; }

        }
    }
}