using Covid19Analysis.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace Covid19Analysis.CovidXML
{
    /// <summary>
    /// The xml reader
    /// </summary>
    public class XmlReader
    {
        /// <summary>
        /// Gets the covid data
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// Deserialized covid collection
        /// </returns>
        public async Task<List<CovidCase>> GetCovidData(StorageFile file)
        {
            var stream = await file.OpenStreamForReadAsync();
            var deserializer = new XmlSerializer(typeof(List<CovidCase>), new XmlRootAttribute("CovidCollection"));
            var covidCollection = (List<CovidCase>)deserializer.Deserialize(stream);
            stream.Dispose();

            return covidCollection;
        }
    }
}
