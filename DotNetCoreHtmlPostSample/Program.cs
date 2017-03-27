using System;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreHtmlPostSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (args.Length == 0)
            {
                Console.WriteLine("引数に検索したいワードを入力してください");
                return;
            }

            nicoDicResultList(args[0]).Wait();
        }

        private static async Task nicoDicResultList(string searchWord)
        {
            var nicoNicoDic = new NicoNicoDic();
            var resultList = await nicoNicoDic.searchResultList(searchWord);

            foreach (var result in resultList)
            {
                Console.WriteLine("{0}", result.title);
            }
        }
    }
}