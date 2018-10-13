using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XmlSubtitlesParser {

    private static XmlSubtitlesParser _instance;
    private XmlSubtitlesParser() { }
    public static XmlSubtitlesParser Instance {
        get {
            if (_instance == null) _instance = new XmlSubtitlesParser();
            return _instance;
        }
    }

    public List<Subtitle> GetSubtitles(TextAsset xmlFile) {
        if (xmlFile == null) return null;
        List<Subtitle> subtitles = new List<Subtitle>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlFile.text);
        foreach (XmlNode node in xmlDoc.GetElementsByTagName("div").Item(0).ChildNodes) {
            subtitles.Add(new Subtitle(node.Attributes, node.InnerText));
        }
        return subtitles;
    }
}


public class Subtitle {
    public double Begin { get; set; }
    public double End { get; set; }
    public string Id { get; set; }
    public string Text { get; set; }

    public Subtitle(XmlAttributeCollection attributes, string text) {
        double temp;
        Begin = double.TryParse(attributes["begin"].Value.Remove(attributes["begin"].Value.Length - 1), out temp) ? temp : 0;
        End = double.TryParse(attributes["end"].Value.Remove(attributes["end"].Value.Length - 1), out temp) ? temp : 0;
        Id = attributes["id"].Value;
        Text = text;
    }

    public override string ToString() {
        return Begin + " " + Id + " " + End + " " + Text;
    }
}