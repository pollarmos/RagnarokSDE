using SDE.Databases.Generic.Parser;
using SDE.Editor.Database;
using System;

namespace SDE.Databases.ItemRandomOptions.Parser
{
    public class ItemRandomOptionParserProvider
        : DatabaseParserProvider
    {
        public override DatabaseReader GetReader(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Yaml:
                    return new ItemRandomOptionReaderYaml();
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
                    return new ItemRandomOptionWriterYaml();
            }

            throw new Exception(
                "No writer found for the specified format: '" +
                fileType + "'.");
        }
    }
}