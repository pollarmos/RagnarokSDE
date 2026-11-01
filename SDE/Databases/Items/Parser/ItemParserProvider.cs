using SDE.Databases.Generic.Parser;
using SDE.Editor.Database;
using System;

namespace SDE.Databases.Items.Parser {
	public class ItemParserProvider : DatabaseParserProvider {
        private readonly Func<bool> _isImport;

        public ItemParserProvider(Func<bool> isImport = null) {
            _isImport = isImport;
        }

		public override DatabaseReader GetReader(FileType fileType) {
			switch (fileType) {
				case FileType.Yaml:
					return new ItemReaderYaml();
				case FileType.Txt:
					return new ItemReaderCsv();
			}

			throw new Exception("No reader found for the specified format: '" + fileType + "'.");
		}

		public override DatabaseWriter GetWriter(FileType fileType) {
			switch (fileType) {
				case FileType.Yaml:
					return new ItemWriterYaml(_isImport?.Invoke() ?? false);
				case FileType.Txt:
					return new ItemWriterCsv();
			}

			throw new Exception("No writer found for the specified format: '" + fileType + "'.");
		}
	}
}
