using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Covid19Analysis.Model;

namespace Covid19Analysis.CovidXML
{
    public class XmlReader
    {
        public async Task<List<CovidCase>> GetCovidData(StorageFile file)
        {
            var stream = await file.OpenStreamForReadAsync();
            var deserializer = new XmlSerializer(typeof(List<CovidCase>), new XmlRootAttribute("CovidCollection"));
            var covidCollection = (List<CovidCase>) deserializer.Deserialize(stream);
            stream.Dispose();

            return covidCollection;
        }
    }
}
