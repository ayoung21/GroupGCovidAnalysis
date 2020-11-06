using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using Covid19Analysis.Model;

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
            var allCovidCases = this.getAllCovidCases(covidCollection);

            var outStream = await file.OpenStreamForWriteAsync();
            var serializer = new XmlSerializer(typeof(List<CovidCase>), new XmlRootAttribute("CovidCollection"));
            serializer.Serialize(outStream, allCovidCases);
            outStream.Dispose();
        }

        private IList<CovidCase> getAllCovidCases(CovidLocationDataCollection covidCollection)
        {
            var covidList = new List<CovidCase>();
            foreach (KeyValuePair<string, CovidLocationData> currentLocation in covidCollection.CollectionOfCovidLocationData)
            {
                var locationData = currentLocation.Value.CovidCases;
                foreach (var currentCovidCase in locationData)
                {
                    covidList.Add(currentCovidCase);
                }
            }

            return covidList;
        }
    }
}