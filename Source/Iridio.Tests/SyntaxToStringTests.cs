using FluentAssertions;
using Iridio.Binding;
using Iridio.Parsing;
using Iridio.Parsing.Model;
using Optional;
using Xunit;

namespace Iridio.Tests
{
    public class SyntaxToStringTests
    {
        [Fact]
        public void Block()
        {
            var sut = new SyntaxStringifyVisitor();
            sut.Visit(CreateBlock());
            sut.ToString().Should().Be("{\r\n}");
        }

        [Fact]
        public void BlockWithBlock()
        {
            var sut = new SyntaxStringifyVisitor();
            sut.Visit(CreateBlock(CreateBlock()));
            sut.ToString().Should().Be("{\r\n\t{\r\n\t}\r\n}");
        }

        [Fact]
        public void FunctionDeclaration()
        {
            var sut = new SyntaxStringifyVisitor();
            sut.Visit(GetFunctionDeclaration());
            sut.ToString().Should().Be("Main\r\n{\r\n}");
        }

        [Fact]
        public void IfWithoutElse()
        {
            var sut = new SyntaxStringifyVisitor();
            var condition = new BooleanExpression(new BooleanOperator("=="),new IdentifierExpression("a"),
                new IdentifierExpression("b"));
            sut.Visit(new IfStatement(condition, CreateBlock(), CreateBlock().None()));
            sut.ToString().Should().Be("if (a == b)\r\n{\r\n}");
        }

        [Fact]
        public void IfWithElse()
        {
            var sut = new SyntaxStringifyVisitor();
            var condition = new BooleanExpression(new BooleanOperator("=="), new IdentifierExpression("a"),
                new IdentifierExpression("b"));
            sut.Visit(new IfStatement(condition, CreateBlock(), CreateBlock().Some()));
            sut.ToString().Should().Be("if (a == b)\r\n{\r\n}\r\nelse\r\n{\r\n}");
        }

        [Fact]
        public void FunctionWithIfElse()
        {
            var sut = new SyntaxStringifyVisitor();
            var condition = new BooleanExpression(new BooleanOperator("=="), new IdentifierExpression("a"),
                new IdentifierExpression("b"));
            var ifStatement = new IfStatement(condition, CreateBlock(), CreateBlock().Some());
            var function = GetFunctionDeclaration(ifStatement);
            sut.Visit(function);
            sut.ToString().Should().Be("Main\r\n{\r\n\tif (a == b)\r\n\t{\r\n\t}\r\n\telse\r\n\t{\r\n\t}\r\n}");
        }

        private static Block CreateBlock(params Statement[] statements)
        {
            return new Block(statements);
        }

        private static Procedure GetFunctionDeclaration(params Statement[] statements)
        {
            return new Procedure("Main", new Block(statements));
        }
    }
}