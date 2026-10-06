using SDE.Databases.Generic.Parser;
using SDE.Editor.Database;
using System;

namespace SDE.Databases.ItemGroups.Parser {
    public class ItemGroupParserProvider : DatabaseParserProvider {
        public override DatabaseReader GetReader(FileType fileType) {
            switch (fileType) {
                case FileType.Yaml:
                    return new ItemGroupReaderYaml();
            }

            throw new Exception("No reader found for the specified format: '" + fileType + "'.");
        }

        public override DatabaseWriter GetWriter(FileType fileType) {
            switch (fileType) {
                case FileType.Yaml:
                    return new ItemGroupWriterYaml();
            }

            throw new Exception("No writer found for the specified format: '" + fileType + "'.");
        }
    }
}