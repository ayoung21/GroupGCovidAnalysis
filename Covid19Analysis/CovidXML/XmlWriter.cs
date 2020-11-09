using Covid19Analysis.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Windows.Storage;

namespace Covid19Analysis.CovidXml
{
    /// <summary>
    ///     Class to save Covid-19 data to a XML file
    /// </summary>
    public class XmlWriter
    {

        /// <summary>Saves the data as CSV.</summary>
        /// <param name="file">The storage file to save to.</param>
        /// <param name="covidCollection">The covid collection to save.</param>
        public async void WriteToXml(StorageFile file, CovidLocationDataCollection covidCollection)
        {
            var allCovidCases = getAllCovidCases(covidCollection);

            var outStream = await file.OpenStreamForWriteAsync();
            var serializer = new XmlSerializer(typeof(List<CovidCase>), new XmlRootAttribute("CovidCollection"));
            serializer.Serialize(outStream, allCovidCases);
            outStream.Dispose();
        }

        private static IList<CovidCase> getAllCovidCases(CovidLocationDataCollection covidCollection)
        {
            return covidCollection.CollectionOfCovidLocationData.SelectMany(currentLocation => currentLocation.Value.CovidCases).ToList();
        }
    }
}