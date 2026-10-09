using SDE.Databases.Generic.Parser;
using SDE.Editor.Database;
using System;

namespace SDE.Databases.ItemRandomOptionGroups.Parser
{
    public class ItemRandomOptionGroupParserProvider
        : DatabaseParserProvider
    {
        public override DatabaseReader GetReader(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Yaml:
                    return new ItemRandomOptionGroupReaderYaml();
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
                    return new ItemRandomOptionGroupWriterYaml();
            }

            throw new Exception(
                "No writer found for the specified format: '" +
                fileType + "'.");
        }
    }
}