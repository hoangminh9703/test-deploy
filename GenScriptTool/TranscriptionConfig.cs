using System.Xml;

public class TranscriptionConfig
{
    public string OpenAIKey { get; private set; }
    public string Language { get; private set; }
    public string OutputPath { get; private set; }

    public TranscriptionConfig()
    {
        LoadFromFile(Path.Combine(AppContext.BaseDirectory, "config.xml"));
    }

    private void LoadFromFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Không tìm thấy file config.xml", path);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlNodeList settings = xmlDoc.SelectNodes("//appSettings/add");
        if (settings != null)
        {
            foreach (XmlNode node in settings)
            {
                string key = node.Attributes?["key"]?.Value ?? "";
                string value = node.Attributes?["value"]?.Value ?? "";

                switch (key)
                {
                    case "OpenAIKey":
                        OpenAIKey = value;
                        break;
                    case "Language":
                        Language = value;
                        break;
                    case "OutputPath":
                        OutputPath = value;
                        break;
                }
            }
        }

        if (string.IsNullOrEmpty(OpenAIKey))
            throw new Exception("Thiếu cấu hình OpenAIKey trong config.xml");
    }
}
