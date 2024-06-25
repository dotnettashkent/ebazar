public class MediaHelper
{
    public static string MakeVideoName(string filename)
    {
        FileInfo fileInfo = new FileInfo(filename);

        string extension = fileInfo.Extension;
        string name = Guid.NewGuid() + extension;

        return name;
    }

    public static string[] GetVideoExtensions()
    {
        return new string[]
        {
        ".mp4",
        ".mkv",
        ".flv",
        ".mov",
        ".avi",
        ".wmv",
        ".MP4",
        ".MKV",
        ".FLV",
        ".MOV",
        ".AVI",
        ".WMV",
        ".jpg",
        ".img",
        ".png",
            // you can add more video extensions here
        };
    }
}
