using SDE.Databases.Generic.Parser;
using SDE.Editor.Database;
using System;

namespace SDE.Databases.ItemPackages.Parser
{
    public class ItemPackageParserProvider :
        DatabaseParserProvider
    {
        public override DatabaseReader GetReader(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Yaml:
                    return new ItemPackageReaderYaml();
            }

            throw new Exception(
                "No reader found for the specified format: '" +
                fileType + "'.");
        }

        public override DatabaseWriter GetWriter(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Yaml:
                    return new ItemPackageWriterYaml();
            }

            throw new Exception(
                "No writer found for the specified format: '" +
                fileType + "'.");
        }
    }
}