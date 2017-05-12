using System.Xml.Linq;
using Hermes.GeoJson.Module.Services;
using Xunit;

namespace Hermes.GeoJson.Testes
{
    /// <summary>
    /// Gpx file parser unit test.
    /// covert gpx file to feature collection 
    /// </summary>
    public class GpxFileParserUnitTest
    {
        [Fact]
        public async void XDocumentParserTest()
        {
            var factory = new RouteFromXMLFactory();
            var document = XDocument.Load(@"C:\Users\d.gordienko\Source\Repos\Hermes.WPF\osrc\hermes.geojson\Hermes.GeoJson.Testes\Data\131328792000000000_84836024.gpx");

            var result = await factory.CreateObjectAsync(document);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
