using FluentAssertions;
using Iridio.Common.Utils;
using Xunit;

namespace Iridio.Tests
{
    public class LineEatingTests
    {
        [Fact]
        public void On_empty_string_newline_does_nothing()
        {
            var sut = CreateSut();

            sut.NewLine();
            sut.ToString().Should().BeEmpty();
        }

        [Fact]
        public void Newline_print_shows_print_content()
        {
            var sut = CreateSut();

            sut.NewLine();
            sut.Print("Hello");
            sut.ToString().Should().Be("Hello");
        }


        [Fact]
        public void Newline_print_newline_gives_print_content()
        {
            var sut = CreateSut();

            sut.NewLine();
            sut.Print("Hello");
            sut.NewLine();
            sut.ToString().Should().Be("Hello");
        }

        [Fact]
        public void Hello_boy_naked()
        {
            var sut = CreateSut();

            sut.Print("Hello");
            sut.NewLine();
            sut.Print("Boy");
            sut.ToString().Should().Be("Hello\r\nBoy");
        }

        [Fact]
        public void Hello_boy_decorated()
        {
            var sut = CreateSut();

            sut.NewLine();
            sut.Print("Hello");
            sut.NewLine();
            sut.Print("Boy");
            sut.NewLine();
            sut.ToString().Should().Be("Hello\r\nBoy");
        }

        [Fact]
        public void Multiple_newlines_do_nothing()
        {
            var sut = CreateSut();

            sut.NewLine();
            sut.NewLine();
            sut.NewLine();
            sut.ToString().Should().BeEmpty();
        }

        private static LineEatingStringAssistant CreateSut()
        {
            return new LineEatingStringAssistant(new StringAssistant());
        }
    }
}