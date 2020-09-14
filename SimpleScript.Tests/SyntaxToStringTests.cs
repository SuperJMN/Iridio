using FluentAssertions;
using Optional;
using SimpleScript.Binding;
using SimpleScript.Parsing.Model;
using Xunit;

namespace SimpleScript.Tests
{
    public class SyntaxToStringTests
    {
        [Fact]
        public void Block()
        {
            var formatter = new Formatter();
            var result = formatter.Format(CreateBlock(), 0);
            result.Should().Be("{\r\n}");
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
            var condition = new Condition(new IdentifierExpression("a"), new BooleanOperator("=="),
                new IdentifierExpression("b"));
            sut.Visit(new IfStatement(condition, CreateBlock(), CreateBlock().None()));
            sut.ToString().Should().Be("if (a == b)\r\n{\r\n}");
        }

        [Fact]
        public void IfWithElse()
        {
            var sut = new SyntaxStringifyVisitor();
            var condition = new Condition(new IdentifierExpression("a"), new BooleanOperator("=="),
                new IdentifierExpression("b"));
            sut.Visit(new IfStatement(condition, CreateBlock(), CreateBlock().Some()));
            sut.ToString().Should().Be("if (a == b)\r\n{\r\n}\r\nelse\r\n{\r\n}");
        }

        [Fact]
        public void FunctionWithIfElse()
        {
            var sut = new SyntaxStringifyVisitor();
            var condition = new Condition(new IdentifierExpression("a"), new BooleanOperator("=="),
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

        private static FunctionDeclaration GetFunctionDeclaration(params Statement[] statements)
        {
            return new FunctionDeclaration("Main", new Block(statements));
        }
    }
}