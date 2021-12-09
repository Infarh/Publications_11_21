namespace Publications.ConsoleTests;

internal class FileSystemVisiter
{
    public int CountSourceCodeLines(FileSystemInfo info)
    {
        Console.WriteLine(info.FullName);

        var lines_count = 0;
        switch (info)
        {
            case DirectoryInfo dir:

                foreach (var dir_info in dir.EnumerateFileSystemInfos())
                {
                    lines_count += CountSourceCodeLines(dir_info);
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

    public List<FileInfo> GetSourceFiles(FileSystemInfo info)
    {
        var result = new List<FileInfo>();

        switch (info)
        {
            case DirectoryInfo dir:

                foreach (var dir_info in dir.EnumerateFileSystemInfos())
                {
                    result.AddRange(GetSourceFiles(dir_info));
                }

                break;

            case FileInfo { Extension: ".cs" } file:
                result.Add(file);
                break;
        }

        return result;
    }
}
