namespace Publications.ConsoleTests;

internal class FileSystemVisiter
{
    public int Visit(FileSystemInfo info)
    {
        Console.WriteLine(info.FullName);

        var lines_count = 0;
        switch (info)
        {
            case DirectoryInfo dir:

                foreach (var dir_info in dir.EnumerateFileSystemInfos())
                {
                    lines_count += Visit(dir_info);
                }

                break;

            case FileInfo { Extension: ".cs" } file:
                using (var reader = file.OpenText())
                    while (!reader.EndOfStream)
                        if (reader.ReadLine() is { Length: > 0 })
                            lines_count++;

                Console.WriteLine("Число строк в файле {0}: {1}", info.Name, lines_count);
                break;
        }

        return lines_count;
    }
}
