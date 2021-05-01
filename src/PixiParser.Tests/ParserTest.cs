using System;
using System.IO;
using Xunit;

namespace PixiEditor.Parser.Tests
{
    public class ParserTest
    {
        [Fact]
        public void SerializingAndDeserialzingWorks()
        {
            SerializableDocument document = new SerializableDocument
            {
                Height = 1,
                Width = 1
            };

            document.AddSwatch(255, 255, 255);

            byte[] imageData = new byte[] { 255, 255, 255, 255 };

            document.Layers = new SerializableLayer[] { new SerializableLayer() {
                Width = 1, Height = 1,
                MaxHeight = 1, MaxWidth = 1,
                BitmapBytes = imageData,
                IsVisible = true, Name = "Base Layer",
                OffsetX = 0, OffsetY = 0,
                Opacity = 1 } };

            byte[] serialized = PixiParser.Serialize(document);

            SerializableDocument deserializedDocument = PixiParser.Deserialize(serialized);

            Assert.Equal(document.FileVersion, deserializedDocument.FileVersion);
            Assert.Equal(document.Height, deserializedDocument.Height);
            Assert.Equal(document.Width, deserializedDocument.Width);
            Assert.Equal(document.Swatches.Count, deserializedDocument.Swatches.Count);

            Assert.Equal(document.Layers[0].BitmapBytes, deserializedDocument.Layers[0].BitmapBytes);
        }

        [Fact]
        public void DetectOldFile()
        {
            Assert.Throws<OldFileFormatException>(() => PixiParser.Deserialize("./OldPixiFile.pixi"));
        }

        [Fact]
        public void DetectCorruptedFile()
        {
            Assert.Throws<InvalidFileException>(delegate { PixiParser.Deserialize("./CorruptedPixiFile.pixi"); });
        }
    }
}