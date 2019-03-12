using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WebSystem.Example.SampleProject.Models
{
    public class SampleTable
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("data")]
        public string Data { get; set; }

        public SampleTable()
        {

        }

        public SampleTable(int id, string data)
        {
            Id = id;
            Data = data;
        }

    }
}
