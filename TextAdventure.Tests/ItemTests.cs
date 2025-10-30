using Xunit;
using Maya_Naila_Jonas_Maddie_TextAdventure;
using Assert = Xunit.Assert;

namespace TextAdventure.Tests
{
    public class ItemTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            string id = "sword";
            string name = "Sword";
            string description = "A sharp blade";

            var item = new Item(id, name, description);

            Assert.Equal(id, item.Id);
            Assert.Equal(name, item.Name);
            Assert.Equal(description, item.Description);
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            var item = new Item("key", "Key", "Opens a locked door");
            string result = item.ToString();
            Assert.Equal("Key (key): Opens a locked door", result);
        }
    }
}
