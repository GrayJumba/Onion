using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace SDLibrary.SmartDesk
{
   
    public static class ObjectCopier
    {
        public static Grid clone(Grid mygrid)
        {
            if (mygrid == null)
                return null;
            string gridXaml = XamlWriter.Save(mygrid);
            StringReader stringReader = new StringReader(gridXaml);
            XmlReader xmlReader = XmlTextReader.Create(stringReader, new XmlReaderSettings());
            return (Grid)XamlReader.Load(xmlReader);
        }
    }
}
