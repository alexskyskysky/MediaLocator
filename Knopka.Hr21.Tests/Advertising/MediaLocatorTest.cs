using Knopka.Hr21.Advertising;
using NUnit.Framework;

namespace Knopka.Hr21.Tests.Advertising
{
    public class MediaLocatorTest
    {
        [TestCase("01 Exact Match", "/ru", new[] { "Яндекс.Директ" })]
        [TestCase("02 Substring match only", "/be/msk/vnukovo", new[] { "Беларусская правда" })]
        [TestCase("03 Substring and exact matches", "/ru/svrd/ekb", new[]
        {
            "Яндекс.Директ", "Бегущая строка в троллейбусах Екатеринбурга", "Быстрый курьер",
        })]
        [TestCase("04 All locations match 1", "/ru/svrd/revda",new[]
        {
            "Яндекс.Директ", "Ревдинский рабочий"
        })]
        [TestCase("05 All locations match 2", "/ru/svrd/pervik",new[]
        {
            "Яндекс.Директ", "Ревдинский рабочий"
        })]
        [TestCase("06 No match", "/us/ohio", new string[0])]
        [TestCase("07 No match almost exact smaller", "/be/ms", new string[0])]
        [TestCase("08 No match almost exact bigger", "/be/mskv", new string[0])]
        [TestCase("09 No match as a mid of string", "/svrd", new string[0])]
        [TestCase("10 No match as an end of string", "/svrd/ekb", new string[0])]
        [TestCase("11 Contains does not work", "/us/ru/svrd/ekb", new string[0])]
        [TestCase("12 No match as an end of string", "/ekb", new string[0])]
        // [TestCase("13 Invalid input", "ekb", new string[0])]
        public void ExactMatch(string _, string location, string[] advertisers)
        {
            var locator = BuildMediaLocator("basic.txt");
            Assert.That(locator.GetMediasForLocation(location), Is.EquivalentTo(advertisers));
        }

        private MediaLocator BuildMediaLocator(string sourceFileName)
        {
            var type = GetType();
            var path = type.FullName!.Replace(nameof(MediaLocatorTest), $"TestData.{sourceFileName}");
            using (var sourceStream = type.Assembly.GetManifestResourceStream(path))
                return new MediaLocator(sourceStream);
        }
    }
}